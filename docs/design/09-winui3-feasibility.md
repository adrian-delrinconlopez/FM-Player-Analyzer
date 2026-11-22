# Factibilidad en WinUI 3 y pruebas que decidirían la implementación

DP-02 fija Windows 11 x64, C#, .NET 10 y WinUI 3/XAML. La UI consume consultas de aplicación y resultados del núcleo. La ordenación por rol, la elegibilidad, el cálculo de percentiles y el permiso efectivo se resuelven antes de crear elementos visuales. El [ADR 004](../architecture/adr/004-tablas-y-graficos.md) y el [inventario de bibliotecas](../architecture/06-bibliotecas-y-componentes.md) mantienen abiertas las elecciones de controles. El mock HTML sirve para probar la comprensión de una tarea.

## Qué se ha comprobado

El manifiesto NuGet de **Microsoft.WindowsAppSDK 2.7.0**, revisión candidata del repositorio, declara **Microsoft.WindowsAppSDK.WinUI 2.7.0**. Se abrió el paquete público sin instalarlo. La lectura de TypeDefinitions en `metadata/Microsoft.UI.Xaml.winmd` encontró NavigationView, ListView, Grid, Canvas, ItemsRepeater y SelectorBar en `Microsoft.UI.Xaml.Controls`. La misma consulta exacta devolvió ausencia para DataGrid y TableView en ese ensamblado. En la documentación XML del paquete figuran UIElement.AllowDrop, ListViewBase.CanDragItems, BitmapImage.DecodePixelWidth, ElementCompositionPreview y AutomationProperties. La evidencia es de superficie de API en ese paquete. Compatibilidad de despliegue, semántica de columnas, accesibilidad y rendimiento en una aplicación real requieren las PoC. [Metapaquete](https://www.nuget.org/packages/Microsoft.WindowsAppSDK/2.7.0) · [Paquete de WinUI](https://www.nuget.org/packages/Microsoft.WindowsAppSDK.WinUI/2.7.0)

| Caso | Primera opción que probar y alternativa | Riesgo concreto y prueba de salida |
|---|---|---|
| Marco, búsqueda, navegación y vuelta | NavigationView adaptable, AutoSuggestBox si se aprueba la búsqueda, Frame/detalle y selector de fase. Guardar parámetros y UID en vez de retener páginas completas. | Volver a la misma fila tras ordenar, degradar a menú a 200 %, descartar retorno de otra carrera. Medir memoria tras 100 ciclos y probar lectura del destino. [Muestra oficial](https://github.com/microsoft/WinUI-Gallery/blob/main/WinUIGallery/Samples/NavigationView/NavigationViewPage.xaml) |
| Miles de filas y columnas configurables | ListView con plantilla de Grid y encabezado coordinado. Virtualización de UI para reciclar elementos y paginación de datos para limitar memoria. Una tabla especializada requiere verificar canal, versión y modelo de columnas. | Evitar una colección residente de 150.000 personas y un ScrollViewer externo ilimitado. PC-05 mide con 10k/50k/100k/150k, ≤500 filas en ventana, orden reproducible y foco/selección por UID. Ensayar lector con encabezados, rango y estado del orden. [Muestra](https://github.com/microsoft/WinUI-Gallery/blob/main/WinUIGallery/Samples/ListView/ListViewPage.xaml) · [Virtualización de datos](https://learn.microsoft.com/en-us/windows/apps/develop/performance/listview-and-gridview-data-optimization) |
| Filtros, avisos y configuración | Controles nativos y etiquetas para rango, combos y permiso. InfoBar para fallo local recuperable. ContentDialog para activación expresa. Comparar SettingsControls del Windows Community Toolkit si ahorra comportamiento repetido. | Mantener el motor de permisos en aplicación. Probar cancelación entre dos consultas y cambios de carrera durante autocompletado, además de devolución de foco. Validar dependencias efectivas del Toolkit en PC-04. [Toolkit](https://github.com/CommunityToolkit/Windows) |
| Radar, pizza y barras | Para pocos ejes, geometría XAML o Win2D con rótulos y tabla. LiveChartsCore.SkiaSharpView.WinUI es candidato alternativo para familias soportadas. Pizza exige sectores de radio independiente y huecos distinguibles. Sólo investigación mientras rija la revisión Q-16. | No asumir que un control polar resuelve ese diseño o su UIA. Probar 6 ejes con un dato faltante, escalas y dirección fijas, par seleccionable y valores exactos en tabla. Comparar coste de mantenimiento, transitivas y tamaño instalado. [Win2D](https://learn.microsoft.com/en-us/windows/apps/develop/win2d/) · [Paquete WinUI de LiveCharts](https://www.nuget.org/packages/LiveChartsCore.SkiaSharpView.WinUI/2.2.0) |
| Dispersión densa, distribuciones y calor | Dibujo por lotes en Win2D o renderer de gráficos tras PC-07. Selección mediante la misma proyección que la tabla, agrupación de puntos coincidentes con recuento visible. Histograma como distribución de clases explícitas. | Comparar 10.000 puntos y 100 ciclos de navegación, GPU en reposo, selección, zoom si aporta valor y tabla equivalente. Evitar un elemento XAML y un nodo UIA por cada punto. Un mapa real espera coordenadas observables y orientación acreditada. [Win2D](https://learn.microsoft.com/en-us/windows/apps/develop/win2d/) |
| Campo, marcadores y movimiento | Fondo de campo + botones XAML con foco y nombres. Usar coordenadas relativas y SelectorBar para alternar fase. Arrastrar y «Asignar al puesto» llaman al mismo caso de uso. Composition es candidata para mover capas si se mide beneficio. | Probar 22 marcadores, dos formaciones, destino ilegal, lesión, bloqueo, cambio de fase y escala 200 %. Un visual dibujado necesita interacción y semántica accesible explícitas. Preferir botones si el dibujo impide selección por teclado. [Capa visual](https://learn.microsoft.com/en-us/windows/apps/develop/composition/visual-layer) · [PC-06](../roadmap/03-riesgos-y-pruebas-de-concepto.md) |
| Retrato, escudo y miniaturas | Image con decodificación al tamaño de presentación, caché limitada por raíz/tipo/ID y demanda de filas visibles. Dibujo de iniciales de respaldo. | El reciclado no debe asociar una respuesta tardía a otro UID. PC-08 prueba 50.000 entradas, archivo roto, raíz movida, colisiones y cancelación. Medir scroll con/sin fotos. [Recursos gráficos](../to-be/15-recursos-graficos.md) |
| Históricos, explicación y chat | Lista o tabla para observaciones y evidencia. Crear renderer de líneas sólo al necesitar series. Lista de mensajes con actualización por bloques y enlaces resueltos con permiso actual. | Dejar huecos en la serie, distinguir respuesta parcial y estado final. Cancelación, lector y retorno al invocador antes/después de revocar. Sin red se mantienen ficha y explicación local. [Pruebas de integración](../testing/05-validacion-integraciones.md) |

### Cómo reproducir la inspección de metadatos

En PowerShell 7 con acceso a NuGet y carpeta temporal escribible. El paquete se abre como archivo ZIP y la consulta enumera nombres exactos. Huella SHA-256 obtenida: `027ecdc0c51a2d31fd25985616ef2e9438f02ee3e7b6e72fd0601ddb119688b4`.

```powershell
$uri = 'https://api.nuget.org/v3-flatcontainer/microsoft.windowsappsdk.winui/2.7.0/microsoft.windowsappsdk.winui.2.7.0.nupkg'
$pkg = Join-Path ([IO.Path]::GetTempPath()) 'fm27-ui-public.nupkg'
Invoke-WebRequest -Uri $uri -OutFile $pkg
Get-FileHash $pkg -Algorithm SHA256
$zip = [IO.Compression.ZipFile]::OpenRead($pkg)
$entry = $zip.GetEntry('metadata/Microsoft.UI.Xaml.winmd').Open()
$stream = [IO.MemoryStream]::new()
$entry.CopyTo($stream)
$stream.Position = 0
$pe = [Reflection.PortableExecutable.PEReader]::new($stream)
$md = [Reflection.Metadata.PEReaderExtensions]::GetMetadataReader($pe)
$types = foreach ($h in $md.TypeDefinitions) {
    $t = $md.GetTypeDefinition($h)
    if ($md.GetString($t.Namespace) -eq 'Microsoft.UI.Xaml.Controls') {
        $md.GetString($t.Name)
    }
}
'NavigationView','ListView','Grid','Canvas','SelectorBar','ItemsRepeater','TableView','DataGrid' |
    ForEach-Object { "$_ $($types -ccontains $_)" }
$pe.Dispose()
$stream.Dispose()
$entry.Dispose()
$zip.Dispose()
```

**Al planificar.** Desde el proyecto efectivamente restaurado, usar WinApp CLI 0.7+ y `winapp find-ui "ListView"`, después `winapp find-api members ListView --project-dir .`. Usar el mismo TFM, RID y versión exacta del plan. La consulta documental de este pack se hizo sin proyecto restaurado. Los presupuestos propuestos del banco son p95 ≤250 ms de filtro, p95 ≤20 ms por fotograma y aumento final en reposo ≤30 MiB. Registrar hardware, semilla 2701, frío/caliente, N, trazas y carga del juego. La comparación PC-05/06/07 puede favorecer una tabla simple, un renderer de terceros o un control propio. [Protocolo y límites](../testing/03-validacion-rendimiento.md) · [Gallery](https://github.com/microsoft/WinUI-Gallery)
