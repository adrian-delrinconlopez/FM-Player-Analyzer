"use strict";
views.reg = () => {
  const s = state(),
    ids = (route.draft ??= [...s.registration]),
    has = ids.includes(8),
    uncertain = ids.includes(10);
  const status = has ? "Reserva cubierta" : uncertain ? "Reserva sin acreditar" : "Falta un formado en club";
  return (
    heading(
      "Inscripción y elegibilidad",
      "Copa Faro · Propuesta local · Una plaza reservada a formado en club",
    ) +
    `<div class="columns">${panel(
      "Propuesta de convocatoria",
      tbl(
        ["Incluir", "Jugador", "Formación en club"],
        own().map((p) => [
          `<input type="checkbox" data-change="register:${p.id}" aria-label="Inscribir a ${p.name}" ${ids.includes(p.id) ? "checked" : ""}>`,
          personBtn(p),
          p.id === 8 ? "Confirmada" : p.id === 10 ? unknown : "Sin acreditar",
        ]),
      ),
    )}
 ${panel(
   "Cupo de competición",
   pair("Plazas propuestas", ids.length + " / 3") +
     pair("Reserva", status) +
     pair("Propuesta aplicada", s.registration.map((id) => person(id).name).join(", ")) +
     pair("Ámbito", "Copa Faro · 2027/28") +
     `<div class="actions">${btn("Aplicar propuesta", "applyReg", "primary")}${btn("Descartar cambios", "undoReg")}</div>`,
 )}</div>`
  );
};
changes.register = (id, el) => {
  route.draft = el.checked ? [...route.draft, +id] : route.draft.filter((x) => x !== +id);
  render();
};
actions.applyReg = () => {
  if (route.draft.length > 3 || !route.draft.includes(8))
    return tell("Propuesta pendiente. Revisa el cupo y la reserva.");
  state().registration = [...route.draft];
  render();
  tell("Propuesta aplicada a Copa Faro");
};
actions.undoReg = () => {
  delete route.draft;
  render();
};
views.staff = () => {
  const balanced = route.profile === "Pesos iguales",
    a = balanced ? 1 : 2,
    b = 1;
  return (
    heading("Cuerpo técnico", "Valoración de personal · " + club()) +
    `<div class="columns">${panel("Vera Luar", pair("Identidad", "E1 · Preparadora") + pair("Club", club()) + pair("Categorías observadas", "Good · Elite") + select("Perfil de contraste", "staffProfile", ["Perfil de muestra", "Pesos iguales"], route.profile || "Perfil de muestra"))}
 ${panel(
   "Estimación ponderada",
   `<div class="rating"><strong>${fmt((13 * a + 20 * b) / (a + b))}</strong> /20</div>` +
     tbl(
       ["Categoría", "Equivalencia estimada", "Peso"],
       [
         ["Good", "12–14 (centro 13)", a],
         ["Elite", "20", b],
       ],
     ) +
     '<p class="note">Bandas del perfil de muestra. Good admite un intervalo de 12 a 14.</p>',
 )}</div>`
  );
};
changes.staffProfile = (el) => {
  route.profile = el.value;
  render();
};
function profileView() {
  const s = state(),
    d = s.draft;
  const output = (w) => (10 + 16 * w) / (1 + w);
  return (
    heading(
      "Perfiles de valoración",
      "Laboratorio didáctico · Escala 1–20 · Revisión activa " + s.profile.revision,
    ) +
    `<div class="columns">${panel(
      "Perfil activo",
      pair("Revisión", "r" + s.profile.revision) +
        pair("Atributos de ejemplo", "10 y 16") +
        pair("Pesos", "1 y " + s.profile.weight) +
        pair("Resultado", fmt(output(s.profile.weight)) + " /20") +
        btn("Duplicar para editar", "duplicateProfile", "", "profiles"),
    )}
 ${panel(
   "Borrador",
   d
     ? select(
         "Peso del segundo atributo",
         "draftWeight",
         [
           [1, "1"],
           [2, "2"],
           [3, "3"],
         ],
         d.weight,
       ) +
         pair("Resultado en vista previa", fmt(output(d.weight)) + " /20") +
         input("Curva creciente 50…69 · punto 10", "curvePoint", d.point, "number", 'min="0" max="100"') +
         pair("Vecinos del punto 10", "58 y 60") +
         pair(
           "Validación",
           d.point < 58 || d.point > 60 ? "Curva decreciente en el tramo" : "Secuencia válida",
         ) +
         `<div class="actions">${btn("Deshacer punto", "undoCurve")}${btn("Activar revisión", "activateProfile", "primary")}</div>`
     : empty("Duplica el perfil para ensayar pesos y curva."),
 )}</div>`
  );
}
views.profiles = profileView;
actions.duplicateProfile = () => {
  state().draft = { ...state().profile, point: state().profile.point ?? 59 };
  render();
};
changes.draftWeight = (el) => {
  state().draft.weight = +el.value;
  render();
};
changes.curvePoint = (el) => {
  if (!el.reportValidity()) return;
  state().draft.point = +el.value;
  render();
};
actions.undoCurve = () => {
  state().draft.point = state().profile.point ?? 59;
  render();
};
actions.activateProfile = () => {
  const s = state(),
    d = s.draft;
  if (d.point < 58 || d.point > 60) return tell("Curva rechazada. Revisa el punto 10.");
  s.profile = { weight: d.weight, point: d.point, revision: s.profile.revision + 1 };
  delete s.draft;
  render();
  tell("Revisión didáctica activada");
};
views.saves = () =>
  heading("Gestión de partidas", "Sincronización de demostración · Datos locales de la sesión") +
  `<div class="columns">${panel("Partida activa", select("Carrera", "career", ["Faro", "Bruma"], career) + pair("Club de la partida", club()) + pair("Mánager", career === "Faro" ? "Vega" : "Alba") + pair("Corte de juego", today()) + pair("Origen", "Datos de demostración · FM27"))}
 ${panel(
   "Simular sincronización",
   select(
     "Estado de fuente",
     "source",
     ["Disponible", "Cargando", "Fuente incompatible", "Sin datos", "Lista parcial"],
     state().source,
   ) +
     pair("Estado", state().source) +
     `<p class="note">${{ Cargando: "Esperando el lote simulado. El último corte sigue disponible.", "Fuente incompatible": "Se conserva el último corte autorizado.", "Sin datos": "No hay registros en la vista del corte.", "Lista parcial": "Lectura incompleta. Las ausencias no acreditan salidas.", Disponible: "El corte ficticio está disponible en esta carrera." }[state().source]}</p><div class="actions">${btn("Reintentar sincronización", "retry", "primary")}</div>`,
 )}</div>`;
