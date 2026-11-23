"use strict";
const listNames = {
  squad: "Plantilla",
  db: "Base de jugadores",
  scout: "Ojeo",
  watch: "Seguimiento",
  youth: "Desarrollo y cantera",
};
const listKey = () =>
  route.name +
  (route.name === "watch" ? String(route.list ?? "all") : "") +
  (route.context ? ":" + route.context.pos : "");
function filters() {
  return (state().filters[listKey()] ??= {
    q: "",
    pos: route.context?.pos || "",
    role: "",
    status: "",
    age: "",
    value: "",
    dribbling: "",
    possible: false,
    sort: "id",
    desc: false,
    page: 0,
  });
}
const initialRows = () =>
  ({
    squad: own(),
    db: people(),
    scout: people().filter((p) => p.group === "Exterior" && !p.stats),
    watch: people().filter((p) =>
      (route.list == null ? state().watch : state().watchlists[route.list]?.ids || []).includes(p.id),
    ),
    youth: own().filter((p) => p.age <= 21),
  })[route.name];
const lightStatus = (p) =>
  p.status === "Lesionado" ? "rose" : p.status === "Cedido" ? "gold" : p.status === "Apto" ? "green" : "";
function rowStatus(p) {
  if (route.name === "squad" && p.group !== "Primer equipo") return p.group + " · " + p.status;
  if (route.name === "scout" && filters().dribbling) {
    const v = attr(p, "dribbling");
    return Array.isArray(v) && v[0] < +filters().dribbling ? "Posible" : "Seguro";
  }
  if (route.name === "scout") return Object.keys(p.attrs).length ? "Ojeo parcial" : "Sin observar";
  return p.status;
}
const selected = () => (state().selection[listKey()] ??= []);
const normalize = (s) =>
  String(s)
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .toLowerCase();
function listRows() {
  const f = filters(),
    rows = initialRows().filter((p) => {
      const v = attr(p, "dribbling"),
        limit = Array.isArray(v) ? v[f.possible ? 1 : 0] : typeof v === "number" ? v : null;
      return (
        normalize(p.name + " " + p.club).includes(normalize(f.q)) &&
        (!f.pos || p.pos.includes(f.pos)) &&
        (!f.role || p.pos.some((pos) => ["IP", "OOP"].some((phase) => role(pos, phase) === f.role))) &&
        Object.keys(state().permissions).every(
          (k) => !state().permissions[k] || !f[k] || (special(p)[k] != null && special(p)[k] >= +f[k]),
        ) &&
        (!f.status || p.status === f.status) &&
        (!f.age || p.age <= +f.age) &&
        (!f.value || (p.value != null && p.value <= +f.value * 1e6)) &&
        (!f.dribbling || (limit != null && limit >= +f.dribbling))
      );
    });
  const sort = f.sort === "name" || authorizedColumns().includes(f.sort) ? f.sort : "id";
  const config = evaluationConfig();
  const val = (p) => {
    const value = columnValue(p, sort, config);
    return Array.isArray(value) ? value[0] : (value?.estimate ?? value);
  };
  return rows.sort((a, b) => {
    const x = val(a),
      y = val(b);
    if (x == null || y == null) return Number(x == null) - Number(y == null) || a.id - b.id;
    return (typeof x === "string" ? x.localeCompare(y, "es") : x - y) * (f.desc ? -1 : 1) || a.id - b.id;
  });
}
const columnNames = {
  age: "Edad",
  pos: "Posiciones",
  group: "Vinculación",
  fit: "Encaje de táctica /100",
  dribbling: "Regate /20",
  value: "Valor de mercado",
  salary: "Sueldo semanal",
  end: "Vencimiento",
  pass: "Pase /20",
  CA: "CA",
  PA: "PA",
};
const tableDefaults = {
  squad: ["pos", "age", "currentIP", "currentOOP", "end", "salary", "CA", "PA"],
  db: ["pos", "age", "currentIP", "dribbling", "value", "salary", "CA", "PA"],
  scout: ["age", "currentIP", "currentOOP", "dribbling", "value", "salary", "CA", "PA"],
  watch: ["pos", "age", "currentIP", "value", "salary", "CA", "PA"],
  youth: ["group", "age", "currentIP", "futureIP", "dribbling", "CA", "PA"],
};
const preferenceKey = "fm27.columns.v2";
const allColumnKeys = [...Object.keys(columnNames), ...Object.keys(qualityColumns)];
let tablePreferences;
try {
  tablePreferences = JSON.parse(localStorage.getItem(preferenceKey)) || {};
} catch {
  tablePreferences = {};
}
if (typeof tablePreferences !== "object" || Array.isArray(tablePreferences)) tablePreferences = {};
const tablePreferenceId = () => career + ":" + route.name;
function preferredColumns() {
  const stored = tablePreferences[tablePreferenceId()];
  return Array.isArray(stored)
    ? [...new Set(stored.filter((k) => allColumnKeys.includes(k)))]
    : tableDefaults[route.name];
}
function storeColumns(columns) {
  tablePreferences[tablePreferenceId()] = columns;
  try {
    localStorage.setItem(preferenceKey, JSON.stringify(tablePreferences));
  } catch {
    /* Retener en esta sesión. */
  }
}
function clearTablePreferences() {
  tablePreferences = {};
  try {
    localStorage.removeItem(preferenceKey);
  } catch {
    /* El navegador puede bloquear file:. */
  }
}
function columnAllowed(key) {
  if (key === "CA" || key === "PA") return career === "Faro" && state().permissions[key];
  return qualityColumns[key]?.kind !== "future" || projectionAllowed();
}
function authorizedColumns() {
  return preferredColumns().filter(columnAllowed);
}
function columnLabel(key, config, excel = false) {
  const quality = qualityColumns[key];
  if (quality) return quality.label + " · " + config[quality.phase] + " (%)";
  return (
    (excel
      ? { age: "Edad (años)", value: "Valor de mercado (EUR)", salary: "Sueldo semanal (EUR/semana)" }[key]
      : null) || columnNames[key]
  );
}
function columnValue(p, key, config) {
  const quality = qualityColumns[key];
  if (quality) return evaluateRole(p, config, quality.phase)[quality.kind];
  if (key === "CA" || key === "PA") return special(p)[key] ?? null;
  if (key === "pos") return p.pos.join(" / ");
  if (key === "fit") return score(p);
  if (key === "pass") return attr(p, "passing");
  if (key === "dribbling") return attr(p, "dribbling");
  return p[key] ?? null;
}
function cell(p, key, config) {
  const value = columnValue(p, key, config);
  if (qualityColumns[key]) return qualityText(value);
  if (key === "age") return value + " años";
  if (key === "value") return money(value);
  if (key === "salary") return wage(p);
  if (key === "end") return value?.split("-").reverse().join("/") || unknown;
  if (key === "fit") return value == null ? "Sin nota" : fmt(value);
  return attrText(value);
}
const pageSize = 16;
const idOptions = (pool) => pool.map((p) => [p.id, p.name + " · " + p.pos[0]]);
function listView() {
  const f = filters(),
    rows = listRows(),
    picked = selected(),
    cols = authorizedColumns(),
    config = evaluationConfig(),
    n = pageSize;
  f.page = Math.min(f.page, Math.max(0, Math.ceil(rows.length / n) - 1));
  const scouting = route.name === "scout";
  const context = route.context,
    columns = cols.map((c) => columnLabel(c, config));
  const posOptions = [...new Set(initialRows().flatMap((p) => p.pos))].sort();
  const roleOptions = [
    ...new Set(posOptions.flatMap((pos) => ["IP", "OOP"].map((phase) => role(pos, phase)))),
  ].sort();
  const controls = `<div class="toolbar"><label>Buscar jugador<input type="search" data-live="q" value="${esc(f.q)}" placeholder="Nombre o club"></label>
 ${select("Posición", "filter:pos", [["", "Todas"], ...posOptions], f.pos)}${scouting ? select("Rol por fase", "filter:role", [["", "Todos"], ...roleOptions], f.role) : select("Disponibilidad", "filter:status", [["", "Todas"], ...new Set(initialRows().map((p) => p.status))], f.status)}
 ${btn(f.advanced ? "Cerrar filtros" : "Más filtros", "advanced", "", "profiles")}${btn("Columnas", "columns", "plain", "list")}</div>`;
  const advanced = f.advanced
    ? `<div class="toolbar">${input("Edad máxima", "filter:age", f.age, "number", 'min="15" max="55"')}${input("Valor máximo (M€)", "filter:value", f.value, "number", 'min="0" step="0.1"')}${input("Regate mínimo /20", "filter:dribbling", f.dribbling, "number", 'min="1" max="20"')}${select(
        "Evidencia",
        "certainty",
        [
          [0, "Sólo seguros"],
          [1, "Incluir posibles"],
        ],
        +f.possible,
      )}
 ${Object.keys(state().permissions)
   .filter((k) => state().permissions[k])
   .map((k) => input(k + " mínima", "filter:" + k, f[k] || "", "number", 'min="1" max="200"'))
   .join("")}
 ${btn("Guardar búsqueda", "saveFilter")}${state().saved[listKey()] ? btn("Recuperar búsqueda", "loadFilter") : ""}${btn("Limpiar", "clearFilter")}</div>`
    : "";
  const listTools =
    route.name === "watch"
      ? `<div class="toolbar">${select("Lista de seguimiento", "activeList", [["all", "Todos los seguidos"], ...state().watchlists.map((l, i) => [i, l.name])], route.list ?? "all")}${btn("Nueva lista", "createList")}${picked.length && state().watchlists.length ? btn("Copiar selección a lista", "copyWatch") : ""}</div>`
      : "";
  const data = rows
    .slice(n * f.page, n * (f.page + 1))
    .map((p) => [
      `<input type="checkbox" aria-label="Seleccionar ${esc(p.name)}" data-change="pick:${p.id}" ${picked.includes(p.id) ? "checked" : ""}>`,
      personBtn(p),
      chip(rowStatus(p), lightStatus(p)),
      ...cols.map((c) => esc(cell(p, c, config))),
      scouting && context
        ? btn("Comparar", "contextPair:" + p.id, "link", "compare")
        : btn(state().watch.includes(p.id) ? "Quitar" : "Seguir", "follow:" + p.id, "link", "watch"),
    ]);
  const sortable = (name, key) =>
    btn(name + (f.sort === key ? (f.desc ? " ↓" : " ↑") : ""), "sort:" + key, "sort");
  return (
    heading(
      listNames[route.name],
      (route.name === "youth"
        ? "Jugadores vinculados de hasta 21 años"
        : route.name === "watch"
          ? "Lista de observación de la carrera"
          : club() + " · " + today()) + " · Datos ficticios",
      scouting ? btn("Rendimiento observado", "go:stats", "", "stats") : "",
    ) +
    (context
      ? `<div class="context"><b>Refuerzo para ${context.pos}</b> · ${person(context.id).name} · ${context.form} · ${role(context.pos, "IP")} / ${role(context.pos, "OOP")}</div>`
      : "") +
    `<section class="panel player-table">${listTools}${controls}${advanced}${evaluationControls(config)}<div class="split table-tools"><span><b>${rows.length}</b> jugadores · ${picked.length} seleccionados</span><div class="actions">${picked.length >= 2 ? btn("Comparar selección", "compareSelected", "primary", "compare") : ""}${picked.length ? btn("Seguir selección", "followSelection") : ""}${btn("Exportar a Excel", "export", "", "list")}</div></div>
 ${rows.length ? tbl(["", sortable("Jugador", "name"), "Estado", ...cols.map((c, i) => sortable(columns[i], c)), "Acción"], data) : empty(route.name === "watch" ? "La lista está vacía. Selecciona Seguir en una ficha o en Ojeo." : undefined)}
 <div class="panel-foot"><span>Valoraciones demostrativas · ${config.pos} · ${evaluationProfiles[config.profile]} ${!projectionAllowed() ? "· La proyección requiere CA y PA" : ""}</span><div class="actions">${f.page ? btn("Anterior", "page:-1") : ""}<span>Página ${f.page + 1} / ${Math.max(1, Math.ceil(rows.length / n))}</span>${(f.page + 1) * n < rows.length ? btn("Siguiente", "page:1") : ""}</div></div></section>`
  );
}
Object.keys(listNames).forEach((k) => (views[k] = listView));
document.addEventListener("input", (e) => {
  if (e.target.dataset.live === "q") {
    const caret = e.target.selectionStart;
    filters().q = e.target.value;
    filters().page = 0;
    render();
    const el = $("[data-live=q]");
    el.focus();
    el.setSelectionRange(caret, caret);
  }
});
Object.assign(changes, {
  filter: (k, el) => {
    if (!el.reportValidity()) return;
    filters()[k] = el.value;
    filters().page = 0;
    render();
  },
  certainty: (el) => {
    filters().possible = el.value === "1";
    filters().page = 0;
    render();
  },
  pick: (id, el) => {
    id = +id;
    let ids = selected();
    if (el.checked && ids.length >= 5) {
      el.checked = false;
      return tell("Puedes comparar hasta cinco jugadores.");
    }
    state().selection[listKey()] = el.checked ? [...ids, id] : ids.filter((x) => x !== id);
    render();
  },
  column: (key, el) => {
    const columns = preferredColumns();
    storeColumns(el.checked ? [...columns, key] : columns.filter((k) => k !== key));
    render();
    columnsDialog();
    $("#modal [data-change='column:" + key + "']")?.focus();
  },
  exportScope: (el) => exportPreview(el.value),
});
function columnsDialog() {
  const visible = authorizedColumns(),
    config = evaluationConfig();
  const entries = [...visible, ...allColumnKeys.filter((k) => columnAllowed(k) && !visible.includes(k))];
  dialog(
    "Columnas de " + listNames[route.name],
    `<div class="column-editor">${entries
      .map((key) => {
        const index = visible.indexOf(key),
          label = columnLabel(key, config);
        return `<div><label class="check"><input type="checkbox" data-change="column:${key}" ${index >= 0 ? "checked" : ""}>${esc(label)}</label>
    ${index > 0 ? btn("Subir " + label, "moveColumn:" + key + ":-1", "plain") : ""}
    ${index >= 0 && index < visible.length - 1 ? btn("Bajar " + label, "moveColumn:" + key + ":1", "plain") : ""}</div>`;
      })
      .join("")}</div>${btn("Restaurar columnas", "resetColumns")}`,
  );
}
actions.moveColumn = (key, step) => {
  const columns = [...preferredColumns()],
    visible = authorizedColumns();
  const target = visible[visible.indexOf(key) + Number(step)];
  if (!target) return;
  const i = columns.indexOf(key),
    j = columns.indexOf(target);
  [columns[i], columns[j]] = [columns[j], columns[i]];
  storeColumns(columns);
  render();
  columnsDialog();
  (
    $("#modal [data-act='moveColumn:" + key + ":" + step + "']") ??
    $("#modal [data-change='column:" + key + "']")
  )?.focus();
};
actions.resetColumns = () => {
  storeColumns([...tableDefaults[route.name]]);
  render();
  columnsDialog();
};

