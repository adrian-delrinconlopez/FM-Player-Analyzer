"use strict";
// Valores del guion de Faro. Las notas de fase son ilustrativas y se conservan por UID.
const core = [
  ["Noa Mar",27,"GK",68,70],["Teo Linde",24,"DR",66,70],["Nil Roa",25,"DCR",65,72],
  ["Ivo Sael",26,"DCL",64,73],["Jon Vela",22,"DL",67,66],["Uri Paz",23,"DM MC",68,74],
  ["Pol Aire",22,"MC DM",70,69],["Ada Neral",19,"MC AMC",73,65],["Lía Sanzal",20,"AMR MR",72,64],
  ["Leo Nube",21,"AML ML",60,56],["Rui Bel",26,"ST",74,62],["Isa Luar",18,"GK",54,55],
  ["Dani Lago",20,"DCR DCL",64,69],["Kai Sena",17,"MC AMC",59,57],["Elia Rul",19,"DCL",62,65]
];
const unknown = "Sin observar";
const fmt = (n,digits=1) => n == null ? unknown : n.toLocaleString("es-ES",{maximumFractionDigits:digits,useGrouping:"always"});
const money = n => n == null ? unknown : fmt(n/1e6,2)+" M€";
const wage = p => p.salary == null ? unknown : fmt(p.salary)+" €/sem";
const score = p => p.ip == null ? null : (p.ip+p.oop)/2;
const sign = n => (n>0?"+":"")+fmt(n);
const keyAttr = key => "attr."+key;
const attr = (p,key) => p.attrs[keyAttr(key)] ?? null;
const attrText = v => Array.isArray(v) ? v.join("–") : v == null ? unknown : typeof v==="object" ? "≈ "+v.estimate : String(v);
const termName = key => ({"attr.dribbling":"Regate","attr.passing":"Pase"})[key] || terms.find(t=>t[0]===key)?.[1] || key;
function player(id,name,age,positions,extras={}) {
  return {id,name,age,pos:positions.split(" "),ip:null,oop:null,attrs:{},salary:null,value:null,end:null,foot:null,
    group:"Exterior",status:"Sin parte",club:"Club Salina",...extras};
}
const squad = core.map(([name,age,pos,ip,oop],i)=>{
  const id=i+1, attrs=Object.fromEntries(terms.map(([k],a)=>[k,8+(id+3*a)%10]));
  if(id===10) attrs["attr.dribbling"]=13;
  return player(id,name,age,pos,{ip,oop,attrs,club:id===15?"Club Brezo":"Valencia CF",foot:id%2?"Derecho":"Izquierdo",
    group:[12,14].includes(id)?"Cantera":id===15?"Cesión":"Primer equipo",status:id===13?"Lesionado":id===15?"Cedido":"Apto",
    value:id*100000,salary:1000+200*id,end:id===10?"2028-06-30":"2029-06-30"});
});
const market=[
  player(21,"Sol Arce",19,"AML ML",{ip:72,oop:66,value:4e6,salary:8000,foot:"Derecho",attrs:{"attr.dribbling":[14,16],"attr.passing":13}}),
  player(22,"Mar Olmo",20,"AML",{value:5e6,foot:"Izquierdo",attrs:{"attr.dribbling":[12,14]}}),
  player(23,"Jan Silo",26,"ST",{ip:70,oop:60,value:8e6,salary:15000,attrs:{"attr.dribbling":11,"attr.passing":12}})
];
const cabo=Array.from({length:25},(_,i)=>{
  const k=i+1,limited=k===25;
  return player(100+k,"Atleta del Cabo "+k,22,"ST",{club:"Club Junco",salary:1000+100*k,stats:{
    minutes:limited?180:900,apps:limited?2:12,goals:limited?1:k-1,shots:limited?null:3*k,
    assists:limited?null:k%5,completed:limited?null:100+2*k,attempted:limited?null:150+3*k,
    tackles:limited?null:24-k,fouls:limited?null:k%4}});
});
const names=["Álex","Irati","Darío","Inés","Gael","Toni","Alma","Bruno","Nico","Celia","Saúl","Mara"];
const surnames=["Dorna","Salce","Cendal","Riba","Nadal","Vento","Senda","Balsa"];
const reserves=Array.from({length:96},(_,i)=>{
  const pos=["GK","DR","DCR","MC","ST","DL","DM","AMR"][i%8];
  return player(200+i,names[i%12]+" "+surnames[Math.floor(i/12)],18+i%18,pos,{
    club:["Club Silvo","UD Dársena","Atlético Senda"][i%3],value:(2+i%20)*350000,salary:1500+i*110,
    foot:i%3?"Derecho":"Izquierdo",ip:i%7?52+i%28:null,oop:i%7?48+i%30:null,
    attrs:i%7?{"attr.dribbling":8+i%9,"attr.passing":{estimate:9+i%8}}:{}
  });
});
const allFaro=[...squad,...market,...cabo,...reserves];
const bruma=[player(1,"Luz Tena",24,"GK",{group:"Primer equipo",club:"Club Bruma",status:"Apto"})];
const cuts=["30 jun 2026","30 jun 2027","14 sep 2027"];
function observation(p,key,index) {
  if(!squad.includes(p)) return null;
  if(key==="score") return p.id===10 && index===0?54:score(p);
  if(key==="dribbling" && p.id===10 && index===0) return 11;
  return attr(p,key);
}
function newSession() {
  return {roster:{},watch:[],plans:[],filters:{},saved:{},lineups:{},formation:"4-3-3",phase:"IP",slot:9,
    permissions:{CA:false,PA:false},registration:[1,7,8],messages:[],compact:false,graphics:false,
    source:"Disponible",profile:{weight:2,revision:1},training:{},stats:{metric:"goals",min:450,pct:0,pop:"all",chart:"Dispersión",focus:112},selection:{}};
}
const sessions={Faro:newSession(),Bruma:newSession()};
let career="Faro";
const state=()=>sessions[career];
const people=()=>career==="Faro"?allFaro:bruma;
const own=()=>career==="Faro"?squad:bruma;
const person=id=>people().find(p=>p.id===Number(id));
const club=()=>career==="Faro"?"Valencia CF":"Club Bruma";
const today=()=>career==="Faro"?cuts[2]:"01 mar 2028";
function special(p) {
  if(career!=="Faro" || p.id!==21) return {};
  const allowed={};
  if(state().permissions.CA) allowed.CA=122;
  if(state().permissions.PA) allowed.PA=151;
  return allowed;
}
function family(pos) {
  if(pos==="GK") return "Portería";
  if(["DL","DR","DCL","DCR"].includes(pos)) return "Defensa";
  return ["MC","DM","AMC"].includes(pos)?"Medio":"Ataque";
}
const roleBase={GK:"GK",DR:"FB",DL:"FB",DCR:"CB",DCL:"CB",MC:"CM",DM:"DM",AMC:"AM",ST:"CFD",AML:"W",AMR:"W",ML:"WM",MR:"WM"};
const role=(pos,phase)=> (phase==="OOP" && ["AML","AMR"].includes(pos)?"WM":roleBase[pos])+"-"+phase;
function lineup(form=state().formation) {
  if(!state().lineups[form]) state().lineups[form]={ids:own().slice(0,11).map(p=>p.id),locks:[]};
  return state().lineups[form];
}
function positions(form) {
  return ["GK","DR","DCR","DCL","DL","DM",form==="4-3-3"?"MC":"DM",form==="4-3-3"?"MC":"AMC","AMR","AML","ST"];
}

