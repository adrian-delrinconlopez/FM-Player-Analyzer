using System.Diagnostics;
using System.Globalization;
using FM27Analyzer.Extraction.Poc.Capture;
using UnityEngine;
using UnityEngine.UIElements;

namespace FM27Analyzer.BepInExProbe;

internal sealed class ScoutedCensus
{
    private const int MaxFramesWithData = 100;
    private static readonly TimeSpan TimeLimit = TimeSpan.FromSeconds(30);
    private readonly CaptureSession session;
    private readonly CaptureContext context;
    private readonly Action<object, Action<Action>> store;
    private readonly int expectedCount;
    private readonly long processStart;
    private readonly ulong loads;
    private readonly Stopwatch timer = Stopwatch.StartNew();
    private readonly Dictionary<int, (string Uid, string Passing)> rows = new();
    private readonly HashSet<string> uids = new(StringComparer.Ordinal);
    private View? view;
    private Vector2 originalOffset;
    private float requestedOffset;
    private int frames;
    private int pages;
    private double readingMilliseconds;

    public ScoutedCensus(int count, CaptureContext context, Func<CaptureContext?> current,
        long processStart, ulong loads, Action<object, Action<Action>> store)
    {
        this.context = context;
        this.store = store;
        this.processStart = processStart;
        this.loads = loads;
        expectedCount = count;
        session = new(context, current, _ => { });
    }

