"""Audita las relaciones documentales y las imágenes locales del pack. Requiere Pillow."""
from collections import Counter
from hashlib import sha256
import json
from pathlib import Path
import re
from urllib.parse import unquote, urlsplit

from PIL import Image


DESIGN = Path(__file__).resolve().parent
DOCS = DESIGN.parent
REQ = r"REQ-[A-Z0-9]+-\d{3}"


def read(path):
    return path.read_text(encoding="utf-8-sig")


def table_rows(path):
    for line in read(path).splitlines():
        if line.startswith("|") and line.endswith("|"):
            yield [cell.strip() for cell in line[1:-1].split("|")]


def require(condition, message):
    if not condition:
        raise ValueError(message)


def id_set(filename, pattern):
    return set(re.findall(pattern, read(DESIGN / filename)))


def verify_trace():
    catalog = {r[0]: r[1] for r in table_rows(DOCS / "to-be/02-catalogo-funcional.md")
               if re.fullmatch(REQ, r[0])}
    ancestors = {}
    for row in table_rows(DOCS / "to-be/18-trazabilidad-as-is-to-be.md"):
        match = re.search(r"FPA-FUNC-\d{3}", row[0])
        if match:
            ancestors[row[1]] = match.group() + " · " + row[2]
    cases = {req: set() for req in catalog}
    for filename in ["01-equivalencia-funcional.md", "02-casos-de-prueba.md"]:
        for row in table_rows(DOCS / "testing" / filename):
            if re.fullmatch(r"(EQ|NV)-\d+", row[0]):
                for req in re.findall(REQ, row[1]):
                    cases[req].add(row[0])
    # Las vistas de una misma familia también pueden compartir un encabezado.
    screens = set()
    for line in read(DESIGN / "07-screen-design-candidates.md").splitlines():
        if line.startswith(("### V", "| V")):
            screens.update(re.findall(r"V\d{2}", line.split("·")[0]))
    components = id_set("08-component-candidates.md", r"(?m)^\| (C\d{2})")
    criteria = id_set("11-ux-validation-criteria.md", r"(?m)^\| (U\d{2})")
    sources = id_set("02-product-benchmark.md", r"(?m)^\| (B\d{2})")
    manifest = json.loads(read(DESIGN / "references/manifest.json"))
    sources.update(item["id"] for item in manifest["recursos"])
    matrix = [r for r in table_rows(DESIGN / "13-traceability-to-to-be.md")
              if re.fullmatch(REQ, r[0])]
    require(Counter(r[0] for r in matrix) == Counter(catalog.keys()), "Cobertura o ID duplicado")
    for req, phase, predecessor, need, view, part, refs, ux, tests in matrix:
        require(phase == catalog[req] and bool(need), req + " · hito/necesidad")
        require(predecessor == ancestors.get(req, "Transversal/nueva"), req + " · antecedente")
        require(set(tests.split(", ")) == cases[req] and bool(cases[req]), req + " · casos")
        if view != "Transversal, sin vista propia":
            views = re.findall(r"V\d{2}", view)
            require(views and set(views) <= screens, req + " · " + view)
        for cell, pattern, allowed in [(part, r"C\d{2}", components), (ux, r"U\d{2}", criteria)]:
            ids = re.findall(pattern, cell)
            require(ids and set(ids) <= allowed, req + " · " + cell)
        require(bool(refs) and set(re.findall(r"\b[BRDE]\d{2}\b", refs)) <= sources,
                req + " · referencia")
    return len(matrix), len(ancestors), len(screens), len(components)


def verify_assets():
    manifest = json.loads(read(DESIGN / "references/manifest.json"))
    identifiers, hashes, files = set(), set(), set()
    required = ("id", "producto", "dominio", "fuente", "fecha_consulta", "pantalla",
                "procedencia", "patron", "observacion", "condiciones", "restricciones")
    for item in manifest["recursos"]:
        require(all(item.get(key) for key in required), "Metadatos incompletos")
        require(item["id"] not in identifiers, item["id"] + " · ID duplicado")
        identifiers.add(item["id"])
        if item["archivo"] is None:
            continue
        path = (DESIGN / item["archivo"]).resolve()
        digest = sha256(path.read_bytes()).hexdigest()
        require(digest == item["sha256"] and digest not in hashes, str(path))
        with Image.open(path) as img:
            img.load()
            require(img.size == (item["ancho"], item["alto"]), str(path))
        files.add(path)
        hashes.add(digest)
    packaged = {p.resolve() for p in (DESIGN / "references").rglob("*")
                if p.suffix.lower() in {".png", ".webp", ".jpg", ".jpeg", ".svg", ".gif"}}
    require(packaged <= files, "Recurso local fuera del manifiesto")
    return len(identifiers), len(files)


def verify_links():
    links = 0
    for path in sorted(DESIGN.glob("*.md")):
        for target in re.findall(r"\]\(([^)\s]+)\)", read(path)):
            uri = urlsplit(target)
            if uri.scheme or not uri.path:
                continue
            require((path.parent / unquote(uri.path)).is_file(), f"{path.name} → {target}")
            links += 1
    return links


if __name__ == "__main__":
    reqs, ancestors, screens, components = verify_trace()
    entries, images = verify_assets()
    print(f"{reqs} requisitos, {ancestors} antecedentes, {screens} superficies, "
          f"{components} candidatos a componentes")
    print(f"{entries} referencias del manifiesto, {images} imágenes decodificadas y verificadas")
    print(f"{verify_links()} enlaces locales resueltos")
