"use strict";

// Datos de ensayo exclusivos de Faro. Las familias responden de forma distinta al desarrollo simulado.
const developmentCases = {
  8: { CA: 106, PA: 149, gains: { Medio: 13, Ataque: 9, Defensa: 3, Portería: 0 } },
  10: { CA: 140, PA: 142, gains: { Medio: 1, Ataque: 2, Defensa: 0, Portería: 0 } },
  21: { CA: 122, PA: 151, gains: { Medio: 7, Ataque: 12, Defensa: 2, Portería: 0 } },
  23: { CA: 128, PA: 143, gains: { Medio: 4, Ataque: 6, Defensa: 1, Portería: 0 } },
};
const rolePositions = {
  ST: ["ST (C)", 50, 11],
  AML: ["AM (L)", 17, 27],
  AMC: ["AM (C)", 50, 27],
  AMR: ["AM (R)", 83, 27],
  ML: ["M (L)", 17, 44],
  MC: ["M (C)", 50, 44],
  MR: ["M (R)", 83, 44],
  WBL: ["WB (L)", 17, 61],
  DM: ["DM", 50, 61],
  WBR: ["WB (R)", 83, 61],
  DL: ["D (L)", 12, 77],
  DCL: ["D (C)", 37, 77],
  DCR: ["D (C)", 63, 77],
  DR: ["D (R)", 88, 77],
  GK: ["GK", 50, 92],
};
const phases = { IP: "Con posesión", OOP: "Sin posesión" };
const qualityColumns = {
  currentIP: { phase: "IP", kind: "current", label: "Actual · con posesión" },
  futureIP: { phase: "IP", kind: "future", label: "Proyección · con posesión" },
  currentOOP: { phase: "OOP", kind: "current", label: "Actual · sin posesión" },
  futureOOP: { phase: "OOP", kind: "future", label: "Proyección · sin posesión" },
};
const evaluationProfiles = { faro: "Faro · r1", technique: "Técnica · r1 (demo)" };
const projectionAllowed = () => career === "Faro" && state().permissions.CA && state().permissions.PA;

function rolesAt(pos, phase) {
  return roleCatalog.filter((row) => row[2] === phase && row[3].includes(rolePositions[pos][0]));
}
function defaultEvaluation(pos = "AML") {
  const defaultRole = (phase) => {
    const compatible = rolesAt(pos, phase);
    return (
      compatible.find(([id]) => id === role(pos, phase))?.[0] ??
      compatible.find(([id]) => id === "W-OOP")?.[0] ??
      compatible[0][0]
    );
  };
  return { pos, IP: defaultRole("IP"), OOP: defaultRole("OOP"), profile: "faro" };
}
function positionalFamiliarity(p, pos) {
  if (p.ip == null) return null;
  if (p.pos.includes(pos)) return 20;
  return p.pos.some((key) => family(key) === family(pos)) ? 11 : 4;
}
function evaluateRole(p, config, phase) {
  const familiarity = positionalFamiliarity(p, config.pos);
  if (familiarity == null) return { current: null, future: null, familiarity, reason: "Datos insuficientes" };

  // Las cifras predefinidas por fase se mantienen como referencia del guion. Los ajustes
  // de rol, perfil y familiaridad son utilería determinista, ajena al motor nativo.
  const roles = rolesAt(config.pos, phase);
  const standard = defaultEvaluation(config.pos)[phase];
  const variant = config[phase] === standard ? 0 : roles.findIndex(([id]) => id === config[phase]) + 1;
  const passing = attr(p, "passing");
  const profileShift = config.profile === "technique" && typeof passing === "number" ? passing - 10 : 0;
  const roleShift = variant ? ((p.id * (variant + 1) + variant * 5) % 13) - 6 : 0;
  const crossLine = p.pos.some((pos) => family(pos) === family(config.pos)) ? 0 : -12;
  const base = (phase === "IP" ? p.ip : p.oop) + profileShift + roleShift + crossLine;
  const bounded = (value) => Math.max(0, Math.min(100, value));
  const current = bounded(base - (20 - familiarity));
  if (!projectionAllowed()) return { current, future: null, familiarity, reason: "Requiere CA y PA" };

  const scenario = developmentCases[p.id];
  if (!scenario) return { current, future: null, familiarity, reason: "Sin escenario de desarrollo" };
  const gain = scenario.gains[family(config.pos)];
  const future = bounded(base + Math.max(0, gain - (phase === "OOP" && gain >= 4 ? 2 : 0)));
  return { current, future, familiarity, reason: null };
}
const qualityText = (v) => (v == null ? "No disponible" : fmt(v) + " %");
const phaseDifference = (value, baseline) =>
  value == null || baseline == null ? "—" : sign(value - baseline) + " pp";
function evaluationConfig() {
  const container = listNames[route.name] ? filters() : route;
  return (container.evaluation ??= defaultEvaluation(
    route.context?.pos ||
      (route.name === "player"
        ? person(route.id).pos[0]
        : route.name === "compare"
          ? person(route.ids[0]).pos[0]
          : "AML"),
  ));
}
function evaluationControls(config, graphic = false) {
  const roleOptions = (phase) => rolesAt(config.pos, phase).map(([id, label]) => [id, id + " · " + label]);
  const positionControl = graphic
    ? `<div class="role-pitch" role="group" aria-label="Seleccionar posición">${pitchLines}${Object.entries(
        rolePositions,
      )
        .map(
          ([pos, [label, x, y]]) =>
            `<button type="button" data-act="evalPosition:${pos}" aria-pressed="${config.pos === pos}" aria-label="${pos} · ${label}" style="left:${x}%;top:${y}%">${pos}</button>`,
        )
        .join("")}</div>`
    : select("Posición evaluada", "evaluation:pos", Object.keys(rolePositions), config.pos);
  const fields = `<div class="role-fields">${graphic ? `<strong>Puesto ${config.pos}</strong>` : ""}
    ${select("Rol con posesión", "evaluation:IP", roleOptions("IP"), config.IP)}
    ${select("Rol sin posesión", "evaluation:OOP", roleOptions("OOP"), config.OOP)}
    ${select("Perfil de valoración", "evaluation:profile", Object.entries(evaluationProfiles), config.profile)}</div>`;
  return graphic
    ? fields + positionControl
    : `<div class="evaluation-controls">${positionControl}${fields}</div>`;
}
function setEvaluation(key, value) {
  const config = evaluationConfig();
  if (key === "pos") Object.assign(config, defaultEvaluation(value), { profile: config.profile });
  else config[key] = value;
  if (listNames[route.name]) filters().page = 0;
  render();
}
changes.evaluation = (key, el) => setEvaluation(key, el.value);
actions.evalPosition = (pos) => setEvaluation("pos", pos);
actions.projection = () =>
  dialog(
    "Supuestos de la proyección",
    `
 <p>El escenario lleva la familiaridad de la posición evaluada a <strong>20/20</strong>. La calidad actual usa la familiaridad presente. Alcanzar esa familiaridad depende del desarrollo del jugador.</p>
 <p>Los resultados utilizan casos ficticios con distinto margen de desarrollo y encaje por fase. La fórmula de proyección para FM27 está pendiente de validación.</p>
 <p>Se necesitan ambos permisos, CA y PA, y un escenario disponible para el futbolista.</p>
 ${btn("Configurar CA y PA", "go:settings", "link", "settings")}`,
  );
