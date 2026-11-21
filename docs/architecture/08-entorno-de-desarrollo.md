# Entorno compartido para VS Code y Visual Studio Enterprise

Seleccionar una versión de Visual Studio Enterprise que admita el SDK .NET 10 elegido, herramientas de escritorio Windows/WinUI y Windows SDK x64 requerido. En VS Code, C# y herramientas WinUI compatibles pueden ayudar a navegar y depurar. Los comandos de restauración, compilación, ejecución de pruebas y empaquetado nacen del mismo repositorio. El núcleo y las pruebas deterministas deben poder ejecutarse por terminal. [Herramientas de desarrollo Windows](https://learn.microsoft.com/en-us/windows/apps/whats-new/whats-new-for-developers)

| Punto de trabajo | VS Code | Visual Studio Enterprise | Evidencia de equivalencia |
|---|---|---|---|
| Abrir | Carpeta raíz y solución .slnx | Abrir ese mismo .slnx | Misma lista de proyectos, TFM, RID y configuraciones. |
| Compilar/depurar | Tarea que invoca comandos comunes, depuración C# del proceso x64 | Configuración equivalente y arranque del mismo proyecto | Versiones de ensamblados, recursos y fixture de DWRS idénticos. |
| Probar | Terminal y salida del ejecutor | Explorador de pruebas o terminal | Mismos casos y resultados, separando pruebas que requieren el juego. |
| Diseñar | Galería WinUI, metadatos restaurados y CLI | XAML, depuración y herramientas nativas cuando proceda | Abrir/cerrar diálogo, teclado y escala observados en ambos lanzamientos. |

Las habilidades oficiales de Microsoft y WinApp CLI ayudan a descubrir controles (find-ui), verificar APIs con las referencias del proyecto (find-api) e inspeccionar UI Automation. Registrar versión de WinApp antes de depender de un comando. Las habilidades son ayudas para agentes y revisión; la compilación no requiere un agente ni un asistente. PC-10 debe guardar los comandos e inventario real por IDE, incluyendo la instalación que falte. [WinApp CLI](https://github.com/microsoft/WinAppCli) · [Formato .slnx](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/10.0/dotnet-new-sln-slnx-default)
