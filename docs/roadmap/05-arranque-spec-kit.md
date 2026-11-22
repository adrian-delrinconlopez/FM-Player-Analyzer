# Empezar con GitHub Spec Kit en un equipo Windows

La raíz del repositorio ya contiene un proyecto Spec Kit 1.4.3. Para retomar el trabajo en otro ordenador basta con obtener la rama que contiene esta preparación, instalar la CLI fijada y abrir la raíz. La primera sesión está dedicada a discutir la [constitución 0.1.0](../../.specify/memory/constitution.md).

## Qué viaja con Git

| Elemento | Ubicación y función |
|---|---|
| Reglas y autoridad | [AGENTS.md](../../AGENTS.md), [decisiones confirmadas](../PRODUCT-DECISIONS.md) y [cuestiones abiertas](../OPEN-QUESTIONS.md). La constitución propuesta utiliza estos documentos como referencia. |
| Infraestructura oficial | `.specify/` contiene metadatos de versión, plantillas, scripts PowerShell, manifiestos y flujo estándar. Se conserva el idioma de los archivos oficiales para poder comprobar sus huellas. |
| Comandos del agente | `.agents/skills/speckit-*/SKILL.md` contiene las nueve habilidades. Codex es la integración registrada. El [espacio de trabajo de VS Code](../../FM27-Analyzer.code-workspace) indica la misma carpeta a Copilot mediante la configuración de ubicaciones de habilidades. |
| Trabajo futuro | Las especificaciones y planes de cada capacidad irán en `specs/`. El puntero de capacidad activa `.specify/feature.json` es local a cada clon. Antes de retomar una capacidad, comprueba la rama y el directorio de la especificación. |

## Preparar un ordenador

1. Instala Git, PowerShell 7 y VS Code si aún no están disponibles. Configura tu identidad Git y autentícate en GitHub con tu cuenta habitual para acceder al repositorio. Comprueba `git --version`, `pwsh --version` y `code --version`. Si ya tienes el repositorio, utiliza tu clon actual.
2. Instala uv para tu usuario. Abre una **terminal nueva** después de la instalación para recoger el PATH. uv puede obtener Python 3.11 cuando no lo encuentre. La CLI quedará aislada de los paquetes Python del proyecto.

   ```powershell
   winget install --id astral-sh.uv --exact --source winget --scope user
   ```

   ```powershell
   uv python install 3.11
   uv tool install --python 3.11 "specify-cli==1.4.3"
   uv tool update-shell
   ```

3. Abre una nueva sesión de PowerShell 7. Obtén esta rama después de publicarla en el remoto, o la rama principal una vez integrada. Si trabajas desde este ordenador, los commits locales ya están disponibles.

   ```powershell
   git clone --branch chore/preparar-spec-kit https://github.com/adrian-delrinconlopez/FM-Player-Analyzer.git
   Set-Location FM-Player-Analyzer
   git status --short --branch
   code FM27-Analyzer.code-workspace
   ```

