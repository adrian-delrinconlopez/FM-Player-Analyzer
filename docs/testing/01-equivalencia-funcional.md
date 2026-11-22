# Equivalencia funcional de FM27

Usar las filas FM27 del oráculo Classic: 79 roles × 16 muestras × 2 configuraciones = **2.528 pares absoluta/normalizada** de los 4.608 publicados para ambas ediciones. Exigir normalizada entera idéntica y |error de absoluta| ≤ 10⁻¹² × máx(1, |referencia|). El evaluador privado de pruebas incluye las muestras ausentes; el servicio deportivo aplica la política de cobertura nueva. En curvas son **237 normalizadas** (79 × 3), error máximo 0,05 por precisión publicada. Probar además cobertura por debajo, exactamente en y por encima de 70 %, porteros por fase y rango 9–11 en curva escalonada. [Classic](../as-is/20-oraculo-dwrs-classic.md) · [Curvas](../as-is/22-oraculo-de-curvas-fm27.md)

Reconstruir entradas según las 47 claves en orden canónico de cálculo y catálogo de roles FM27. Comparar contra cada celda publicada, fijando fase, cuota, orden y redondeo. Registrar primera discrepancia con factores. Para G03, G11 y demás fixtures exclusivos de FM24, recrear la propiedad con puestos y entradas válidos de FM27 y etiquetar **derivado**. El nuevo caso todavía carece del sello G. [Casos y alcance originales](../as-is/14-casos-de-prueba-de-referencia.md)

En XI comprobar tupla objetivo, puestos, UID por fase, A/B disjuntos, restricciones, desempates y límite de refinamiento. Repetir con orden conservado y permutado. Si falta evidencia para el UID vencedor en un empate, comparar factibilidad y objetivo y registrar las asignaciones para revisión. Comparar números internos antes de probar en otro caso el texto presentado.

## Pruebas por capacidad

G/M califica el escenario **de origen** del [anexo 24](../as-is/24-casos-de-aceptacion-por-capacidad.md). El nuevo resultado aún espera ejecución. Los parámetros específicos se toman del contrato enlazado por ID en la [matriz](../to-be/18-trazabilidad-as-is-to-be.md).

