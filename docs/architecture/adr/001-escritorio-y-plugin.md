# ADR 001 · Separar escritorio y adaptador de FM27

**PROPUESTA.** .NET 10 y WinUI residen en el escritorio. Un plugin de lectura usa el runtime y las referencias compatibles con el IL2CPP de la compilación validada. Comunican observaciones por protocolo local versionado.

**Alternativas.** Cargar en el juego todo el motor analítico acopla CPU, memoria y dependencias al juego. El intercambio por archivo visible es una alternativa si el canal local falla. El canal por pipes permite confirmar lotes y negociar capacidades con menor demora, a cambio de gestionar reconexión y dos versiones.

**Aceptación.** Reiniciar cualquiera de los procesos entre páginas debe preservar la última revisión confirmada. Una actualización incompatible deja la captura pausada. Revisar con PC-01/04 y con el contrato de reintento de integración.
