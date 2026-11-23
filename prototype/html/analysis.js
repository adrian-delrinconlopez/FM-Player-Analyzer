"use strict";
const metricsDef={goals:["Goles /90","Goles"],shots:["Remates /90","Remates"],assists:["Asistencias /90","Asistencias"],
 completed:["Acierto de pase (%)","Pases acertados"],tackles:["Entradas ganadas /90","Entradas ganadas"],fouls:["Faltas /90","Faltas"]};
function statValue(p,key){const s=p?.stats;if(!s||s[key]==null)return null;return key==="completed"?100*s.completed/s.attempted:90*s[key]/s.minutes;}
function percentile(p,pool,key){
 const v=statValue(p,key),known=pool.map(q=>statValue(q,key)).filter(x=>x!=null);
 if(v==null||!pool.includes(p)||known.length<20)return null;
 const lower=known.filter(x=>key==="fouls"?x>v:x<v).length,equal=known.filter(x=>Math.abs(x-v)<1e-9).length;
 return 100*(lower+equal/2)/known.length;
}
function statDetail(p){
 const st=p.stats;
 return panel("Registro de temporada",pair("Competición y temporada","Liga del Cabo · 2026/27")+pair("Minutos / apariciones",st.minutes+" / "+st.apps)+
 tbl(["Métrica","Total","Tasa"],Object.entries(metricsDef).map(([k,[label]])=>[label,fmt(st[k]),fmt(statValue(p,k))]))+
 (st.minutes<450?'<p class="context">Muestra provisional · por debajo de 450 minutos.</p>':""));
}
function statPool(){
 const s=state().stats,scope=s.pop==="small"?cabo.slice(0,12):s.pop==="own"?own():cabo;
 return {scope,pool:scope.filter(p=>p.stats?.minutes>=s.min&&statValue(p,s.metric)!=null)};
}
function scatter(pool,p,metric){
 if(!pool.length)return empty("Sin puntos elegibles con este mínimo.");
 const xmax=Math.max(1,...pool.map(x=>statValue(x,metric))),ymax=Math.max(...pool.map(q=>q.salary));
 const X=v=>51+400*v/xmax,Y=v=>171-142*v/ymax;
 return `<svg class="chart" viewBox="0 0 490 215" role="img" aria-label="Dispersión de ${metricsDef[metric][0]} frente a sueldo semanal. Valores en la tabla.">
 <g class="gridlines">${[0,.5,1].map(f=>`<path d="M51 ${Y(f*ymax)}H451"/><text x="44" y="${Y(f*ymax)+4}" text-anchor="end">${fmt(f*ymax)}</text><text x="${X(f*xmax)}" y="190" text-anchor="middle">${fmt(f*xmax)}</text>`).join("")}</g>
 <text x="15" y="16">€/semana</text><text x="250" y="211" text-anchor="middle">${metricsDef[metric][0]}</text>
 ${pool.map(q=>`<circle data-act="statFocus:${q.id}" cx="${X(statValue(q,metric))}" cy="${Y(q.salary)}" r="${q===p?6:4}" fill="${q===p?"#f7c572":"#69d5c0"}"><title>${q.name} · ${fmt(statValue(q,metric))} · ${wage(q)}</title></circle>`).join("")}</svg>`;
}
function radar(p,pool){
 const keys=Object.keys(metricsDef),center=[180,102],point=(i,r)=>{const a=i*Math.PI/3-Math.PI/2;return [center[0]+r*Math.cos(a),center[1]+r*Math.sin(a)].join(",");};
 const vals=keys.map(k=>percentile(p,pool,k));
 if(vals.some(x=>x==null))return empty("Radar sin percentiles válidos. Consulta los valores observados.");
 return `<svg class="chart" viewBox="0 0 360 210" role="img" aria-label="Radar de percentiles, escala 0 a 100"><g fill="none" stroke="#536577">${[24,48,72].map(r=>`<polygon points="${keys.map((_,i)=>point(i,r)).join(" ")}"/>`).join("")}</g><polygon fill="#69d5c03d" stroke="#69d5c0" points="${vals.map((v,i)=>point(i,v*.72)).join(" ")}"/>${keys.map((k,i)=>{const [x,y]=point(i,90).split(",");return `<text x="${x}" y="${y}" text-anchor="middle">${metricsDef[k][1]}</text>`;}).join("")}</svg>`+
 tbl(["Métrica","Percentil"],keys.map((k,i)=>[metricsDef[k][0],fmt(vals[i])]));
}
function statsView(){
 const s=state().stats,{scope,pool}=statPool(),p=person(s.focus),metric=s.metric,chosen=s.chosen||[];
 const qualifying=s.pct>0?pool.filter(q=>percentile(q,pool,metric)>=s.pct):scope.filter(q=>q.stats && (pool.includes(q)||q.id===125 && s.min<=900));
 const goodFocus=qualifying.includes(p)?p:qualifying[0];if(goodFocus)s.focus=goodFocus.id;
 const controls=`<div class="toolbar">${select("Población","stat:pop",[["all","Cabo · delanteros"],["small","Cabo · ordinales 1–12"],["own",club()]],s.pop)}
 ${select("Métrica","stat:metric",Object.entries(metricsDef).map(([k,[n]])=>[k,n]),metric)}
 ${input("Mínimo de minutos","stat:min",s.min,"number",'min="0" max="10000"')}
 ${input("Percentil mínimo","stat:pct",s.pct,"number",'min="0" max="100"')}${select("Gráfico","stat:chart",["Dispersión","Radar"],s.chart)}</div>`;
 return heading("Rendimiento y Moneyball","Liga del Cabo · Temporada 2026/27 · Datos ficticios")+
 (trail.at(-1)?.name==="scout"?'<p class="context">Criterios de atributos y encaje suspendidos. Volver recupera la búsqueda de ojeo.</p>':"")+controls+
 `<div class="columns">${panel(s.chart==="Radar"?"Perfil relativo":"Rendimiento y coste",s.chart==="Radar"&&goodFocus?radar(goodFocus,pool):scatter(pool,goodFocus,metric),chip("N = "+pool.length,"green"))}
 ${panel("Referencia estadística",pair("Población",s.pop==="own"?club():"Delanteros · Liga del Cabo")+pair("Mínimo de minutos",s.min)+pair("Cohorte con métrica",pool.length)+pair("Percentiles",pool.length<20?"N insuficiente (<20)":"Rango medio · escala 0–100")+
 (goodFocus?personBtn(goodFocus)+pair(metricsDef[metric][0],fmt(statValue(goodFocus,metric)))+pair("Percentil",fmt(percentile(goodFocus,pool,metric)))+pair("Minutos",goodFocus.stats.minutes):""))}</div>`+
 panel("Resultados · "+qualifying.length,qualifying.length?tbl(["Selección","Jugador","Minutos",metricsDef[metric][0],"Percentil","Sueldo","Gráfico"],qualifying.map(q=>[
 `<input type="checkbox" aria-label="Comparar ${q.name}" data-change="statPick:${q.id}" ${chosen.includes(q.id)?"checked":""} ${!pool.includes(q)?"disabled":""}>`,personBtn(q),q.stats.minutes,fmt(statValue(q,metric)),pool.includes(q)?fmt(percentile(q,pool,metric)):chip("Provisional","gold"),wage(q),btn("Inspeccionar","statFocus:"+q.id,"link")])):empty("Sin resultados para la población y el umbral elegidos."),
 chosen.length>=2?btn("Comparar selección","statCompare","primary","compare"):"");
}
views.stats=statsView;
Object.assign(changes,{stat:(k,el)=>{if(!el.reportValidity())return;const s=state().stats;s[k]=["pct","min"].includes(k)?+el.value:el.value;
 if(k!=="chart")s.chosen=[];render();},statPick:(id,el)=>{const s=state().stats,ids=s.chosen||[];if(el.checked&&ids.length>=5){el.checked=false;return tell("Máximo cinco jugadores.");}
 s.chosen=el.checked?[...ids,+id]:ids.filter(x=>x!==+id);render();}});