function exportRows(scope) {
  const rows = listRows();
  if (scope === "page") return rows.slice(filters().page * pageSize, (filters().page + 1) * pageSize);
  if (scope === "selection") return rows.filter((p) => selected().includes(p.id));
  return rows;
}
function exportPreview(scope = "all") {
  const cols = authorizedColumns(),
    config = evaluationConfig(),
    rows = exportRows(scope);
  const options = [
    ["all", "Todos los resultados filtrados"],
    ["page", "Página visible"],
  ];
  if (selected().length) options.push(["selection", "Selección dentro del filtro"]);
  dialog(
    "Exportar a Excel",
    select("Filas que se exportarán", "exportScope", options, scope) +
      `<p class="context">${rows.length} filas · Orden y columnas de la vista · Datos ficticios</p>` +
      btn("Descargar XLSX", "downloadList:" + scope, "primary") +
      tbl(
        ["Jugador", "Estado", ...cols.map((c) => columnLabel(c, config))],
        rows.map((p) => [esc(p.name), rowStatus(p), ...cols.map((c) => esc(cell(p, c, config)))]),
      ),
  );
}
function excelValue(p, key, config) {
  const value = columnValue(p, key, config);
  if (qualityColumns[key]) return percentCell(value);
  if (value == null) return null;
  if (Array.isArray(value) || typeof value === "object") return attrText(value);
  if (key === "end") return { value: new Date(value + "T00:00:00Z"), format: "dd/mm/yyyy" };
  return value;
}
actions.downloadList = (scope) => {
  const cols = authorizedColumns(),
    config = evaluationConfig(),
    f = filters();
  const rows = exportRows(scope);
  const applied = [
    ["Búsqueda", f.q],
    ["Posición", f.pos],
    ["Rol", f.role],
    ["Disponibilidad", f.status],
    ["Edad máxima", f.age],
    ["Valor máximo (M€)", f.value],
    ["Regate mínimo", f.dribbling],
    ...["CA", "PA"].filter(columnAllowed).map((k) => [k + " mínima", f[k]]),
  ]
    .filter(([, value]) => value != null && value !== "")
    .map(([key, value]) => key + " = " + value);
  if (f.dribbling) applied.push(f.possible ? "Incluir posibles" : "Sólo seguros");
  const sort = f.sort === "name" || cols.includes(f.sort) ? f.sort : "id";
  exportExcel(
    listNames[route.name],
    ["Jugador", "Estado", ...cols.map((c) => columnLabel(c, config, true))],
    rows.map((p) => [p.name, rowStatus(p), ...cols.map((c) => excelValue(p, c, config))]),
    [
      [
        "Ámbito",
        { all: "Resultados filtrados", page: "Página " + (f.page + 1), selection: "Selección filtrada" }[
          scope
        ],
      ],
      ["Filtros", applied.join(" · ") || "Sin filtros"],
      [
        "Orden",
        sort === "id"
          ? "UID ascendente"
          : (sort === "name" ? "Jugador" : columnLabel(sort, config)) +
            (f.desc ? " descendente" : " ascendente"),
      ],
      ...evaluationMetadata(
        config,
        cols.some((c) => qualityColumns[c]?.kind === "future"),
      ),
    ],
  );
};
function follow(id) {
  const s = state();
  if (route.name === "watch" && route.list != null) {
    const list = s.watchlists[route.list];
    list.ids = list.ids.filter((x) => x !== id);
    return;
  }
  if (s.watch.includes(id)) {
    s.watch = s.watch.filter((x) => x !== id);
    for (const list of s.watchlists) list.ids = list.ids.filter((x) => x !== id);
  } else s.watch.push(id);
}
changes.activeList = (el) => {
  route.list = el.value === "all" ? null : +el.value;
  render();
};
actions.createList = () =>
  dialog(
    "Nueva lista de seguimiento",
    '<label>Nombre de la lista<input id="listName" maxlength="50"></label><div class="actions">' +
      btn("Crear lista", "saveList", "primary") +
      "</div>",
  );