    public bool Tick()
    {
        try
        {
            if (timer.Elapsed > TimeLimit || pages >= MaxFramesWithData)
                throw new InvalidDataException("Límite del recorrido de ojeados.");
            if (view is null)
            {
                view = session.Read(FindView);
                originalOffset = view.Scroll.scrollOffset;
                MoveTo(0);
                return false;
            }
            session.Read(() => CheckView(view));
            if (--frames > 0) return false;

            var clock = Stopwatch.StartNew();
            session.Read(() => ReadPage(view));
            readingMilliseconds += clock.Elapsed.TotalMilliseconds;
            pages++;
            float bottom = view.Scroll.verticalScroller.highValue;
            if (bottom - requestedOffset > 1)
            {
                float step = view.Scroll.contentViewport.worldBound.height * 0.65f;
                if (step < 10) throw new InvalidDataException("La altura de página no está disponible.");
                MoveTo(Math.Min(bottom, requestedOffset + step));
                return false;
            }

            if (rows.Count != expectedCount)
                throw new InvalidDataException("El total de filas recuperadas difiere del total anunciado.");
            var result = rows.OrderBy(pair => pair.Key).Select(pair => new
            {
                pair.Value.Uid,
                Pase = pair.Value.Passing,
                Estado = ProbeComponent.IsUnknown(pair.Value.Passing) ? "desconocido"
                    : ProbeComponent.MatchesPrecision(pair.Value.Passing, "inclusiveRange") ? "intervalo" : "exacto"
            }).ToArray();
            store(new
            {
                context.Build, Date = context.GameDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                context.Cut, ProcessStartUtcTicks = processStart, Loads = loads,
                ExpectedCount = expectedCount, Pages = pages, ReadMilliseconds = readingMilliseconds,
                ElapsedMilliseconds = timer.Elapsed.TotalMilliseconds, Rows = result
            }, session.Commit);
            Finish();
            return true;
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or InvalidOperationException
            or OperationCanceledException or UnauthorizedAccessException)
        {
            Finish();
            try
            {
                store(new { Reason = ex is InvalidDataException ? ex.Message : "Censo cancelado o contexto no verificable." },
                    publish => publish());
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            return true;
        }
    }

    private void Finish()
    {
        session.Dispose();
        if (view is { } active && active.Scroll.panel is not null
            && Math.Abs(active.Scroll.scrollOffset.y - requestedOffset) <= 1)
            active.Scroll.scrollOffset = originalOffset;
    }

    private void MoveTo(float offset)
    {
        requestedOffset = offset;
        view!.Scroll.scrollOffset = new Vector2(originalOffset.x, offset);
        frames = 4;
    }

    private float CheckView(View active)
    {
        if (active.Table.panel is null || !Shown(active.Table) || !Shown(active.CountLabel)
            || active.CountLabel.text != active.OriginalLabel)
            throw new InvalidDataException("Cambió la tabla o el total de ojeados.");
        float offset = active.Scroll.scrollOffset.y;
        if (Math.Abs(offset - requestedOffset) > 1)
            throw new InvalidDataException("Se desplazó la vista durante el censo.");
        return offset;
    }

    private int ReadPage(View active)
    {
        var clock = Stopwatch.StartNew();
        var viewport = active.Scroll.contentViewport.worldBound;
        float contentTop = active.Scroll.contentContainer.worldBound.y;
        foreach (var row in Walk(active.Scroll.contentContainer, clock, IsRow).Where(IsRow))
        {
            var bounds = row.worldBound;
            if (bounds.yMin < viewport.yMin - 1 || bounds.yMax > viewport.yMax + 1 || bounds.height < 5) continue;
            var cells = Walk(row, clock, IsCell).Where(IsCell).ToArray();
            if (cells.Length != active.Columns) continue;
            string uid = Text(cells[active.IdColumn], clock);
            string passing = Text(cells[active.PassColumn], clock);
            if (!ProbeComponent.ValidUid(uid) || !ProbeComponent.IsUnknown(passing) && !ProbeComponent.MatchesPrecision(passing, "visible"))
                continue;
            int position = (int)Math.Round(bounds.yMin - contentTop);
            if (rows.TryGetValue(position, out var previous))
            {
                if (previous != (uid, passing)) throw new InvalidDataException("La misma fila cambió durante el recorrido.");
                continue;
            }
            if (!uids.Add(uid)) throw new InvalidDataException("UID repetido en distintas filas de ojeados.");
            if (rows.Count >= expectedCount) throw new InvalidDataException("La vista supera el total autorizado.");
            rows.Add(position, (uid, passing));
        }
        return rows.Count;
    }

    private View FindView()
    {
        var clock = Stopwatch.StartNew();
        var panels = UIElementsRuntimeUtility.GetSortedPlayerPanels();
        for (int i = panels.Count - 1; i >= 0 && i >= panels.Count - 32; i--)
        {
            var root = panels[i].visualTree;
            var table = Walk(root, clock).FirstOrDefault(e => e.name == "playertable" && e.worldBound.width > 0);
            if (table is null) continue;
            // El total se coteja aparte en pantalla. Se comprueba también el rótulo de esta instancia de la vista.
            var label = Walk(root, clock, e => e.Pointer == table.Pointer)
                .Select(e => e.TryCast<TextElement>()).FirstOrDefault(t => MatchesCount(t?.text));
            if (label is null) throw new InvalidDataException("No se pudo comprobar el total anunciado por FM27.");
            var header = Walk(table, clock, IsRow).FirstOrDefault(e => e.name == "column-headers");
            if (header is null) throw new InvalidDataException("Cabecera de búsqueda ausente.");
            string[] names = Enumerable.Range(0, header.childCount).Select(index =>
                Walk(header.ElementAt(index), clock).FirstOrDefault(e => e.ClassListContains("table-header-xs"))?
                    .TryCast<TextElement>()?.text?.Trim() ?? "").ToArray();
            int id = Array.IndexOf(names, "Unique ID");
            int pass = Array.IndexOf(names, "Pase");
            if (id < 0 || pass < 0 || Array.LastIndexOf(names, "Unique ID") != id || Array.LastIndexOf(names, "Pase") != pass
                || !names.Contains("Club") || !names.Contains("Posición"))
                throw new InvalidDataException("La vista de ojeo requiere Unique ID, Pase, Club y Posición.");
            var firstRow = Walk(table, clock).FirstOrDefault(IsRow);
            for (var parent = firstRow?.parent; parent is not null && parent.Pointer != table.Pointer; parent = parent.parent)
            {
                var scroll = parent.TryCast<ScrollView>();
                if (scroll is not null) return new(table, scroll, label, label.text, names.Length, id, pass);
            }
            throw new InvalidDataException("No se encontró la lista de filas virtualizadas.");
        }
        throw new InvalidDataException("No hay una tabla de ojeados activa.");
    }

    private bool MatchesCount(string? raw)
    {
        if (raw is null) return false;
        // Algunas vistas usan una etiqueta única y otras un contador separado.
        string text = System.Text.RegularExpressions.Regex.Replace(raw, "<[^>]+>", "").Trim();
        var number = expectedCount.ToString(CultureInfo.InvariantCulture);
        return text == number || text.StartsWith(number + " Jugadores Encontrados", StringComparison.OrdinalIgnoreCase);
    }

    private static bool Shown(VisualElement e) => e.visible && e.resolvedStyle.display != DisplayStyle.None;
    private static bool IsRow(VisualElement e) => e.ClassListContains("streamed-table__row");
    private static bool IsCell(VisualElement e) => e.ClassListContains("streamed-table__cell");

    private static string Text(VisualElement cell, Stopwatch clock) => string.Join(" ", Walk(cell, clock)
        .Select(e => e.TryCast<TextElement>()?.text?.Trim()).Where(value => !string.IsNullOrEmpty(value)));

    private static IEnumerable<VisualElement> Walk(VisualElement root, Stopwatch clock, Func<VisualElement, bool>? stop = null)
    {
        var pending = new Stack<VisualElement>();
        pending.Push(root);
        while (pending.TryPop(out var element))
        {
            if (clock.ElapsedMilliseconds > 250) throw new InvalidDataException("Límite de lectura de una página.");
            if (!Shown(element)) continue;
            yield return element;
            if (stop?.Invoke(element) == true) continue;
            for (int child = element.childCount - 1; child >= 0; child--) pending.Push(element.ElementAt(child));
        }
    }

    private sealed record View(VisualElement Table, ScrollView Scroll, TextElement CountLabel, string OriginalLabel,
        int Columns, int IdColumn, int PassColumn);
}