| Caso | Requisito | Origen | Tratamiento | Resultado exigido en destino |
|---|---|---|---|---|
| EQ-001 | REQ-DATA-001 | M | AMPLIADA | UID 42, ritmo 12–14 y altura 170 cm permanecen rango y medida tras reabrir. |
| EQ-002 | REQ-DATA-002 | M | MODIFICADA | 437 y r-437 quedan vinculados sólo tras evidencia o aceptación; dos Luis homónimos conservan sus series. |
| EQ-003 | REQ-DATA-003 | G | MODIFICADA | €1,5K en columna EUR semanal da 1.500; texto ilegible y club ausente no implican coste cero ni libertad. |
| EQ-004 | REQ-ROLE-001 | M | CONSERVADA | Dos posiciones con rol común generan una sola asignación compatible. |
| EQ-005 | REQ-ROLE-002 | M | MODIFICADA | Key 14–16 produce fortaleza de media 15; desconocido queda sin clasificar. |
| EQ-006 | REQ-RATING-001 | G | MODIFICADA | Caso documentado A=92 y N=55; al faltar Aceleración el oráculo da 32 y la aplicación muestra cobertura insuficiente. |
| EQ-007 | REQ-RATING-002 | G | CONSERVADA | Uniforme12 da 70,1 en AM-IP y 70,3 en AM-OOP. |
| EQ-008 | REQ-RATING-003 | G | CONSERVADA | BGK da 57,9 / 54,0 / 46,0 en las tres fichas del anexo 22. |
| EQ-009 | REQ-TAC-001 | G | CONSERVADA | Dos puestos, P=80/79 y Q=78/ausente dan Q y P, suma 157. |
| EQ-010 | REQ-TAC-002 | G | CONSERVADA | G04 cambia de Gnabry a Olise al pasar de 50 a 70 % IP. |
| EQ-011 | REQ-TAC-003 | M | CONSERVADA | Baja del titular 80, única alternativa legal 71, pérdida 9. |
| EQ-012 | REQ-SQUAD-001 | M | CONSERVADA | Único sobrante válido en tres roles cubre el tercero por reserva aun con límite inicial 2. |
| EQ-013 | REQ-SQUAD-002 | G | AMPLIADA | G05 señala MC desplazado, mejor destino DMC, gravedad 30. El acceso contextual se verifica en NV-25. |
| EQ-014 | REQ-SQUAD-003 | G | AMPLIADA | G05 da 11 titulares, media 71,5, mediana 71 y siete puestos frágiles. |
| EQ-015 | REQ-NAT-001 | G | POSPUESTA | Al retomar, diez plazas, objetivo tres porteros y sólo dos disponibles producen dos y ocho de campo. |
| EQ-016 | REQ-SCOUT-001 | M | MODIFICADA | Ritmo 12–14 con mínimo 13 es posible; con pase desconocido no satisface ambos mínimos seguros. |
| EQ-017 | REQ-SCOUT-002 | M | CONSERVADA | Mejor propio 74, candidatos 74 y 76, única propuesta 76 con ventaja 2. |
| EQ-018 | REQ-MARKET-001 | G | AMPLIADA | G06 eleva A de 228 a 236 y B en 70; la plantilla base conserva sus miembros. |
| EQ-019 | REQ-MARKET-002 | M | CONSERVADA | Tres excedentes del caso 019 de 16, 17 y 21 años producen sin sugerencia, cesión y venta. |
| EQ-020 | REQ-DEV-001 | G | MODIFICADA | G07 da 87,5; sin personalidad conocida el desglose fija su término en 0, sin deducirla. |
| EQ-021 | REQ-DEV-002 | M | MODIFICADA | Atributo lineal a 10, edad 22, factor 2/3: prioridad 200/57. |
| EQ-022 | REQ-DEV-003 | M | CONSERVADA | Quickness obtiene 3 con prioridades 2 y 1; se excluye rehabilitación. |
| EQ-023 | REQ-HIST-001 | M | MODIFICADA | Enero 10, febrero 12 y vuelta a 10 conservan el cierre mensual igual a enero; carga anterior queda en revisión. |
| EQ-024 | REQ-DATA-004 | G | MODIFICADA | CSV y HTML de la misma vista FM27 producen los mismos UID y rangos; encabezados ambiguos quedan pendientes. |
| EQ-025 | REQ-FM-001 | G | SUSTITUIDA | Un intervalo leído en pantalla llega igual al backend; un archivo .fm se rechaza como fuente. |
| EQ-026 | REQ-FM-002 | M | SUSTITUIDA | Reenviar un lote confirmado deja idéntico número de filas y observaciones. |
| EQ-027 | REQ-HIST-002 | G | AMPLIADA | G08 muestra [60,60,62], variación deportiva +2 y reajuste +10 identificados. |
| EQ-028 | REQ-HIST-003 | M | MODIFICADA | Ausencia de un jugador en una vista parcial no adelanta su contador de retirada. |
| EQ-029 | REQ-REG-001 | M | AMPLIADA | Adulto sin dato de formación consta como incierto; en la simulación no satisface cuota local. |
| EQ-030 | REQ-REG-002 | G | MODIFICADA | G09 se conserva en referencia; con sueldo desconocido la lista muestra subtotal y viabilidad económica incierta. |
| EQ-031 | REQ-REG-003 | M | CONSERVADA | Dos adultos para dos plazas, sólo el de 80 inscrito, media 80; vaciar lista admite ambos y media 70. |
| EQ-032 | REQ-SAVE-001 | M | AMPLIADA | Copiar táctica deja la nueva carrera sin club, personas, fotos asociadas ni conexión activa del original. |
| EQ-033 | REQ-PROFILE-001 | M | AMPLIADA | Renombrar conserva huella matemática; editar un punto válido la cambia; sigue accesible la revisión anterior. |
| EQ-034 | REQ-TAC-004 | G | CONSERVADA | G13 acepta STC con PFB y señala NIX desconocido sin reemplazar la táctica activa. |
| EQ-035 | REQ-ROLE-003 | M | CONSERVADA | Rol manual X y predeterminado Y dan X+Y al completar y sólo Y al restablecer. |
| EQ-036 | REQ-SQUAD-004 | M | MODIFICADA | Aceleración 12–16 y ritmo desconocido dan media 14 y cobertura 1/2. |
| EQ-037 | REQ-NAT-002 | M | POSPUESTA | Al retomar, candidatos de 72 y 74 contra referencia 70 sugieren 74 sin convocarlo. |
| EQ-038 | REQ-EXPORT-001 | M | MODIFICADA | Exportar una lista filtrada de atributos con más filas que una página, un valor exacto, un rango 12–14, un valor desconocido y un nombre con coma. En ambos formatos coinciden selección, orden y valores. En Excel el exacto es numérico. En CSV se escapa el nombre. Sueldo oculto queda fuera por defecto y CA/PA desactivados quedan fuera de ambos archivos. Si falla Excel, CSV conserva la selección. |
| EQ-039 | REQ-DEV-004 | M | CONSERVADA | Pico 60, bloqueo 30 y peso 8 se ajustan a 50/50/5. |
| EQ-040 | REQ-SQUAD-005 | M | MODIFICADA | Ausente de una vista parcial conserva club; al cancelar la conciliación completa también. |
| EQ-041 | REQ-FM-003 | M | POSPUESTA | Si se habilita, verificar los cinco archivos contra el inventario y huellas del contrato 041. |
| EQ-042 | REQ-PROFILE-002 | M | CONSERVADA | Serie 50…69 da base 50 y peso 19; introducir 47 en nivel 10 mantiene el último borrador válido. |