actions.saveList = () => {
  const name = $("#listName").value.trim();
  if (!name) return $("#listName").focus();
  state().watchlists.push({ name, ids: [] });
  route.list = state().watchlists.length - 1;
  closeDialog();
  render();
};
actions.copyWatch = () =>
  dialog(
    "Copiar selección",
    select(
      "Lista de destino",
      "copyTarget",
      [["", "Elegir lista"], ...state().watchlists.map((l, i) => [i, l.name])],
      "",
    ),
  );
changes.copyTarget = (el) => {
  if (el.value === "") return;
  const list = state().watchlists[+el.value];
  list.ids = [...new Set([...list.ids, ...selected()])];
  closeDialog();
  render();
  tell("Selección copiada a " + list.name);
};
Object.assign(actions, {
  advanced: () => {
    filters().advanced = !filters().advanced;
    render();
  },
  clearFilter: () => {
    delete state().filters[listKey()];
    render();
  },
  columns: columnsDialog,
  saveFilter: () => {
    state().saved[listKey()] = structuredClone(filters());
    tell("Búsqueda guardada en " + career);
  },
  loadFilter: () => {
    state().filters[listKey()] = structuredClone(state().saved[listKey()]);
    render();
  },
  sort: (key) => {
    const f = filters();
    f.desc = f.sort === key ? !f.desc : false;
    f.sort = key;
    f.page = 0;
    render();
  },
  page: (delta) => {
    filters().page += +delta;
    render();
  },
  export: () => exportPreview(),
  follow: (id) => {
    const custom = route.name === "watch" && route.list != null;
    follow(+id);
    if (route.name === "watch") state().selection[listKey()] = selected().filter((x) => x !== +id);
    render();
    tell(
      custom
        ? "Retirado de la lista"
        : state().watch.includes(+id)
          ? "Añadido a seguimiento"
          : "Retirado de seguimiento",
    );
  },
  followSelection: () => {
    state().watch = [...new Set([...state().watch, ...selected()])];
    tell("Selección añadida a seguimiento");
    render();
  },
  compareSelected: () => go("compare", { ids: [...selected()], evaluation: { ...evaluationConfig() } }),
  contextPair: (id) =>
    go("compare", {
      ids: [route.context.id, +id],
      context: route.context,
      evaluation: { ...evaluationConfig() },
    }),
  compareFrom: (id) => {
    dialog(
      "Comparar con",
      select(
        "Segundo jugador",
        "second",
        [[0, "Elegir jugador"], ...idOptions(people().filter((p) => p.id !== +id))],
        0,
      ),
    );
    route.reference = +id;
  },
  ptab: (tab) => {
    route.tab = tab;
    render();
  },
});
changes.second = (el) => {
  if (+el.value) go("compare", { ids: [route.reference, +el.value], evaluation: { ...evaluationConfig() } });
};
function attributes(p) {
  const groups = ["Técnica", "Mental", "Física", "Portería"];
  return `<div class="attrgrid">${groups
    .map((group) =>
      panel(
        group,
        terms
          .filter((t) => t[2] === group)
          .map(([k]) => {
            const v = p.attrs[k],
              label = attrText(v);
            return `<div class="attribute"><span>${esc(termName(k))}</span><b class="${v == null ? "muted" : Array.isArray(v) ? "range" : "mint"}">${label}</b></div>`;
          })
          .join(""),
      ),
    )
    .join("")}</div>`;
}
function fitPanel(p) {
  const config = evaluationConfig();
  const known = Object.keys(p.attrs).length;
  const orderedBands = terms
    .filter(([k]) => p.attrs[k] != null)
    .map(([k]) => {
      const v = p.attrs[k],
        low = Array.isArray(v) ? v[0] : typeof v === "number" ? v : v.estimate,
        high = Array.isArray(v) ? v[1] : low;
      return { k, low, high, v };
    })
    .sort((a, b) => b.low - a.low);
  const evidence = orderedBands
    .slice(0, 4)
    .map(
      ({ k, low, high, v }) =>
        `<div class="observation"><div><b>${esc(termName(k))}</b><strong>${attrText(v)} /20</strong></div><div class="scale"><i style="width:${low * 5}%"></i><i class="interval" style="width:${(high - low) * 5}%"></i></div></div>`,
    )
    .join("");

  return `<div class="columns dossier">${panel(
    "Calidad por posición y rol",
    evaluationControls(config) +
      roleQualityTable([p], config) +
      pair(
        "Familiaridad actual en " + config.pos,
        positionalFamiliarity(p, config.pos) == null ? unknown : positionalFamiliarity(p, config.pos) + "/20",
      ) +
      pair("Atributos con evidencia", known + " de 47") +
      `<p class="note">Valoraciones demostrativas · FM27 · Códigos de rol de referencia.</p>`,
  )}
 ${panel(
   visualDirection === "pro" ? "Evidencia de ojeo" : "Lectura del jugador",
   evidence +
     pair(
       "Mayor mínimo observado",
       orderedBands.length
         ? termName(orderedBands[0].k) + " · " + attrText(orderedBands[0].v) + "/20"
         : unknown,
     ) +
     pair(
       "Menor mínimo observado",
       orderedBands.length > 1
         ? termName(orderedBands.at(-1).k) + " · " + attrText(orderedBands.at(-1).v) + "/20"
         : unknown,
     ) +
     pair("Regate /20", attrText(attr(p, "dribbling"))) +
     pair("Pase /20", attrText(attr(p, "passing"))) +
     Object.entries(special(p))
       .map(([k, v]) => pair(k + " ficticia", v))
       .join("") +
     `<p class="note">Nota predefinida para el puesto. Los intervalos conservan ambos extremos.</p>`,
 )}</div>`;
}
function playerView() {
  const p = person(route.id),
    tab = route.tab || "Resumen",
    s = state();
  const head = `<div class="identity">${portrait(p)}<div><div class="eyebrow">${p.club} · UID ${p.id}</div><h1>${esc(p.name)}</h1><p>${p.pos.join(" / ")} · ${p.age} años · Pie ${p.foot?.toLowerCase() || "sin observar"}</p></div>
 <div class="actions">${btn(s.watch.includes(p.id) ? "Quitar seguimiento" : "Seguir", "follow:" + p.id, "", "watch")}${people().length > 1 ? btn("Comparar", "compareFrom:" + p.id, "primary", "compare") : ""}</div></div>`;
  let body = "";
  if (tab === "Resumen") body = fitPanel(p);
  if (tab === "Atributos") body = attributes(p);
  if (tab === "Contrato")
    body = panel(
      "Condiciones vigentes",
      pair("Club", p.club) +
        pair("Valor de mercado de ejemplo", money(p.value)) +
        pair("Sueldo vigente", wage(p)) +
        pair("Vencimiento", cell(p, "end")) +
        pair("Precio observado", unknown) +
        (squad.includes(p) || market.includes(p) ? btn("Simular operación", "deal:" + p.id, "primary") : ""),
    );
  if (tab === "Evolución") body = views.history ? historyBody(p) : empty("Sin observaciones en esta fecha.");
  if (tab === "Estadísticas")
    body = p.stats
      ? statDetail(p)
      : panel(
          "Rendimiento observado",
          empty("Sin minutos registrados para este jugador en las temporadas de demostración.") +
            btn("Explorar cohorte", "go:stats", "link", "arrow"),
        );
  return (
    head +
    tabs(["Resumen", "Atributos", "Estadísticas", "Evolución", "Contrato"], tab, "ptab") +
    body +
    `<p class="note">FM27 · Datos de demostración · ${today()} · Carrera ${career}. Etiquetas pendientes de cotejo con el juego.</p>`
  );
}
views.player = playerView;
function roleQualityTable(players, config) {
  const projections = projectionAllowed();
  const rates = ["IP", "OOP"]
    .map((phase) => {
      const ratings = players.map((p) => evaluateRole(p, config, phase));
      const rateRow = (kind, label) => {
        const values = ratings.map((r) => r[kind]);
        return [
          label,
          ...ratings.map((result, i) => {
            const value = values[i];
            const delta = value == null ? null : values[0] == null ? null : value - values[0];
            const label = kind === "future" && result.reason ? result.reason : qualityText(value);
            return `<span class="role-value">${label}</span>${
              i && delta != null
                ? `<span class="delta ${delta > 0 ? "gain" : delta < 0 ? "loss" : ""}">${phaseDifference(value, values[0])}</span>`
                : ""
            }`;
          }),
        ];
      };
      return `<section class="phase-quality ${phase.toLowerCase()}"><h3>${phases[phase]} <span>${config[phase]}</span></h3>
    ${tbl(
      ["Indicador", ...players.map((p) => esc(p.name))],
      [rateRow("current", "Actual"), ...(projections ? [rateRow("future", "Proyección")] : [])],
    )}</section>`;
    })
    .join("");
  return `<div class="quality-matrix">${rates}</div><div class="quality-note">
  <span>${players.length > 1 ? "Δ frente al referente en puntos porcentuales (pp). " : ""}${projections ? "Proyección con familiaridad hipotética 20/20." : "Proyección indisponible · requiere habilitar CA y PA."}</span>
  ${btn("Supuestos de la proyección", "projection", "link")}</div>`;
}
function compareView() {
  const players = route.ids.map(person),
    reference = players[0],
    config = evaluationConfig();
  const cohort = route.cohort;
  const rows = cohort
    ? performanceIndicators(cohort)
    : [
        ["Posiciones", (p) => p.pos.join(" / ")],
        [
          "Familiaridad actual en " + config.pos,
          (p) => {
            const value = positionalFamiliarity(p, config.pos);
            return value == null ? unknown : value + "/20";
          },
        ],
        ["Regate /20", (p) => attrText(attr(p, "dribbling"))],
        ["Pase /20", (p) => attrText(attr(p, "passing"))],
        ["Edad", (p) => p.age],
        ["Valor de mercado de ejemplo", (p) => money(p.value)],
        ["Sueldo vigente", wage],
        ...["CA", "PA"].filter(columnAllowed).map((k) => [k + " ficticia", (p) => special(p)[k] ?? unknown]),
      ];
  const challenger = players[1];
  const samePosition = reference.pos.some((pos) => challenger.pos.includes(pos));
  const canSimulate = market.includes(challenger) && own().includes(reference) && samePosition;
  const tactical =
    route.context && canSimulate && score(reference) != null && score(challenger) != null
      ? `<div class="context"><b>Escenario táctico ${route.context.form}</b> · ${route.context.pos} ·
      Índice del XI de ejemplo al 50 % por fase: ${fmt(score(reference))} → ${fmt(score(challenger))}/100
      (${sign(score(challenger) - score(reference))} puntos). ${btn("Simular refuerzo", "simulate:" + reference.id + ":" + challenger.id, "link", "market")}</div>`
      : "";
  return (
    heading(
      "Comparador de jugadores",
      cohort
        ? statSeason(reference) + " · Población fija de " + cohort.ids.length
        : "Valoraciones demostrativas · " + today(),
      btn("Exportar a Excel", "exportComparison", "", "list"),
    ) +
    `<div class="compare-people">${players
      .map((p, i) => `<div><small>${i ? "Comparado" : "Referente"}</small>${personBtn(p)}</div>`)
      .join("")}</div>
    <div class="toolbar compare-actions">${select("Referente", "reference", idOptions(players), reference.id)}
    ${players.length < 5 ? select("Añadir jugador", "compareAdd", [[0, "Seleccionar…"], ...idOptions(people().filter((p) => !route.ids.includes(p.id) && (!cohort || cohort.ids.includes(p.id))))], 0) : ""}
    ${players.length > 2 ? select("Retirar jugador", "compareRemove", [[0, "Seleccionar…"], ...idOptions(players)], 0) : ""}
    ${canSimulate && !tactical ? btn("Simular refuerzo", "simulate:" + reference.id + ":" + challenger.id, "", "market") : ""}</div>` +
    tactical +
    (cohort
      ? ""
      : panel(
          "Evaluación de rol",
          `<div class="role-workspace">${evaluationControls(config, true)}<div>${roleQualityTable(players, config)}</div></div>`,
        )) +
    panel(
      cohort ? "Rendimiento y observaciones" : "Evidencia comparable",
      tbl(
        ["Indicador", ...players.map((p) => esc(p.name))],
        rows.map(([key, value]) => [key, ...players.map((p) => esc(cohort ? fmt(value(p)) : value(p)))]),
      ) +
        (reference.id === 10 && route.ids.includes(21)
          ? '<p class="note">Sol frente a Leo · Regate entre +1 y +3 puntos sobre 20.</p>'
          : ""),
    )
  );
}
actions.exportComparison = () => {
  const players = route.ids.map(person),
    config = evaluationConfig();
  const cohort = route.cohort;
  if (cohort) {
    exportExcel(
      "Comparación observada",
      ["Indicador", ...players.map((p) => p.name)],
      performanceIndicators(cohort).map(([name, value]) => [name, ...players.map(value)]),
      [
        ["Temporada y competición", statSeason(players[0])],
        ["N de la cohorte", cohort.ids.length],
      ],
    );
    return;
  }
  const authorized = projectionAllowed();
  const headers = ["Indicador", players[0].name];
  for (const p of players.slice(1)) headers.push(p.name, "Δ " + p.name + " (pp)");
  const rows = Object.entries(qualityColumns)
    .filter(([key]) => columnAllowed(key))
    .map(([key, { phase, kind }]) => {
      const ratings = players.map((p) => evaluateRole(p, config, phase)[kind]);
      const reference = ratings[0];
      return [
        columnLabel(key, config),
        percentCell(reference),
        ...ratings
          .slice(1)
          .flatMap((value) => [
            percentCell(value),
            ppCell(value == null || reference == null ? null : value - reference),
          ]),
      ];
    });
  exportExcel("Evaluación de rol", headers, rows, [
    ...evaluationMetadata(config, authorized),
    ["Diferencias", "Jugador comparado menos referente, expresadas en puntos porcentuales."],
  ]);
};
views.compare = compareView;
Object.assign(changes, {
  reference: (el) => {
    route.ids = [+el.value, ...route.ids.filter((id) => id !== +el.value)];
    render();
  },
  compareAdd: (el) => {
    if (+el.value) route.ids.push(+el.value);
    render();
  },
  compareRemove: (el) => {
    route.ids = route.ids.filter((id) => id !== +el.value);
    render();
  },
});