4. Elige el agente. Para Codex en VS Code, instala la extensión `OpenAI.chatgpt` e inicia sesión. En una sesión nueva abierta en la raíz, comprueba el selector de habilidades con `$speckit-constitution`. Para Copilot, instala o activa `GitHub.copilot-chat`, inicia sesión con una cuenta habilitada y abre el archivo de espacio de trabajo. Busca `/speckit-constitution` en el chat. Los nombres de esta versión llevan guion. Las políticas corporativas y el cliente pueden afectar al descubrimiento. En Copilot, si no aparece, pide al agente leer `.agents/skills/speckit-constitution/SKILL.md` y seguir su flujo para el proyecto. Las sesiones y los permisos de cada cuenta se configuran por equipo. [Habilidades en VS Code](https://code.visualstudio.com/docs/agent-customization/agent-skills)

## Comprobación sin crear una funcionalidad

Desde la raíz del clon en PowerShell 7:

```powershell
specify --version
specify integration status
pwsh -NoProfile -File .specify/scripts/powershell/create-new-feature.ps1 -DryRun -Json -ShortName comprobar-entorno "Comprobar el entorno"
$plan = pwsh -NoProfile -File .specify/scripts/powershell/resolve-template.ps1 plan-template -Json
($plan | ConvertFrom-Json).TEMPLATE_NAME
git status --short
```

Resultados esperados: `specify 1.4.3`, estado `OK`, integración `codex`, cero archivos gestionados ausentes o modificados, `DRY_RUN: true` y `plan-template`. En un clon limpio la última orden no produce salida. Si la terminal no encuentra specify, comprueba `uv tool dir --bin` y vuelve a abrir el editor y la terminal. Si PowerShell bloquea un script, revisa la procedencia y la política de ejecución de tu organización. No hace falta repetir `specify init` en cada equipo.

## Primera conversación con el agente

Abre la constitución y el registro de cuestiones. Puedes pegar directamente este encargo:

> Revisa conmigo el borrador 0.1.0 de la constitución de FM27 Analyzer. Contrástalo con PRODUCT-DECISIONS y OPEN-QUESTIONS. Empieza por las ambigüedades de identidad de carrera, visibilidad histórica y equivalencia de fórmulas. Para cada una explícame las alternativas y el efecto en el modelo de datos o en sus pruebas. Por ahora, sólo discusión; espera a que te pida editar o ratificar.

Después de debatir, invoca la habilidad de constitución indicando los acuerdos concretos que quieras incorporar y si autorizas ratificar. El siguiente paso disponible es Specify sobre un solo recorrido aprobado, por ejemplo el del prototipo con datos ficticios. Clarify, Plan, Tasks, Analyze y Converge tienen entradas y salidas en el [mapa de trabajo](04-preparacion-spec-kit.md). Puedes parar y revisar cada documento. El repositorio incluye el flujo estándar upstream para uso futuro; lanzar el ciclo completo también puede alcanzar Implement.

## Inventario externo por etapa

| Cuándo | Software, acceso o configuración fuera de Git | Verificación |
|---|---|---|
| Ahora, en cada ordenador | Git, PowerShell 7, uv, Python 3.11 para la instalación aislada, Specify 1.4.3, editor y una cuenta de agente autorizada | Comandos anteriores y descubrimiento en una sesión nueva. |
| Al sincronizar trabajo entre equipos | Acceso al remoto, credenciales de usuario, rama publicada y commits de las especificaciones | Comparar `git rev-parse HEAD` en ambos equipos. Desde el equipo de origen, cuando quieras compartir esta rama, ejecutar `git push -u origin chore/preparar-spec-kit`. |
| Para revisar el prototipo futuro | Navegador de escritorio con ejecución de archivos HTML locales | Abrir la demo sin conexión y seguir el guion visual cuando exista. |
| Antes de PC-04/10 y del escritorio | Windows 11 x64, .NET SDK 10 LTS, Windows SDK y herramientas WinUI compatibles, VS Code y Visual Studio Enterprise con sus componentes de desarrollo de escritorio Windows | Registrar versiones y compilar la misma .slnx en ambos IDE y en terminal. La revisión de Windows App SDK se cerrará con la PoC. |
| Para PoC específicas | FM27 con copia de prueba y build identificada, BepInEx/IL2CPP compatible, carpeta graphics autorizada; cuenta OpenAI elegible sólo para PC-09 | Aplicar el [protocolo de cada PoC](03-riesgos-y-pruebas-de-concepto.md). Credenciales, partidas, packs gráficos y certificados permanecen fuera de Git. |

**Equipo de preparación, 22-11-2026.** Git 2.34.1, PowerShell 7.1.4, Python 3.11, VS Code 1.147.0, uv 0.13.9 y Specify 1.4.3. VS Code dispone de la extensión de Codex y el instalador de extensiones reconoce Copilot Chat. El SDK .NET detectado fue 3.1.426; .NET 10 y la comprobación de Visual Studio Enterprise quedan para la fase nativa. La autenticación interactiva y la visualización del menú de habilidades deben comprobarse con la cuenta correspondiente.

**Mantenimiento.** La versión está fijada también en `.specify/init-options.json`. Ensayar actualizaciones en una rama independiente, revisar diferencias en habilidades y plantillas, conservar la constitución de proyecto y repetir las comprobaciones en un clon limpio. En 1.4.3 el CLI marca como error registrar simultáneamente las integraciones Codex y Copilot; se eligió una sola integración y una carpeta de habilidades compartida. [Versión oficial utilizada](https://github.com/github/spec-kit/releases/tag/v1.4.3)
