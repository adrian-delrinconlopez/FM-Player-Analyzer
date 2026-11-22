# FM27 Analyzer · definición de producto

**Fase B · 21 de noviembre de 2026 · especificación para revisión del propietario.** Una partida plantea tres preguntas diferentes: quién puede jugar hoy, qué conviene cambiar y cómo ha evolucionado el equipo. El analizador debe permitir responderlas con los datos que conoce el mánager. Desde una carencia se llega al puesto, a los suplentes, a los candidatos y al efecto calculado de un posible cambio. Cada cifra conserva su fecha, unidad y procedencia.

La referencia funcional es FM Player Analyzer 2.3.1, revisión de producto `126c97007946604b4e83bb6c089b473442581753`; la documentación AS-IS se leyó en `4be8025`. La nueva aplicación admite exclusivamente Football Manager 27 para PC. La documentación histórica contiene ejemplos de otras ediciones. Su presencia identifica la procedencia de un comportamiento. [Inventario de partida](../as-is/00-indice-y-alcance.md)

## Cómo tomar una decisión

1. Consultar el [registro de decisiones confirmadas](../PRODUCT-DECISIONS.md). El [registro abierto](../OPEN-QUESTIONS.md) identifica decisiones de diseño pendientes y pruebas de viabilidad.
2. Buscar la tarea en el catálogo funcional y seguir su ID hasta la matriz si tiene antecedente. Los capítulos de dominio especifican el comportamiento común a varias tareas.
3. Usar el catálogo de pantallas y el guion del prototipo para revisar la experiencia. La implementación de ese prototipo empieza después de la revisión de esta documentación.
4. Antes de planificar una capacidad, comprobar su prueba, sus fuentes y su dependencia en el roadmap. [Balance de la fase](../INFORME-FASE-B.md)

La revisión del 22 de noviembre concreta el recorrido de refuerzo, la lectura económica y el uso de Moneyball en las vistas existentes. Sus cambios de alcance están pendientes de ratificación. [Contraste con ScoutSheet.fm](19-contraste-scoutsheet.md)

## Mapa de lectura

### Producto y comportamiento

- [indice y vision](../to-be/00-indice-y-vision.md)
- [alcance y requisitos globales](../to-be/01-alcance-y-requisitos-globales.md)
- [catalogo funcional](../to-be/02-catalogo-funcional.md)
- [modelo funcional de datos](../to-be/03-modelo-funcional-de-datos.md)
- [valoraciones y perfiles](../to-be/04-valoraciones-y-perfiles.md)
- [tacticas y optimizacion](../to-be/05-tacticas-y-optimizacion.md)
- [plantilla y planificacion](../to-be/06-plantilla-y-planificacion.md)
- [scouting y mercado](../to-be/07-scouting-y-mercado.md)
- [desarrollo y entrenamiento](../to-be/08-desarrollo-y-entrenamiento.md)
- [estadisticas y moneyball](../to-be/09-estadisticas-y-moneyball.md)
- [historicos y temporadas](../to-be/10-historicos-y-temporadas.md)
- [inscripcion y elegibilidad](../to-be/11-inscripcion-y-elegibilidad.md)
- [integracion fm27](../to-be/12-integracion-fm27.md)
- [visibilidad y juego limpio](../to-be/13-visibilidad-y-juego-limpio.md)
- [asistente chatgpt](../to-be/14-asistente-chatgpt.md)
- [recursos graficos](../to-be/15-recursos-graficos.md)
- [localizacion y nomenclaturas](../to-be/16-localizacion-y-nomenclaturas.md)
- [configuracion](../to-be/17-configuracion.md)
- [trazabilidad as is to be](../to-be/18-trazabilidad-as-is-to-be.md)
- [contraste con ScoutSheet.fm](19-contraste-scoutsheet.md)

### Arquitectura y decisiones técnicas

- [arquitectura general](../architecture/00-arquitectura-general.md)
- [stack tecnologico](../architecture/01-stack-tecnologico.md)
- [modelo de persistencia](../architecture/02-modelo-de-persistencia.md)
- [integraciones y contratos](../architecture/03-integraciones-y-contratos.md)
- [rendimiento y memoria](../architecture/04-rendimiento-y-memoria.md)
- [seguridad y privacidad](../architecture/05-seguridad-y-privacidad.md)
- [bibliotecas y componentes](../architecture/06-bibliotecas-y-componentes.md)
- [compilacion y distribucion](../architecture/07-compilacion-y-distribucion.md)
- [entorno de desarrollo](../architecture/08-entorno-de-desarrollo.md)
- [decisiones tecnicas pendientes](../architecture/09-decisiones-tecnicas-pendientes.md)

### Diseño y prototipo

- [Cuaderno de investigación UX/UI, referencias visuales y trazabilidad por requisito](../design/00-index.md)
- [vision visual](../ux-ui/00-vision-visual.md)
- [sistema de diseno](../ux-ui/01-sistema-de-diseno.md)
- [navegacion y flujos](../ux-ui/02-navegacion-y-flujos.md)
- [catalogo de pantallas](../ux-ui/03-catalogo-de-pantallas.md)
- [catalogo de componentes](../ux-ui/04-catalogo-de-componentes.md)
- [especificacion prototipo html](../ux-ui/05-especificacion-prototipo-html.md)
- [criterios de aceptacion visual](../ux-ui/06-criterios-de-aceptacion-visual.md)

### Validación

- [estrategia de validacion](../testing/00-estrategia-de-validacion.md)
- [equivalencia funcional](../testing/01-equivalencia-funcional.md)
- [casos de prueba](../testing/02-casos-de-prueba.md)
- [validacion rendimiento](../testing/03-validacion-rendimiento.md)
- [validacion interfaz](../testing/04-validacion-interfaz.md)
- [validacion integraciones](../testing/05-validacion-integraciones.md)

### Entregas y Spec Kit

- [roadmap general](../roadmap/00-roadmap-general.md)
- [fases y entregables](../roadmap/01-fases-y-entregables.md)
- [dependencias y prioridades](../roadmap/02-dependencias-y-prioridades.md)
- [riesgos y pruebas de concepto](../roadmap/03-riesgos-y-pruebas-de-concepto.md)
- [preparacion spec kit](../roadmap/04-preparacion-spec-kit.md)
