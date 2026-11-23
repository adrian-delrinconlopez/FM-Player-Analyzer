"use strict";
const paths = {
  home: "M3 3h7v7H3z M14 3h7v7h-7z M3 14h7v7H3z M14 14h7v7h-7z",
  squad:
    "M16 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2 M10 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8 M18 6a4 4 0 0 1 0 7 M20 16q3 1 3 5",
  search: "M16 16l5 5 M10.5 17a6.5 6.5 0 1 0 0-13 6.5 6.5 0 0 0 0 13",
  scout:
    "M3 16l3-11h3v10 M15 15V5h3l3 11 M8 10h8 M7 20a4 4 0 1 0 0-8 4 4 0 0 0 0 8 M17 20a4 4 0 1 0 0-8 4 4 0 0 0 0 8",
  tactics: "M3 3h18v18H3z M3 12h18 M8 3v4h8V3 M8 21v-4h8v4",
  stats: "M3 3v18h18 M7 16v-4 M12 16V9 M17 16V5",
  market: "M3 7h17l-4-4 M21 17H4l4 4",
  youth: "M3 18l6-7 4 3 8-10 M15 4h6v6",
  history: "M3 10a9 9 0 1 1 1 8 M3 4v6h6 M12 7v6l4 2",
  watch: "M6 3h12v18l-6-4-6 4z",
  staff: "M3 8h18v12H3z M8 8V4h8v4 M3 12q9 6 18 0",
  reg: "M5 3h14v18H5z M8 12l3 3 5-6",
  profiles: "M3 6h8m5 0h5 M3 17h4m5 0h9 M11 3v6h5V3z M7 14v6h5v-6z",
  saves: "M3 6h7l2 2h9v12H3z",
  settings:
    "M12 2l3 3 4 1 1 4 2 2-2 3-1 4-4 1-3 2-3-2-4-1-1-4-2-3 2-3 1-4 4-1z M12 15a3 3 0 1 0 0-6 3 3 0 0 0 0 6",
  ai: "M12 3l2.5 6.5L21 12l-6.5 2.5L12 21l-2.5-6.5L3 12l6.5-2.5z M20 2v4m-2-2h4",
  next: "M9 5l7 7-7 7",
  arrow: "M4 12h16m-6-6 6 6-6 6",
  back: "M20 12H4l6-6m-6 6 6 6",
  down: "M5 9l7 7 7-7",
  alert: "M12 3l10 18H2z M12 9v5m0 3v1",
  calendar: "M3 5h18v16H3z M3 10h18 M7 3v5m10-5v5",
  wallet: "M3 6l15-3v4 M3 7h18v14H3z M21 12h-6v5h6",
  ball: "M12 22a10 10 0 1 0 0-20 10 10 0 0 0 0 20 M12 7l5 4-2 6H9l-2-6z M12 7V2m5 9 5-2m-7 8 3 4m-9-4-3 4m1-10L2 9",
  shield: "M12 2l8 3v7q0 5-8 10-8-5-8-10V5z M8 12l3 3 5-6",
  menu: "M3 6h18M3 12h18M3 18h18",
  close: "M5 5l14 14M19 5L5 19",
  list: "M8 6h13M8 12h13M8 18h13 M3 6h1M3 12h1M3 18h1",
  db: "M3 6c0-5 18-5 18 0s-18 5-18 0v12c0 5 18 5 18 0V6 M3 12c0 5 18 5 18 0",
  clock: "M12 21a9 9 0 1 0 0-18 9 9 0 0 0 0 18 M12 6v6l4 2",
  compare: "M4 4h6v16H4z M14 4h6v16h-6z",
  pin: "M5 8l5 1 4-6 7 7-6 4 1 5-4-3-7 6 6-8z",
};
const icon = (name, size = 18) =>
  `<svg class="icon" width="${size}" height="${size}" viewBox="0 0 24 24" aria-hidden="true" fill="none" stroke="currentColor" stroke-width="1.65" stroke-linecap="round" stroke-linejoin="round"><path d="${paths[name] || paths.list}"/></svg>`;
