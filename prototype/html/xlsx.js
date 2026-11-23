"use strict";

const percentCell = (value) => (value == null ? null : { value: value / 100, format: "0.0%" });
const ppCell = (value) => (value == null ? null : { value, format: '+0.0" pp";-0.0" pp";0.0" pp"' });
let exportRevision = 0;

async function downloadWorkbook(sheetName, headers, rows, metadata) {
  const revision = exportRevision;
  const workbook = new ExcelJS.Workbook();
  workbook.creator = "FM27 Analyzer · Datos ficticios";
  const data = workbook.addWorksheet(sheetName, { views: [{ state: "frozen", ySplit: 1 }] });
  data.addRow(headers);
  data.autoFilter = {
    from: { row: 1, column: 1 },
    to: { row: Math.max(1, rows.length + 1), column: headers.length },
  };
  for (const values of rows) {
    const row = data.addRow(values.map((cell) => (cell && typeof cell === "object" ? cell.value : cell)));
    values.forEach((cell, i) => {
      if (cell?.format) row.getCell(i + 1).numFmt = cell.format;
    });
  }
  data.columns.forEach((column, index) => {
    const lengths = rows.map((row) => String(row[index]?.value ?? row[index] ?? "").length);
    column.width = Math.min(52, Math.max(14, headers[index].length + 3, ...lengths));
  });
  const context = workbook.addWorksheet("Contexto");
  context.columns = [{ width: 26 }, { width: 84 }];
  context.addRow(["Campo", "Valor"]);
  context.addRows([
    ["Carrera", career],
    ["Corte", today()],
    ["Fuente", "FM27 · Datos de demostración"],
    ...metadata,
  ]);
  context.getColumn(2).alignment = { wrapText: true, vertical: "top" };
  for (const sheet of [data, context]) {
    sheet.getRow(1).height = 24;
    sheet.getRow(1).eachCell((cell) => {
      cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FF243F4D" } };
      cell.font = { bold: true, color: { argb: "FFFFFFFF" } };
      cell.alignment = { vertical: "middle" };
    });
  }
  const buffer = await workbook.xlsx.writeBuffer();
  if (revision !== exportRevision) return false;
  const blob = new Blob([buffer], {
    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
  });
  const url = URL.createObjectURL(blob);
  const link = document.createElement("a");
  link.href = url;
  link.download = "FM27-" + sheetName.toLowerCase().replaceAll(" ", "-") + ".xlsx";
  document.body.append(link);
  link.click();
  link.remove();
  setTimeout(() => URL.revokeObjectURL(url), 1000);
  return true;
}

function evaluationMetadata(config, withFuture) {
  return [
    ["Posición evaluada", config.pos],
    ["Rol con posesión", config.IP],
    ["Rol sin posesión", config.OOP],
    ["Perfil de valoración", evaluationProfiles[config.profile]],
    ["Escala de calidad", "Porcentaje de 0 a 100. Valoración demostrativa."],
    ...(withFuture ? [["Proyección", "Escenario ficticio. Familiaridad hipotética 20/20."]] : []),
  ];
}
async function exportExcel(sheet, headers, rows, metadata) {
  try {
    if (await downloadWorkbook(sheet, headers, rows, metadata)) {
      closeDialog();
      tell("Archivo Excel generado · " + rows.length + " filas");
    }
  } catch (error) {
    console.error(error);
    tell("No se pudo generar el Excel. Inténtalo de nuevo.");
  }
}
