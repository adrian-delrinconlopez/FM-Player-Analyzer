# FM27 Analyzer · instrucciones para agentes

## Producto y autoridad

FM27 Analyzer es una aplicación nueva para Football Manager 27 en Windows 11 x64. Trabaja en castellano en especificaciones, documentación de uso y decisiones. Conserva nombres propios, IDs y denominaciones técnicas originales cuando sean necesarios. La interfaz inicial usa castellano y se prepara para inglés en la segunda entrega.

- Empieza por el [índice TO-BE](docs/to-be/00-indice-y-vision.md) y las [decisiones del propietario](docs/PRODUCT-DECISIONS.md). Las propuestas, los experimentos pendientes y las cuestiones abiertas se resuelven en sus registros. Un agente puede recomendar; la confirmación de alcance corresponde al propietario.
- La referencia funcional congelada es [AS-IS 2.3.1](docs/as-is/00-indice-y-alcance.md). Conserva intacta esa carpeta. Usa el código de `old_version/` únicamente si la documentación deja ambigua una regla. FM24 es referencia histórica y está fuera del producto.
- Identifica rama, commit y cambios locales antes de tocar archivos. Conserva trabajo ajeno, etiquetas y artefactos publicados. Cada bloque documental o funcional debe tener un commit descriptivo y una verificación que otro agente pueda repetir.

## Trabajo por capacidad

1. Elige los IDs de [requisitos](docs/to-be/02-catalogo-funcional.md), el tratamiento del [antecedente](docs/to-be/18-trazabilidad-as-is-to-be.md), las pruebas y el hito relevante. Limita la tarea a un resultado observable.
2. Distingue dato observado, derivado, estimado, desconocido y restringido. Etiqueta edición, origen, revisión de perfil, cobertura y fecha. Una fórmula heredada sólo cambia con motivo, expectativa previa y expectativa nueva.
3. Respeta las [políticas de visibilidad](docs/to-be/13-visibilidad-y-juego-limpio.md) antes de toda consulta, cálculo, filtro, ranking, exportación o herramienta. CA/PA apagados por defecto. Una sesión, trabajo o caché no cruza carreras ni revisiones de permiso.
4. Separa núcleo determinista, casos de uso, SQLite, adaptador BepInEx de sólo lectura, presentación WinUI y cliente opcional de ChatGPT. Usa una solución .slnx y los mismos proyectos en terminal, VS Code y Visual Studio Enterprise. Reutiliza contratos probados en lugar de duplicar decisiones en controles de pantalla.
5. Valida primero el caso afectado, después sus consumidores y los casos extremos. Registra entradas, comandos, salidas, pruebas reales omitidas y límites. Las pruebas con juego o datos de usuario utilizan copias o fixtures. Los resultados sintéticos y las referencias de terceros mantienen esa etiqueta.
6. Actualiza requisitos, pruebas, decisiones abiertas y [registro de cambios](docs/CHANGELOG-TO-BE.md) cuando cambie comportamiento o alcance. Para WinUI consulta habilidades oficiales, muestras y metadatos del SDK elegido. La compilación debe ser independiente del agente.

## Estado de trabajo

Spec Kit está inicializado para trabajo manual por capacidades. Consulta la [guía por equipo](docs/roadmap/05-arranque-spec-kit.md) y la [constitución en revisión](.specify/memory/constitution.md). Al debatir una propuesta, distingue la revisión del texto, su edición y la ratificación por el propietario.

Fase B entrega especificaciones. El propietario revisa esos documentos antes de iniciar el prototipo HTML ficticio. La aprobación del prototipo antecede a implementar la UI definitiva. Las PoC se ejecutan al comienzo de sus fases, con los criterios del [roadmap](docs/roadmap/03-riesgos-y-pruebas-de-concepto.md). Las instrucciones para preparar Spec Kit están en el [flujo de desarrollo](docs/roadmap/04-preparacion-spec-kit.md).