const esc = (v) =>
  String(v ?? "").replace(
    /[&<>"']/g,
    (c) => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" })[c],
  );
const btn = (label, action, cls = "", ico = "") =>
  `<button type="button" class="${cls}" data-act="${esc(action)}">${ico ? icon(ico) : ""}${esc(label)}</button>`;
const chip = (label, tone = "") => `<span class="chip ${tone}">${esc(label)}</span>`;
const opt = (v, label, selected) =>
  `<option value="${esc(v)}" ${String(v) === String(selected) ? "selected" : ""}>${esc(label)}</option>`;
const select = (label, action, options, current) =>
  `<label>${esc(label)}<select data-change="${esc(action)}">${options.map((x) => (Array.isArray(x) ? opt(...x, current) : opt(x, x, current))).join("")}</select></label>`;
const input = (label, action, value, type = "text", limits = "") =>
  `<label>${esc(label)}<input type="${type}" value="${esc(value)}" data-change="${action}" ${limits}></label>`;
const panel = (title, body, tail = "", cls = "") =>
  `<section class="panel ${cls}"><div class="panel-head"><h2>${title}</h2>${tail}</div>${body}</section>`;
const heading = (title, subtitle = "", action = "") =>
  `<div class="heading"><div><h1>${esc(title)}</h1>${subtitle ? `<p>${esc(subtitle)}</p>` : ""}</div>${action}</div>`;
const pair = (k, v) => `<div class="pair"><span>${esc(k)}</span><strong>${esc(v)}</strong></div>`;
const portrait = (p) =>
  state().graphics && career === "Faro" && p.id === 1
    ? '<span class="avatar portrait-demo" role="img" aria-label="Retrato ficticio de Noa Mar"></span>'
    : `<span aria-hidden="true" class="avatar color${p.id % 4}">${p.name
        .split(" ")
        .slice(0, 2)
        .map((x) => x[0])
        .join("")}</span>`;
const personBtn = (p) =>
  `<button class="person" data-act="player:${p.id}">${portrait(p)}<span><b>${esc(p.name)}</b><small>${esc(p.club)} · ${p.pos.join(" / ")}</small></span></button>`;
const tabs = (items, current, action) =>
  `<div class="tabs">${items.map((x) => btn(x, action + ":" + x, x === current ? "active" : "")).join("")}</div>`;
const tbl = (heads, rows) =>
  `<div class="table-wrap"><table><thead><tr>${heads.map((x) => `<th scope="col">${x}</th>`).join("")}</tr></thead><tbody>${rows.map((r) => `<tr>${r.map((x) => `<td>${x}</td>`).join("")}</tr>`).join("")}</tbody></table></div>`;
const empty = (msg = "Ninguna persona cumple los filtros.") => `<p class="empty">${esc(msg)}</p>`;
const views = {},
  actions = {},
  changes = {};
const menuGroups = [
  [
    "Club",
    [
      ["home", "Resumen"],
      ["squad", "Plantilla"],
      ["tactics", "Táctica"],
      ["youth", "Desarrollo"],
      ["reg", "Inscripción"],
      ["staff", "Cuerpo técnico"],
    ],
  ],
  [
    "Mercado",
    [
      ["scout", "Ojeo"],
      ["db", "Base de jugadores"],
      ["watch", "Seguimiento"],
      ["market", "Operaciones"],
    ],
  ],
  [
    "Análisis",
    [
      ["stats", "Rendimiento"],
      ["history", "Históricos"],
      ["profiles", "Perfiles"],
    ],
  ],
  [
    "Sistema",
    [
      ["saves", "Partidas"],
      ["settings", "Configuración"],
    ],
  ],
];
const labels = Object.fromEntries(
  menuGroups
    .flatMap((g) => g[1])
    .concat([
      ["ai", "Asistente"],
      ["player", "Ficha individual"],
      ["compare", "Comparador"],
    ]),
);
let route = { name: "home" },
  trail = [],
  modalFocus = null,
  timer,
  menuOpen = false;
const $ = (s) => document.querySelector(s);
function bookmark() {
  return { action: document.activeElement?.dataset.act, scroll: window.scrollY };
}
function go(name, more = {}) {
  if (
    !views[name] ||
    (name === route.name &&
      !Object.keys(more).length &&
      !Object.keys(route).some((k) => k !== "name" && k !== "bookmark"))
  ) {
    menuOpen = false;
    closeDialog();
    render();
    return;
  }
  Object.assign(route, { bookmark: bookmark() });
  trail.push(route);
  route = { name, ...more };
  $("#notice").textContent = "";
  menuOpen = false;
  closeDialog();
  render();
  window.scrollTo(0, 0);
  $("#content").focus({ preventScroll: true });
}
function back() {
  if (!trail.length) return;
  route = trail.pop();
  $("#notice").textContent = "";
  render();
  window.scrollTo(0, route.bookmark?.scroll || 0);
  const focus = route.bookmark?.action;
  ((focus && document.querySelector('[data-act="' + CSS.escape(focus) + '"]')) || $("#content")).focus({
    preventScroll: true,
  });
}
function tell(msg) {
  $("#notice").textContent = msg;
  clearTimeout(timer);
  timer = setTimeout(() => ($("#notice").textContent = ""), 4200);
}
function dialog(title, body) {
  const modal = $("#modal");
  if (!modal.open) modalFocus = bookmark().action;
  $("#modalBody").innerHTML =
    `<div class="panel-head"><h2>${esc(title)}</h2>${btn("Cerrar", "close", "plain", "close")}</div>${body}`;
  if (!modal.open) modal.showModal();
}
function closeDialog() {
  if ($("#modal").open) $("#modal").close();
}
const crest = `<svg class="crest" viewBox="0 0 48 56" aria-hidden="true"><path fill="#efd5a7" d="M3 5l21-4 21 4v25c0 9-12 19-21 24C15 49 3 39 3 30z"/><path fill="#202933" d="M6 8l18-4 18 4v21c0 9-10 17-18 22C16 46 6 38 6 29z"/><path fill="#eeae63" d="M10 12h4v21l-4-5zm8 0h4v31l-4-5zm8 0h4v26l-4 5zm8 0h4v16l-4 5z"/><path fill="#fff1d0" d="M9 10l15 8 15-8-5 9-10 5-10-5z"/></svg>`;
function render() {
  const active = document.activeElement;
  const key = active?.dataset.change ? "change" : active?.dataset.act ? "act" : null;
  const focusKey = key ? active.dataset[key] : null;
  const s = state();
  document.body.classList.toggle("compact", s.compact);
  document.body.classList.toggle("menu-open", menuOpen);
  const destinations = menuGroups.flatMap(([, items]) => items.map(([id]) => id));
  const activeSection =
    destinations.includes(route.name) || route.name === "ai"
      ? route.name
      : trail
          .slice()
          .reverse()
          .find((r) => destinations.includes(r.name))?.name;
  const nav = (id, label) =>
    `<button data-act="go:${id}" class="nav ${activeSection === id ? "current" : ""}" title="${label}" ${activeSection === id ? 'aria-current="page"' : ""}>${icon(id)}<span>${label}</span></button>`;
  $("#rail").innerHTML =
    `<div class="brand"><span class="brandmark"><i></i><i></i><i></i></span><b>FM27 <em>Analyzer</em></b></div><nav>${menuGroups
      .map(
        ([name, items]) =>
          `<div class="navgroup"><small>${name}</small>${items
            .filter(([id]) => views[id])
            .map((x) => nav(...x))
            .join("")}</div>`,
      )
      .join(
        "",
      )}</nav><div class="manager"><span class="avatar">${career === "Faro" ? "MV" : "MA"}</span><span><b>Mánager ${career === "Faro" ? "Vega" : "Alba"}</b><small>Carrera ${career}</small></span></div>`;
  $("#top").innerHTML =
    `<div class="top-left">${btn("Menú", "menu", "mobile", "menu")}<span class="crumb">${route.name === "home" ? "Centro de análisis" : club() + " · " + today()}</span>${icon("next", 12)}<b>${labels[route.name]}</b></div><div class="tools">${btn("Buscar", "search", "search", "search")}${views.ai ? btn("Asistente", "go:ai", "assistant", "ai") : ""}<span class="demo"><i></i>Partida de demostración</span></div>`;
  const brumaEmpty =
    career === "Bruma" &&
    !["home", "db", "squad", "player", "watch", "saves", "settings", "ai"].includes(route.name);
  let content;
  if (s.source === "Sin datos" && !["saves", "settings"].includes(route.name))
    content =
      heading(labels[route.name]) +
      empty("Sin datos en el corte seleccionado.") +
      btn("Gestionar partida", "go:saves");
  else
    content = brumaEmpty
      ? heading(labels[route.name]) + empty("Bruma aún no tiene observaciones para esta tarea.")
      : views[route.name]();
  $("#content").innerHTML =
    (trail.length ? btn("Volver", "back", "back", "back") : "") +
    (s.source !== "Disponible"
      ? `<div class="banner">${s.source} · Último corte simulado, ${today()} ${btn("Gestionar", "go:saves", "link")}</div>`
      : "") +
    content;
  document.title = labels[route.name] + " · FM27 Analyzer";
  if (focusKey && !active.isConnected)
    document
      .querySelector("[data-" + key + '="' + CSS.escape(focusKey) + '"]')
      ?.focus({ preventScroll: true });
}
function dispatch(el, map, key) {
  const [action, ...args] = el.dataset[key].split(":");
  map[action]?.(...args, el);
}
document.addEventListener("click", (e) => {
  const b = e.target.closest("[data-act]");
  if (b) dispatch(b, actions, "act");
});
document.addEventListener("change", (e) => {
  if (e.target.dataset.change) dispatch(e.target, changes, "change");
});
document.addEventListener("keydown", (e) => {
  const marker = e.target.closest("[data-slot]");
  if (marker && ["ArrowLeft", "ArrowUp", "ArrowDown", "ArrowRight"].includes(e.key)) {
    e.preventDefault();
    const next = (+marker.dataset.slot + (["ArrowLeft", "ArrowUp"].includes(e.key) ? 10 : 1)) % 11;
    document.querySelector('[data-slot="' + next + '"]')?.focus();
    return;
  }
  if ((e.ctrlKey || e.metaKey) && e.key.toLowerCase() === "k") {
    e.preventDefault();
    actions.search();
  }
  if (e.key === "Escape" && !$("#modal").open) {
    if (menuOpen) {
      menuOpen = false;
      render();
    } else back();
  }
});
$("#modal").addEventListener("close", () => {
  $("#modalBody").replaceChildren();
  if (modalFocus)
    document.querySelector('[data-act="' + CSS.escape(modalFocus) + '"]')?.focus({ preventScroll: true });
  modalFocus = null;
});
Object.assign(actions, {
  go: (id) => go(id),
  back,
  close: closeDialog,
  menu: () => {
    menuOpen = !menuOpen;
    render();
  },
  player: (id) => {
    if (person(id)) go("player", { id: Number(id), tab: "Resumen" });
  },
  search: () => {
    dialog(
      "Buscar en " + career,
      '<label>Jugador o destino<input id="query" autocomplete="off" placeholder="Nombre, posición o pantalla"></label><div id="hits"></div>',
    );
    const update = () => {
      const q = $("#query").value.trim().toLocaleLowerCase("es");
      const destinations = Object.keys(labels)
        .filter(
          (k) =>
            views[k] && !["compare", "player", "train"].includes(k) && labels[k].toLowerCase().includes(q),
        )
        .slice(0, 5);
      $("#hits").innerHTML =
        destinations.map((k) => btn(labels[k], "go:" + k, "hit", k)).join("") +
          people()
            .filter(
              (p) =>
                state().source !== "Sin datos" && (p.name + " " + p.pos.join(" ")).toLowerCase().includes(q),
            )
            .slice(0, 12)
            .map(personBtn)
            .join("") || empty("Sin coincidencias.");
    };
    $("#query").oninput = update;
    update();
    $("#query").focus();
  },
});
