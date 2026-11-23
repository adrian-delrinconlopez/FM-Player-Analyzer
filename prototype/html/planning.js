"use strict";
const viable = (p, pos) => p.status === "Apto" && p.pos.includes(pos);
const alternateForm = () => (state().formation === "4-3-3" ? "4-2-3-1" : "4-3-3");
const formationTotal = (form) => lineup(form).ids.reduce((n, id) => n + score(person(id)), 0);
function slotName(pos, phase) {
  return phase === "OOP" ? { AML: "ML", AMR: "MR" }[pos] || pos : pos;
}
function depth(form) {
  const l = lineup(form);
  return positions(form).map((pos, i) => ({
    i,
    pos,
    id: l.ids[i],
    alternatives: own().filter((p) => !l.ids.includes(p.id) && viable(p, pos)),
  }));
}
function tactics() {
  const s = state(),
    form = s.formation,
    l = lineup(),
    i = s.slot,
    p = person(l.ids[i]),
    pos = positions(form)[i],
    ph = s.phase,
    tab = route.tab || "Pizarra",
    bench = own().filter((p) => !l.ids.includes(p.id));
  const controls = `<div class="toolbar">${select("Formación activa", "formation", ["4-3-3", "4-2-3-1"], form)}${select(
    "Fase táctica",
    "phase",
    [
      ["IP", "Con posesión"],
      ["OOP", "Sin posesión"],
    ],
    ph,
  )}${btn("Mejor XI simulado", "bestXI", "primary", "tactics")}</div>`;
  const inspector = panel(
    "Puesto " + slotName(pos, ph),
    personBtn(p) +
      pair("Rol de fase", role(pos, ph)) +
      pair("Valoración de ejemplo", fmt(score(p)) + " /100") +
      pair("Con / sin posesión", p.ip + " / " + p.oop) +
      pair(
        "Relevo disponible",
        depth(form)
          [i].alternatives.map((q) => q.name)
          .join(", ") || "Vacante",
      ) +
      pair("Fijación", l.locks.includes(i) ? "Fijado" : "Libre") +
      btn("Simular baja", "absence", "", "shield") +
      `<div class="actions">${btn(l.locks.includes(i) ? "Quitar fijación" : "Fijar titular", "lock", "", "pin")}${btn("Buscar refuerzo", "reinforce", "primary", "scout")}</div>` +
      select(
        "Asignar o intercambiar",
        "assign",
        [
          [0, "Elegir jugador…"],
          ...own()
            .filter((q) => q.id !== p.id)
            .map((q) => [q.id, q.name + " · " + q.pos[0] + " · " + q.status]),
        ],
        0,
      ) +
      `<p class="note">El selector y el arrastre aplican las mismas condiciones de disponibilidad, puesto y fijación.</p>`,
  );
  let body =
    tab === "Pizarra"
      ? `<div class="columns tactics">${panel("Distribución del once", pitch(form, ph, "slot", i) + `<div class="panel-foot"><span>↑ Ataque · ${ph === "IP" ? "Con posesión" : "Sin posesión"}</span><span>11 titulares</span></div>`, chip(form))}${inspector}</div>` +
        panel(
          "Banquillo y ausencias",
          `<div class="bench">${bench.map((q) => `<div draggable="true" data-drag-id="${q.id}">${personBtn(q)}${chip(q.status, lightStatus(q))}</div>`).join("")}</div>`,
        )
      : tab === "Profundidad"
        ? panel(
            "Cobertura por puesto",
            tbl(
              ["Puesto", "Titular", "Relevo fuera del XI", ""],
              depth(form).map((d) => [
                d.pos,
                personBtn(person(d.id)),
                d.alternatives.map((q) => q.name).join(", ") || chip("Vacante", "gold"),
                btn("Revisar", "inspectSlot:" + d.i, "link"),
              ]),
            ),
          )
        : panel(
            "Dos configuraciones de la misma plantilla",
            tbl(
              ["Indicador", "4-3-3", "4-2-3-1"],
              [
                [
                  "Suma del XI · puntos de ejemplo",
                  ...["4-3-3", "4-2-3-1"].map((f) => fmt(formationTotal(f))),
                ],
                [
                  "Puestos con relevo",
                  ...["4-3-3", "4-2-3-1"].map(
                    (f) => depth(f).filter((d) => d.alternatives.length).length + " / 11",
                  ),
                ],
                ...positions(form).map((_, idx) => [
                  "Puesto " + (idx + 1),
                  ...["4-3-3", "4-2-3-1"].map(
                    (f) => positions(f)[idx] + " · " + person(lineup(f).ids[idx]).name,
                  ),
                ]),
              ],
            ),
          );
  return (
    heading("Planificador táctico", club() + " · Dos fases · Perfil Faro r1 · IP 50 %") +
    controls +
    tabs(["Pizarra", "Profundidad", "Comparar tácticas"], tab, "ttab") +
    body
  );
}
views.tactics = tactics;
function assign(id, i) {
  const s = state(),
    l = lineup(),
    pos = positions(s.formation),
    p = person(id),
    old = l.ids.indexOf(id);
  let reason = "";
  if (!p || p.status !== "Apto")
    reason =
      p?.status === "Lesionado"
        ? "Lesionado hasta el 30 sep 2027."
        : "Jugador fuera de la plantilla disponible.";
  else if (l.locks.includes(i) || l.locks.includes(old))
    reason = "Quita la fijación antes de cambiar el titular.";
  else if (!viable(p, pos[i]) || (old >= 0 && !viable(person(l.ids[i]), pos[old])))
    reason = "Las posiciones admitidas impiden esta asignación.";
  if (reason) {
    tell(reason);
    render();
    return;
  }
  if (old >= 0) l.ids[old] = l.ids[i];
  l.ids[i] = id;
  s.slot = i;
  render();
  tell("Asignación actualizada");
}
function setForm(form) {
  const previous = lineup(),
    next = lineup(form);
  for (const i of previous.locks) {
    const prior = next.ids.indexOf(previous.ids[i]);
    if (prior >= 0 && prior !== i) next.ids[prior] = next.ids[i];
    next.ids[i] = previous.ids[i];
  }
  next.locks = [...new Set([...next.locks, ...previous.locks])];
  state().formation = form;
  closeDialog();
  render();
}
Object.assign(changes, {
  phase: (el) => {
    state().phase = el.value;
    render();
  },
  formation: (el) => {
    const form = el.value,
      l = lineup(),
      incompatible = l.locks.filter((i) => positions(form)[i] !== positions(state().formation)[i]);
    if (incompatible.length) {
      render();
      dialog(
        "Revisar fijación",
        `<p>El puesto de ${incompatible.map((i) => person(l.ids[i]).name).join(", ")} cambia con la formación. Para continuar, libera esa fijación.</p><div class="actions">${btn("Cancelar", "close")}${btn("Liberar y cambiar", "unlockForm:" + form, "primary")}</div>`,
      );
    } else {
      setForm(form);
    }
  },
  assign: (el) => {
    if (+el.value) assign(+el.value, state().slot);
  },
});
Object.assign(actions, {
  slot: (i) => {
    state().slot = +i;
    render();
    document.querySelector('[data-slot="' + i + '"]')?.focus({ preventScroll: true });
  },
  absence: () => {
    const s = state(),
      d = depth(s.formation)[s.slot],
      p = person(d.id),
      alt = d.alternatives[0];
    dialog(
      "Baja hipotética · " + p.name,
      pair("Puesto", d.pos) +
        pair("Relevo", alt ? alt.name : "Vacante · sin alternativa disponible") +
        pair(
          "Diferencia ilustrativa",
          alt ? sign(score(alt) - score(p)) + " puntos" : "Sin nota comparable",
        ) +
        (alt ? btn("Asignar relevo", "useRelief:" + alt.id, "primary") : ""),
    );
  },
  useRelief: (id) => {
    closeDialog();
    assign(+id, state().slot);
  },
  inspectSlot: (i) => {
    state().slot = +i;
    route.tab = "Pizarra";
    render();
  },
  ttab: (tab) => {
    route.tab = tab;
    render();
  },
  lock: () => {
    const l = lineup(),
      i = state().slot;
    l.locks = l.locks.includes(i) ? l.locks.filter((x) => x !== i) : [...l.locks, i];
    render();
  },
  unlockForm: (form) => {
    const l = lineup();
    l.locks = l.locks.filter((i) => positions(form)[i] === positions(state().formation)[i]);
    setForm(form);
  },
  reinforce: () => {
    const s = state(),
      i = s.slot;
    go("scout", { context: { id: lineup().ids[i], pos: positions(s.formation)[i], form: s.formation, i } });
  },
  bestXI: () => {
    const l = lineup(),
      pos = positions(state().formation),
      used = new Set(l.locks.map((i) => l.ids[i])),
      out = [...l.ids];
    // Conservar cobertura completa antes de maximizar la suma, con los fijados inmóviles.
    let best = -Infinity,
      bestIds = null;
    function visit(i, total) {
      if (i === pos.length) {
        if (total > best) {
          best = total;
          bestIds = [...out];
        }
        return;
      }
      if (l.locks.includes(i)) {
        visit(i + 1, total + score(person(out[i])));
        return;
      }
      for (const p of own().filter((q) => viable(q, pos[i]) && !used.has(q.id))) {
        out[i] = p.id;
        used.add(p.id);
        visit(i + 1, total + score(p));
        used.delete(p.id);
      }
    }
    visit(0, 0);
    if (bestIds) l.ids = bestIds;
    render();
    tell(
      bestIds
        ? "XI simulado · " + fmt(best) + " puntos entre once titulares"
        : "Las fijaciones impiden completar el XI.",
    );
  },
});
document.addEventListener("dragstart", (e) => {
  const el = e.target.closest("[data-drag-id],[data-slot]");
  if (route.name === "tactics" && el)
    e.dataTransfer.setData("text/plain", el.dataset.dragId || lineup().ids[+el.dataset.slot]);
});
document.addEventListener("dragover", (e) => {
  if (route.name === "tactics" && e.target.closest("[data-slot]")) e.preventDefault();
});
document.addEventListener("drop", (e) => {
  const slot = e.target.closest("[data-slot]");
  if (route.name === "tactics" && slot) {
    e.preventDefault();
    assign(+e.dataTransfer.getData("text/plain"), +slot.dataset.slot);
  }
});
function newDeal(id, ref = null) {
  const p = person(id),
    type = own().includes(p) ? "Venta" : "Compra";
  if (type === "Venta" && state().source === "Lista parcial")
    return tell("Lista parcial. Reintenta la sincronización antes de proponer salidas.");
  go("market", { deal: { id, ref, type, price: "", currency: "EUR", offer: "" }, context: route.context });
}
function marketView() {
  const s = state(),
    d = route.deal,
    head = heading(
      "Operaciones de mercado",
      "Planificación de compras y ventas · Presupuesto de ejemplo 6 M€",
    );
  if (!d)
    return (
      head +
      panel(
        "Nueva hipótesis",
        `<div class="toolbar">${select("Posible compra", "chooseDeal", [[0, "Elegir candidato"], ...idOptions(market)], 0)}${select("Posible venta", "chooseDeal", [[0, "Elegir jugador"], ...idOptions(own())], 0)}</div>`,
      ) +
      panel(
        "Escenarios guardados",
        s.plans.length
          ? tbl(
              ["Operación", "Jugador", "Importe hipotético", ""],
              s.plans.map((d, i) => [
                d.type,
                personBtn(person(d.id)),
                d.price !== "" ? fmt(+d.price) + " M " + d.currency : "Indeterminado",
                btn("Revisar", "loadDeal:" + i, "link"),
              ]),
            )
          : empty("Todavía no hay escenarios guardados."),
      ) +
      (route.context ? btn("Volver al puesto", "returnSlot", "", "back") : "")
    );
  const p = person(d.id),
    vacancy = depth(s.formation).find((slot) => slot.id === p.id),
    ref = person(d.ref),
    saldo =
      d.currency === "EUR" && d.price !== "" ? 6 + (d.type === "Venta" ? 1 : -1) * Number(d.price) : null;
  return (
    head +
    `<div class="columns">${panel(
      d.type + " hipotética",
      personBtn(p) +
        pair("Valor de mercado de ejemplo", money(p.value)) +
        pair("Precio observado", unknown) +
        `<div class="toolbar">${input("Importe hipotético (millones)", "dealField:price", d.price, "number", 'min="0" step="0.01"')}${select("Moneda", "dealField:currency", ["EUR", "GBP"], d.currency)}</div>` +
        btn("Adoptar valor de mercado", "adoptPrice") +
        input("Sueldo ofertado (€/semana)", "dealField:offer", d.offer, "number", 'min="0"') +
        pair("Saldo antes de gastos", saldo === null ? "Indeterminado" : fmt(saldo) + " M€") +
        pair("Sueldo ofertado", d.offer === "" ? "Sin determinar" : fmt(+d.offer) + " €/sem"),
    )}
 ${panel("Efecto deportivo", ref ? personBtn(ref) + pair("Titular → candidato", ref.name + " → " + p.name) + pair("Contribución al puesto", fmt(score(ref)) + " → " + fmt(score(p))) + pair("Variación del XI", score(ref) != null && score(p) != null ? sign(score(p) - score(ref)) + " puntos" : "Sin nota comparable") : pair("Plantilla vinculada", own().length + " jugadores") + pair("Escenario", d.type === "Venta" ? "Salida hipotética" : "Entrada hipotética") + (d.type === "Venta" ? pair("Puesto afectado", vacancy?.pos || "Fuera del XI") + pair("Relevo disponible", vacancy ? vacancy.alternatives.map((q) => q.name).join(", ") || "Vacante" : "Titulares conservados") : ""))}</div>
 <div class="actions">${btn("Guardar escenario", "saveDeal", "primary")}${btn("Descartar", "discardDeal")}${route.context ? btn("Volver al puesto", "returnSlot", "", "back") : ""}</div>`
  );
}
views.market = marketView;
Object.assign(changes, {
  chooseDeal: (el) => {
    if (+el.value) newDeal(+el.value);
  },
  dealField: (k, el) => {
    if (!el.reportValidity()) return;
    route.deal[k] = el.value;
    render();
  },
});
Object.assign(actions, {
  simulate: (ref, id) => newDeal(+id, +ref),
  deal: (id) => newDeal(+id),
  adoptPrice: () => {
    route.deal.price = String(person(route.deal.id).value / 1e6);
    route.deal.currency = "EUR";
    render();
  },
  saveDeal: () => {
    const d = structuredClone(route.deal);
    if (route.index != null) state().plans[route.index] = d;
    else state().plans.push(d);
    delete route.deal;
    delete route.index;
    render();
    tell("Escenario guardado en " + career);
  },
  discardDeal: () => {
    delete route.deal;
    delete route.index;
    render();
  },
  loadDeal: (i) => {
    route.deal = structuredClone(state().plans[+i]);
    route.index = +i;
    render();
  },
  returnSlot: () => {
    const i = route.context.i;
    while (trail.length && trail.at(-1).name !== "tactics") trail.pop();
    if (trail.length) {
      state().slot = i;
      back();
    } else go("tactics");
  },
});
