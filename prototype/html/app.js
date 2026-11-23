"use strict";
const pitchLines = `<svg class="markings" viewBox="0 0 600 380" preserveAspectRatio="none" aria-hidden="true"><g fill="none" stroke="currentColor" stroke-width="1.4"><rect x="14" y="12" width="572" height="356" rx="2"/><path d="M14 190h572 M187 12v55h226V12 M245 12v25h110V12 M187 368v-55h226v55 M245 368v-25h110v25"/><ellipse cx="300" cy="190" rx="47" ry="32"/><path d="M259 67q41 32 82 0 M259 313q41-32 82 0"/><circle cx="300" cy="190" r="2"/><path d="M14 23q11 0 11-11 M575 12q0 11 11 11 M14 357q11 0 11 11 M575 368q0-11 11-11"/></g></svg>`;
function coords(form, phase) {
  const second = form === "4-2-3-1",
    oop = phase === "OOP";
  return [
    [50, 87],
    [84, 68],
    [62, 73],
    [38, 73],
    [16, 68],
    [second ? 61 : 50, 54],
    [second ? 39 : 67, second ? 54 : 40],
    [second ? 50 : 33, second ? (oop ? 40 : 34) : 40],
    [83, oop ? (second ? 54 : 40) : 22],
    [17, oop ? (second ? 54 : 40) : 22],
    [50, 14],
  ];
}
function pitch(form, phase, action, selected = -1) {
  const loc = coords(form, phase),
    ids = lineup(form).ids,
    pos = positions(form);
  return `<div class="pitch">${pitchLines}${ids
    .map((id, i) => {
      const p = person(id),
        [x, y] = loc[i];
      return `<button class="marker ${selected === i ? "chosen" : ""} ${i === 9 ? "wing" : ""}" style="left:${x}%;top:${y}%" data-act="${action}:${i}" data-slot="${i}" draggable="${action === "slot"}" title="${p.name} · ${pos[i]} · ${role(pos[i], phase)}" aria-label="${p.name}, ${pos[i]}, ${role(pos[i], phase)}, ${lineup(form).locks.includes(i) ? "fijado" : "libre"}"><b>${id}</b><span>${esc(p.name)}${action === "slot" ? `<small>${role(pos[i], phase)}${lineup(form).locks.includes(i) ? " · Fijado" : ""}</small>` : ""}</span></button>`;
    })
    .join("")}</div>`;
}
function metric(title, value, detail, ico, tone = "") {
  return `<div class="metric"><div>${icon(ico, 16)}<span>${title}</span></div><strong class="${tone}">${value}</strong><small>${detail}</small></div>`;
}
const attention = (ico, tone, title, reason, action) =>
  `<button class="attention" data-act="${action}"><span class="iconbox ${tone}">${icon(ico)}</span><span><b>${title}</b><small>${reason}</small></span>${icon("next", 14)}</button>`;
views.home = () => {
  const s = state(),
    team = own(),
    fit = team.filter((p) => p.status === "Apto").length;
  const hero = `<section class="hero"><div class="hero-data">${crest}<div><div class="hero-season">Temporada ${career === "Faro" ? "2027 / 28" : "2028"} · Carrera ${career}</div><h1>${club()}</h1><p>${icon("calendar", 15)} ${today()} <span>·</span> Informe del club</p></div></div><div class="hero-foot">${chip("Datos ficticios", "glass")}</div></section>`;
  if (career === "Bruma") return hero + panel("Plantilla vinculada", personBtn(team[0]));
  const aml = person(lineup().ids[9]);
  return (
    hero +
    `<div class="metrics">${metric("Plantilla vinculada", team.length, "Primer equipo, cantera y cesión", "squad")}${metric("Disponibles", fit + "<em> / " + team.length + "</em>", "1 lesionado · 1 cedida", "shield", "mint")}${metric("Presupuesto", "6,0 <em>M€</em>", "Para escenarios de compra", "wallet")}${metric("Sistema activo", s.formation, "Dos fases · 50 % con posesión", "tactics")}</div>
 <div class="dashboard-grid">${panel("Once de referencia", pitch(s.formation, s.phase, "openSlot") + `<div class="panel-foot"><span><i class="dot"></i> ${s.phase === "IP" ? "Con posesión" : "Sin posesión"}</span>${btn("Abrir pizarra", "go:tactics", "link", "arrow")}</div>`, chip(s.formation), "fieldpanel")}
 ${panel("Prioridades deportivas", attention("alert", "gold", "Revisar la banda izquierda", aml.name + " · " + fmt(score(aml)) + "/100 · Sin relevo", "openSlot:9") + attention("calendar", "blue", "Contrato en último año", "Leo Nube · vence el 30 jun 2028", "contract:10") + attention("shield", "rose", "Baja en la defensa", "Dani Lago · hasta el 30 sep 2027", "player:13") + `<div class="insight"><div class="eyebrow">${icon("youth", 16)} Evolución destacada</div><div class="split"><div><b>Leo Nube</b><small>Regate · cierres 2026 / 2027</small></div><strong class="mint">11 → 13</strong></div>${btn("Ver evolución", "evolve:10", "link", "arrow")}</div>`, chip("3 avisos", "gold"))}</div>
 <div class="dashboard-grid">${panel(
   "Distribución por línea",
   ["Portería", "Defensa", "Medio", "Ataque"]
     .map((k) => {
       const n = team.filter((p) => family(p.pos[0]) === k).length;
       return `<div class="barrow"><span>${k}</span><div><i style="width:${(n / team.length) * 100}%"></i></div><b>${n}</b></div>`;
     })
     .join(""),
 )}
 ${panel("Jugadores observados", market.map((p) => `<div class="split row">${personBtn(p)}${chip(money(p.value))}</div>`).join(""), btn("Ojeo", "go:scout", "link", "arrow"))}</div>`
  );
};
actions.openSlot = (i) => {
  state().slot = +i;
  go("tactics");
};
actions.contract = (id) => go("player", { id: +id, tab: "Contrato" });
actions.evolve = (id) => go("player", { id: +id, tab: "Evolución" });
document.addEventListener("DOMContentLoaded", render);