changes.career = (el) => {
  exportRevision++;
  closeDialog();
  career = el.value;
  trail = [];
  route = { name: "saves" };
  $("#notice").textContent = "";
  render();
};
changes.source = (el) => {
  exportRevision++;
  state().source = el.value;
  render();
};
actions.retry = () => {
  state().source = "Disponible";
  render();
  tell("Corte de demostración disponible · " + today());
};
function settingsView() {
  const s = state(),
    perm = (k) =>
      `<div class="setting"><div><b>${k === "CA" ? "Capacidad actual" : "Capacidad potencial"} (${k})</b><small>${s.permissions[k] ? "Habilitada" : "Oculta"} · ${career === "Faro" ? "Datos ficticios para el conjunto de desarrollo" : "Sin fuente disponible en Bruma"}</small></div>${career !== "Faro" ? chip("Sin fuente") : btn(s.permissions[k] ? "Desactivar " + k : "Activar " + k, "permit:" + k, s.permissions[k] ? "" : "plain")}</div>`;
  return (
    heading("Configuración", "Preferencias de la carrera " + career) +
    panel(
      "Visibilidad y juego limpio",
      perm("CA") +
        perm("PA") +
        '<p class="note">Los intervalos y los atributos sin observar conservan el conocimiento registrado por el ojeador.</p>',
    ) +
    `<div class="columns">${panel(
      "Presentación",
      select(
        "Densidad de tablas",
        "density",
        [
          [0, "Cómoda"],
          [1, "Compacta"],
        ],
        +s.compact,
      ) +
        pair("Idioma", "Castellano") +
        pair("Recursos gráficos de ejemplo", s.graphics ? "Indexados" : "Iniciales") +
        btn("Elegir carpeta de ejemplo", "graphics") +
        (s.graphics ? btn("Reindexar", "reindex", "plain") : ""),
    )}
 ${panel("Estado de demostración", pair("Conservación", "Durante esta sesión") + pair("Carreras disponibles", "Faro · Bruma") + btn("Reiniciar demostración", "resetPrompt"))}</div>`
  );
}
views.settings = settingsView;
Object.assign(actions, {
  permit: (k) => {
    if (state().permissions[k]) {
      state().permissions[k] = false;
      exportRevision++;
      for (const filter of [...Object.values(state().filters), ...Object.values(state().saved)]) {
        delete filter[k];
        if (filter.sort === k || qualityColumns[filter.sort]?.kind === "future") {
          filter.sort = "id";
          filter.desc = false;
          filter.page = 0;
        }
      }
      state().selection = {};
      state().messages = state().messages.filter((m) => m.kind !== "permission");
      render();
      tell(k + " revocada");
    } else
      dialog(
        "Habilitar " + k + " ficticia",
        `<p>Football Manager normalmente oculta estos valores. Si habilitas ${k}, podrás verla en los datos de ejemplo con fuente disponible. El conocimiento del resto de atributos seguirá aplicándose.</p><div class="actions">${btn("Cancelar", "close")}${btn("Aceptar y habilitar " + k, "acceptPermit:" + k, "primary")}</div>`,
      );
  },
  acceptPermit: (k) => {
    state().permissions[k] = true;
    exportRevision++;
    closeDialog();
    render();
    tell("Permiso " + k + " habilitado en " + career);
  },
  graphics: () => {
    state().graphics = true;
    render();
    tell("Carpeta de ejemplo indexada · un retrato ficticio");
  },
  reindex: () => tell("Índice de ejemplo actualizado · un retrato ficticio"),
  resetPrompt: () =>
    dialog(
      "Reiniciar demostración",
      `<p>Se restaurarán ambas carreras, sus permisos, listas y escenarios de esta sesión.</p><div class="actions">${btn("Cancelar", "close")}${btn("Restaurar datos originales", "reset", "primary")}</div>`,
    ),
  reset: () => {
    exportRevision++;
    clearTablePreferences();
    sessions.Faro = newSession();
    sessions.Bruma = newSession();
    career = "Faro";
    route = { name: "home" };
    trail = [];
    closeDialog();
    render();
    tell("Datos originales restaurados");
  },
});
changes.density = (el) => {
  state().compact = el.value === "1";
  render();
};
const questions = [
  "¿Qué posición debería reforzar?",
  "¿Qué extremo encaja mejor?",
  "¿Qué jóvenes destacan?",
  "¿Cuál sería el mejor XI?",
  "¿Por qué este futbolista tiene mayor puntuación?",
  "¿Qué candidato sería mejor fichaje?",
];
function questionKind(q) {
  const n = normalize(q);
  if (/\b(ca|pa)\b|capacidad actual|capacidad potencial/.test(n)) return "permission";
  if (/extremo|banda/.test(n)) return "wing";
  if (/reforz|refuerz/.test(n)) return "position";
  if (/joven|cantera/.test(n)) return "youth";
  if (/mejor xi|alineacion/.test(n)) return "xi";
  if (/puntuacion|valoracion/.test(n)) return "fit";
  if (/fichaje|candidato/.test(n)) return "buy";
  return "unknown";
}
function reply(m) {
  if (career !== "Faro")
    return empty("Bruma dispone de una identidad y aún carece de evidencia para esta recomendación.");
  if (m.kind === "permission") {
    const values = special(person(21));
    return (
      (Object.keys(values).length
        ? Object.entries(values)
            .map(([k, v]) => k + " ficticia de Sol Arce: " + v)
            .join(" · ")
        : "Los valores solicitados están restringidos por la configuración de esta carrera.") +
      `<div class="actions">${btn("Revisar permisos", "go:settings", "link")}</div>`
    );
  }
  const s = state(),
    holder = person(lineup().ids[9]),
    sol = person(21),
    delta = score(sol) - score(holder);
  const responses = {
    position: [
      "La banda izquierda carece de relevo disponible. " +
        holder.name +
        " suma " +
        fmt(score(holder)) +
        " y Sol Arce " +
        fmt(score(sol)) +
        " sobre 100. La diferencia ilustrativa es " +
        sign(delta) +
        " puntos.",
      [
        ["Revisar AML", "openSlot:9"],
        [holder.name, "player:" + holder.id],
        ["Comparar con Sol", "aiCompare"],
      ],
    ],
    wing: [
      "Sol Arce tiene Regate 14–16 y Pase 13. Mar Olmo tiene Regate 12–14 y su Pase sigue sin observar.",
      [["Comparar extremos", "aiWings"]],
    ],
    youth: [
      "Ada Neral, 19 años, registra 73/100 con posesión en el perfil de muestra. Leo Nube mejoró dos puntos en Regate entre los cierres.",
      [
        ["Ada Neral", "player:8"],
        ["Evolución de Leo", "evolve:10"],
      ],
    ],
    xi: [
      "La pizarra " +
        s.formation +
        " contiene once titulares. Puedes calcular el mejor XI ilustrativo respetando las fijaciones activas.",
      [["Abrir pizarra", "go:tactics"]],
    ],
    fit: [
      "En AML, la media ilustrativa de las dos fases de Sol es (72 + 66) / 2 = 69. Leo tiene (60 + 56) / 2 = 58.",
      [["Ver evidencia", "aiCompare"]],
    ],
    buy: [
      "Sol presenta encaje ilustrativo para AML y valor de mercado de 4 M€. El precio de compra y el sueldo ofertado requieren una hipótesis.",
      [["Simular a Sol", "simulate:10:21"]],
    ],
    unknown: ["Consulta fuera de esta demostración.", []],
  };
  const [message, links] = responses[m.kind];
  return (
    "<p>" +
    esc(message) +
    '</p><div class="actions">' +
    links.map(([l, a]) => btn(l, a, "link", "arrow")).join("") +
    "</div>"
  );
}
views.ai = () =>
  heading(
    "Asistente ChatGPT",
    "Conversación simulada · Carrera " + career + " · " + today(),
    chip("Local · sin conexión", "green"),
  ) +
  panel(
    "Consultas de análisis",
    `<div class="suggestions">${questions.map((q, i) => btn(q, "ask:" + i)).join("")}</div>`,
  ) +
  `<section class="panel chat">${state()
    .messages.map(
      (m) =>
        `<article><small>Mánager · ${career}</small><b>${esc(m.q)}</b><div class="answer"><div class="eyebrow">${icon("ai", 15)} Asistente simulado</div>${reply(m)}</div></article>`,
    )
    .join("")}
 <form id="chatForm"><label>Consulta contextual<input id="chatInput" maxlength="300" placeholder="Pregunta por un jugador, un puesto o un fichaje…"></label><button class="primary">Consultar</button></form></section>`;
function ask(q) {
  q = q.trim();
  if (!q) return;
  state().messages.push({ q, kind: questionKind(q) });
  render();
  $("#chatInput")?.focus();
}
actions.ask = (i) => ask(questions[+i]);
actions.aiCompare = () => go("compare", { ids: [10, 21] });
actions.aiWings = () => go("compare", { ids: [21, 22] });
document.addEventListener("submit", (e) => {
  if (e.target.id === "chatForm") {
    e.preventDefault();
    ask($("#chatInput").value);
  }
});