Object.assign(actions,{statFocus:id=>{state().stats.focus=+id;render();},statCompare:()=>go("compare",{ids:[...state().stats.chosen],cohort:{metric:state().stats.metric,ids:statPool().pool.map(p=>p.id)}})});
function historyBody(p) {
 if(!squad.includes(p))return empty("Sin observación en esta fecha.");
 const key=route.attribute||"dribbling",idx=route.interval==="Último cierre → presente"?1:0,end=idx+1;
 const val=i=>observation(p,key,i),a=val(idx),b=val(end),max=key==="score"?100:20;
 const cutClub=i=>p.id===15&&i===0?"Valencia CF":p.club;
 const y=v=>122-95*v/max;
 const chart=`<svg class="chart historychart" viewBox="0 0 420 158" role="img" aria-label="${cuts[idx]}: ${a}. ${cuts[end]}: ${b}. Escala de 0 a ${max}">
 <g class="gridlines">${[0,max/2,max].map(n=>`<path d="M40 ${y(n)}H395"/><text x="5" y="${y(n)+4}">${n}</text>`).join("")}</g>
 <path d="M76 ${y(a)} L348 ${y(b)}" stroke="#69d5c0" fill="none" stroke-width="3"/>
 ${[[76,idx],[348,end]].map(([x,i])=>`<circle cx="${x}" cy="${y(val(i))}" r="5" fill="#69d5c0"/><text x="${x}" y="${y(val(i))-12}" text-anchor="middle">${fmt(val(i))}</text><text x="${x}" y="148" text-anchor="middle">${cuts[i]}</text>`).join("")}</svg>`;
 return `<div class="toolbar">${select("Indicador","historyField:attribute",[["dribbling","Regate"],["score","Valoración de ejemplo"],...terms.filter(([id])=>id!=="attr.dribbling").map(([id])=>[id.slice(5),termName(id)])],key)}${select("Intervalo","historyField:interval",["Entre cierres","Último cierre → presente"],route.interval||"Entre cierres")}</div>
 <div class="columns">${panel("Evolución de "+p.name,chart,chip(sign(b-a)+" /"+max,"green"))}
 ${panel("Observaciones de demostración",tbl(["Corte","Club","Valor /"+max], [idx,end].map(i=>[cuts[i],cutClub(i),fmt(val(i))]))+pair("Revisión","Datos r1 · Perfil Faro r1")+pair("Cambio entre cortes",sign(b-a))+
 btn("Entrenamiento individual","train:"+p.id,"link","arrow"))}</div>`;
}
views.history=()=>{const p=person(route.id||10);return heading("Históricos y temporadas","Dos cierres y un corte actual · Carrera "+career)+select("Persona con observaciones","historyId",idOptions(own()),p.id)+historyBody(p);};
changes.historyId=el=>{route.id=+el.value;render();};
changes.historyField=(k,el)=>{route[k]=el.value;render();};
function trainingView(){
 const p=person(route.id),k=route.focusAttr||"dribbling",v=attr(p,k),saved=state().training[p.id];
 return heading("Entrenamiento individual",p.name+" · "+p.pos.join(" / ")+" · Recomendación simulada")+
 `<div class="columns">${panel("Objetivo individual",personBtn(p)+select("Atributo objetivo","focusAttr",terms.map(([id])=>[id.slice(5),termName(id)]),k)+pair("Observación actual",attrText(v)+" /20")+
 pair("Plan guardado",saved?termName(keyAttr(saved)):"Sin asignar")+btn("Guardar objetivo","saveTraining","primary"))}
 ${panel("Ejemplo pedagógico",pair("Curva lineal · un punto","100/19 = 5,26")+pair("Factor de banda","2/3")+pair("Prioridad ilustrativa","200/57 = 3,51")+
 '<p class="note">Ejercicio de ponderación. La recomendación de muestra permite seguir el atributo elegido en el siguiente corte.</p>')}</div>`;
}
views.train=trainingView;labels.train="Entrenamiento";
actions.train=id=>go("train",{id:+id});
changes.focusAttr=el=>{route.focusAttr=el.value;render();};
actions.saveTraining=()=>{state().training[route.id]=route.focusAttr||"dribbling";render();tell("Objetivo individual guardado");};

