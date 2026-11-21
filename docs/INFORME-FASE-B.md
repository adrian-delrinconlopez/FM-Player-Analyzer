# Informe final de Fase B · FM27 Analyzer

**Resultado para revisión.** La especificación define cómo pasar de una observación autorizada de FM27 a una decisión de plantilla con evidencia. Por ejemplo, el usuario parte de una carencia en el extremo izquierdo, compara a dos candidatos con atributos parcialmente observados y ve por separado el cambio del XI y el gasto previsto. El mismo análisis puede consultarse sin red ni cuenta de IA. [Índice de la especificación](to-be/00-indice-y-vision.md)

| Tema | Resultado y consecuencia |
|---|---|
| Producto resultante | Aplicación de escritorio en castellano, exclusiva para FM27, con carreras aisladas, roles por fase, planificación y seguimiento durante la temporada. |
| Diferencias frente al original | Integración de sólo lectura mediante BepInEx y observaciones autorizadas. Perfil, fórmula, fecha e incertidumbre acompañan cada resultado. Se proponen cambios explícitos para Classic parcial, medias de atributos, importes desconocidos, revocación y exportación. |
| Tecnología confirmada | Windows 11 x64, C#, .NET 10 LTS, WinUI 3/XAML y Windows App SDK compatible, SQLite, Git, .slnx, VS Code y Visual Studio Enterprise. Spec Kit con Codex/Copilot. ChatGPT opcional como único proveedor inicial. |
| Mejoras definidas | Escenarios deportivos y económicos, filosofía configurable, perfiles alternativos, cortes incrementales, cohortes y comparación interanual, cuerpo técnico, recursos gráficos locales y herramientas de explicación. |
| Descartes y aplazamientos | Se descartan FM24, lectura de .fm, escritura en juego y UI web definitiva. Se posponen convocatoria nacional, instalación de los .fmf, migración histórica, sinergias y modelos experimentales. |
| Riesgos principales | Identidad estable de carrera y conocimiento de ojeo; origen de CA/PA, personal y estadísticas; catálogo lingüístico oficial, derechos de material, comportamiento de WinUI con 150.000 filas y elegibilidad real del acceso ChatGPT. |
| Aprobación del propietario | Revisar supresión de nota Classic incompleta, cobertura, importación manual, retención, prioridades, umbrales de muestra, licencia y modelo de consentimiento. La arquitectura candidata requiere pruebas técnicas separadas. |
| Siguiente paso | Revisar las decisiones abiertas y los recorridos del prototipo. Tras la revisión del TO-BE, construir el HTML exclusivamente con datos ficticios. Validar con el propietario antes de implementar la UI nativa. |

## Auditoría de coherencia

| Control | Evidencia documental o límite |
|---|---|
| Cobertura y cambios | 42 filas de origen con tratamiento, contrato, destino, impacto, dependencias y aceptación. 24 requisitos nuevos/transversales. [Matriz](to-be/18-trazabilidad-as-is-to-be.md) · [Catálogo](to-be/02-catalogo-funcional.md) |
| Equivalencia | 2.528 pares Classic de FM27 y 237 valores de curvas constituyen la referencia de comparación posterior. Una prueba manual o una fixture derivada conserva su etiqueta. [Equivalencia](testing/01-equivalencia-funcional.md) |
| Seguridad y opcionalidad | Autorización antes de ordenar, calcular y exportar, incluidas herramientas y revocación en curso. El plan incluye validación sin red. [Política](to-be/13-visibilidad-y-juego-limpio.md) |
| Viabilidad técnica | Versiones, dependencias y licencias documentadas a partir de fuentes primarias. Faltan pruebas en el entorno completo de juego/escritorio. [Stack](architecture/01-stack-tecnologico.md) · [Diez PoC con salida](roadmap/03-riesgos-y-pruebas-de-concepto.md) |
| Experiencia y nomenclatura | 19 superficies, estados de datos y errores, navegación, acciones de teclado, datos ficticios y pruebas de escalado. Identificadores y términos de referencia separados del catálogo oficial por verificar. [Diseño](ux-ui/03-catalogo-de-pantallas.md) · [Terminología](to-be/16-localizacion-y-nomenclaturas.md) |
| Entrega incremental | Prioridades por requisito, puertas por fase, criterios de aceptación y trabajo delimitado para agentes. [Spec Kit](roadmap/04-preparacion-spec-kit.md) |

Esta entrega es documental. No se han ejecutado las pruebas de la futura aplicación, instalado un plugin, solicitado una cuenta, generado el prototipo ni medido rendimientos del producto. Los umbrales y elecciones técnicas conservan su estado de propuesta. El [registro de cuestiones](OPEN-QUESTIONS.md) permite continuar con la evidencia que falta.

**Comprobaciones efectuadas sobre el repositorio.** Se cotejaron automáticamente los 42 identificadores del inventario AS-IS con las filas de trazabilidad, los 66 requisitos únicos con 42 casos EQ y 24 casos NV, y los 19 destinos y diez PoC con sus catálogos. Se contaron en los anexos originales los 2.528 pares y los 237 valores FM27, se verificó la unicidad de 169 claves de nomenclatura y se resolvieron los 190 enlaces locales de los 58 documentos de la entrega, incluidas las dos guías de agentes. La comparación con la base 4be8025 no encontró cambios en docs/as-is ni en old_version. Durante la revisión se corrigieron el tratamiento de la edad desconocida en entrenamiento y las posiciones elegibles de los extremos en la fase defensiva del guion.
