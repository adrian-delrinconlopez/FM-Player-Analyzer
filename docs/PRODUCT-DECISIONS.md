# Decisiones confirmadas del producto

Autoridad: encargo de Fase B y aclaraciones del propietario del 21 de noviembre de 2026. En este documento **CONFIRMADA** significa aprobada por el propietario. Una solución técnica candidata, una referencia comunitaria y el resultado de una prueba requieren registros distintos. Para cambiar una decisión, anotar fecha, motivo, IDs afectados y efecto en pruebas e históricos en el [registro de cambios](CHANGELOG-TO-BE.md).

| ID | Decisión CONFIRMADA | Consecuencia verificable |
|---|---|---|
| DP-01 | Producto nuevo, exclusivamente para Football Manager 27. | Toda importación, catálogo, perfil y táctica se vincula a FM27. Se excluye compatibilidad con FM24. |
| DP-02 | Windows 11 x64, C#, .NET 10 LTS, WinUI 3 nativo con XAML y Windows App SDK compatible. | HTML/CSS/JS queda reservado al prototipo. Electron, Tauri, React, Dioxus, WPF, Blazor Hybrid y WebView2 quedan descartados como base de UI. |
| DP-03 | SQLite es la fuente persistente local. Soporte para carreras independientes y sesiones concurrentes con el juego. | Aislar identidades y series, acotar memoria y evaluar volúmenes superiores a 100.000 registros. |
| DP-04 | BepInEx es la vía principal prevista, de sólo lectura. Quedan descartadas lectura de .fm y edición del juego. | El runtime del plugin depende del entorno IL2CPP validado. La lectura externa complementaria sólo puede investigarse como decisión futura. |
| DP-05 | Juego limpio por defecto. CA y PA deshabilitados, habilitación expresa con advertencia. | El permiso afecta a todas las superficies. Los campos dependen de disponibilidad validada. Ojeo y enmascaramiento tienen controles independientes del permiso de CA/PA. |
| DP-06 | ChatGPT es el único proveedor inicial y su uso es opcional. El motor local calcula indicadores. | Sin red, cuenta ni modelo se puede analizar, planificar, comparar y exportar. Las herramientas sólo acceden a proyecciones autorizadas. |
| DP-07 | Recursos locales en una carpeta graphics seleccionada por el usuario. | Acceso de lectura, asociación por ID y carga bajo demanda. Se excluye redistribuir paquetes de terceros. |
| DP-08 | Castellano para la documentación y primera interfaz. Nomenclatura, grupos y orden oficiales españoles de FM27. | Datos sin verificar se marcan pendientes. Inglés seleccionable previsto para segunda entrega. |
| DP-09 | Prototipo HTML/CSS/JS interactivo, autónomo, ficticio, anterior a la UI final. | Fase B entrega sus especificaciones; la revisión precede al inicio del prototipo y la validación visual precede a implementar la UI definitiva. |
| DP-10 | Git y GitHub Spec Kit, Codex y/o Copilot, una solución .slnx para VS Code y Visual Studio Enterprise. | Los comandos de compilación y prueba se pueden ejecutar sin un IDE. El propietario puede dirigir cada paso. |
| DP-11 | Conservar algoritmos valiosos y explicar las diferencias. Preferencias deportivas configurables. | Calidad, rol, táctica, filosofía y conveniencia económica se muestran como indicadores diferenciados. |
