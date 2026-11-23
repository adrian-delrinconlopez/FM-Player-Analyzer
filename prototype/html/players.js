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
    columns:
      route.name === "squad"
        ? ["age", "fit", "end", "salary"]
        : ["age", "fit", "dribbling", "value", "salary"],
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
  const val = (p) =>
    f.sort === "fit"
      ? score(p)
      : f.sort === "dribbling"
        ? Array.isArray(attr(p, "dribbling"))
          ? attr(p, "dribbling")[0]
          : attr(p, "dribbling")
        : p[f.sort];
  return rows.sort((a, b) => {
    const x = val(a),
      y = val(b);
    if (x == null || y == null) return Number(x == null) - Number(y == null);
    return (typeof x === "string" ? x.localeCompare(y, "es") : x - y) * (f.desc ? -1 : 1) || a.id - b.id;
  });
}
const columnNames = {
  age: "Edad",
  fit: "Encaje /100",
  dribbling: "Regate /20",
  value: "Valor de mercado",
  salary: "Sueldo semanal",
  end: "Vencimiento",
  pass: "Pase /20",
};
function cell(p, col) {
  return (
    {
      age: p.age + " años",
      fit: score(p) == null ? "Sin nota" : fmt(score(p)),
      dribbling: attrText(attr(p, "dribbling")),
      pass: attrText(attr(p, "passing")),
      value: money(p.value),
      salary: wage(p),
      end: p.end?.split("-").reverse().join("/") || unknown,
      ...special(p),
    }[col] ?? unknown
  );
}
function authorizedColumns() {
  return [...filters().columns, ...Object.keys(state().permissions).filter((k) => state().permissions[k])];
}
const idOptions = (pool) => pool.map((p) => [p.id, p.name + " · " + p.pos[0]]);
function listView() {
  const f = filters(),
    rows = listRows(),
    picked = selected(),
    cols = authorizedColumns(),
    n = 16;
  f.page = Math.min(f.page, Math.max(0, Math.ceil(rows.length / n) - 1));
  const scouting = route.name === "scout";
  const context = route.context,
    columns = cols.map((c) => columnNames[c] || c);
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
      ...cols.map((c) => (c === "fit" && score(p) != null ? chip(cell(p, c), "green") : esc(cell(p, c)))),
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
    `<section class="panel player-table">${listTools}${controls}${advanced}<div class="split table-tools"><span><b>${rows.length}</b> jugadores · ${picked.length} seleccionados</span><div class="actions">${picked.length >= 2 ? btn("Comparar selección", "compareSelected", "primary", "compare") : ""}${picked.length ? btn("Seguir selección", "followSelection") : ""}${btn("Exportar", "export", "plain", "list")}</div></div>
 ${rows.length ? tbl(["", sortable("Jugador", "name"), "Estado", ...cols.map((c, i) => (["age", "fit", "value", "salary", "end", "dribbling"].includes(c) ? sortable(columns[i], c) : columns[i])), "Acción"], data) : empty(route.name === "watch" ? "La lista está vacía. Selecciona Seguir en una ficha o en Ojeo." : undefined)}
 <div class="panel-foot"><span>Valoración de ejemplo · Perfil Faro r1 · 50 % IP</span><div class="actions">${f.page ? btn("Anterior", "page:-1") : ""}<span>Página ${f.page + 1} / ${Math.max(1, Math.ceil(rows.length / n))}</span>${(f.page + 1) * n < rows.length ? btn("Siguiente", "page:1") : ""}</div></div></section>`
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
    let f = filters();
    f.columns = el.checked ? [...f.columns, key] : f.columns.filter((k) => k !== key);
    render();
  },
  exportFormat: (el) => exportPreview(el.value),
});
function exportPreview(format = "Excel (.xlsx)") {
  const ids = selected(),
    rows = listRows().filter((p) => !ids.length || ids.includes(p.id)),
    cols = authorizedColumns();
  dialog(
    "Vista previa de exportación",
    select("Formato simulado", "exportFormat", ["Excel (.xlsx)", "CSV"], format) +
      `<p class="context">${rows.length} filas · Orden y columnas de la vista · Datos ficticios</p>` +
      tbl(
        ["Jugador", "Estado", ...cols.map((c) => columnNames[c] || c)],
        rows.map((p) => [p.name, rowStatus(p), ...cols.map((c) => esc(cell(p, c)))]),
      ),
  );
}
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
  columns: () =>
    dialog(
      "Columnas visibles",
      Object.entries(columnNames)
        .map(
          ([k, v]) =>
            `<label class="check"><input type="checkbox" data-change="column:${k}" ${filters().columns.includes(k) ? "checked" : ""}>${v}</label>`,
        )
        .join(""),
    ),
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
  compareSelected: () => go("compare", { ids: [...selected()] }),
  contextPair: (id) => go("compare", { ids: [route.context.id, +id], context: route.context }),
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
  if (+el.value) go("compare", { ids: [route.reference, +el.value] });
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
  const phase = route.role?.endsWith("OOP") ? "OOP" : "IP",
    also = p.pos.flatMap((pos) => ["IP", "OOP"].map((ph) => role(pos, ph)));
  const rating = phase === "IP" ? p.ip : p.oop;
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

  return `<div class="columns">${panel(
    "Encaje por rol",
    select("Rol analizado", "profileRole", [...new Set(also)], route.role || also[0]) +
      `<div class="rating"><strong>${fmt(rating)}</strong> /100</div>${phaseBars(p)}` +
      pair("Valoración de ejemplo", phase === "IP" ? "Con posesión" : "Sin posesión") +
      pair("Perfil de muestra", "Faro · r1") +
      pair("Atributos con evidencia", known + " de 47") +
      pair("Rol de fase", "Denominación pendiente"),
  )}
 ${panel(
   "Lectura del jugador",
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
changes.profileRole = (el) => {
  route.role = el.value;
  render();
};
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
function phaseBars(p) {
  return (
    '<div class="phase-bars">' +
    [
      ["IP", p.ip],
      ["OOP", p.oop],
    ]
      .map(
        ([phase, v]) =>
          `<div><span>${phase}</span><div class="scale">${v == null ? "" : `<i style="width:${v}%"></i>`}</div><b>${fmt(v)}</b></div>`,
      )
      .join("") +
    "</div>"
  );
}
function compareView() {
  const ps = route.ids.map(person),
    ref = ps[0],
    rows = [
      ["Posiciones", (p) => p.pos.join(" / ")],
      ["Encaje /100", (p) => fmt(score(p))],
      ["Con posesión /100", (p) => fmt(p.ip)],
      ["Sin posesión /100", (p) => fmt(p.oop)],
      ["Regate /20", (p) => attrText(attr(p, "dribbling"))],
      ["Pase /20", (p) => attrText(attr(p, "passing"))],
      ["Edad", (p) => p.age],
      ["Valor de mercado de ejemplo", (p) => money(p.value)],
      ["Sueldo vigente", wage],
      ...Object.keys(state().permissions)
        .filter((k) => state().permissions[k])
        .map((k) => [k + " ficticia", (p) => special(p)[k] ?? unknown]),
    ];
  if (route.cohort)
    rows.unshift(
      [metricsDef[route.cohort.metric][0], (p) => fmt(statValue(p, route.cohort.metric))],
      [
        "Percentil · N=" + route.cohort.ids.length,
        (p) => fmt(percentile(p, route.cohort.ids.map(person), route.cohort.metric)),
      ],
    );
  const cohort = route.cohort;
  function cardValues(p, i) {
    if (cohort) {
      const rank = percentile(p, cohort.ids.map(person), cohort.metric);
      return `<div class="rating"><strong>${fmt(statValue(p, cohort.metric))}</strong></div><small>${metricsDef[cohort.metric][0]} · ${statSeason(p)}</small>${pair("Minutos", p.stats.minutes)}${pair("Percentil", rank == null ? "N insuficiente" : fmt(rank))}`;
    }
    const value = score(p),
      reference = score(ref);
    return `<div class="rating"><strong>${value == null ? "Sin nota" : fmt(value)}</strong>${value == null ? "" : " /100"}</div><small>Valoración de ejemplo</small>${phaseBars(p)}${i && reference != null && value != null ? chip(sign(value - reference) + " puntos", value >= reference ? "green" : "gold") : ""}`;
  }
  const layout = ps
    .map(
      (p, i) =>
        `<div class="card compare-card"><div class="eyebrow">${i === 0 ? "Referente" : "Comparado " + i}</div>${personBtn(p)}${cardValues(p, i)}</div>`,
    )
    .join("");
  return (
    heading(
      "Comparador de jugadores",
      route.cohort
        ? statSeason(ref) + " · Población fija de " + route.cohort.ids.length
        : "Perfil Faro r1 · 50 % con posesión · " + today(),
    ) +
    `<div class="comparison">${layout}</div><div class="toolbar">${select("Referente", "reference", idOptions(ps), ref.id)}${ps.length < 5 ? select("Añadir jugador", "compareAdd", [[0, "Seleccionar…"], ...idOptions(people().filter((p) => !route.ids.includes(p.id) && (!route.cohort || route.cohort.ids.includes(p.id))))], 0) : ""}${ps.length > 2 ? select("Retirar jugador", "compareRemove", [[0, "Seleccionar…"], ...idOptions(ps)], 0) : ""}
 ${market.includes(ps[1]) && own().includes(ref) && ps[1].pos.some((pos) => ref.pos.includes(pos)) ? btn("Simular refuerzo", "simulate:" + ref.id + ":" + ps[1].id, "primary", "market") : ""}</div>` +
    panel(
      "Evidencia comparable",
      tbl(
        ["Indicador", ...ps.map((p) => p.name)],
        rows.map(([k, fn]) => [k, ...ps.map(fn)]),
      ) +
        (ref.id === 10 && route.ids.includes(21)
          ? '<p class="note">Sol frente a Leo · Regate entre +1 y +3 puntos sobre 20.</p>'
          : ""),
    )
  );
}
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
