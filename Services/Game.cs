using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AlchemyGame.Data;
using AlchemyGame.Models;

namespace AlchemyGame.Services;

public class Game
{
    private readonly CraftService _craftService = new();

    // ── Состояние ─────────────────────────────────────────────────────────

    /// <summary>Id элементов, которые игрок уже открыл (инвентарь)</summary>
    public HashSet<string> UnlockedIds { get; } = new(ElementData.StarterElements);

    /// <summary>Id последнего открытого элемента (для MainForm)</summary>
    public string LastUnlockedId { get; private set; } = "";

    /// <summary>Элементы, размещённые на рабочей области</summary>
    public List<WorkspaceElement> WorkspaceElements { get; } = new();

    // ── Статистика ────────────────────────────────────────────────────────

    public int TotalElements     => ElementData.Elements.Count;
    public int DiscoveredCount   => UnlockedIds.Count;

    // ── Инвентарь ─────────────────────────────────────────────────────────

    /// <summary>Все открытые элементы в порядке добавления</summary>
    public IEnumerable<Element> GetUnlocked()
        => UnlockedIds
            .Where(id => ElementData.Elements.ContainsKey(id))
            .Select(id => ElementData.Elements[id]);

    // ── Рабочая область ───────────────────────────────────────────────────

    /// <summary>Создаёт новый экземпляр элемента на поле и добавляет в список</summary>
    public WorkspaceElement Spawn(string elementId, Point position)
    {
        var element = ElementData.Elements[elementId];
        var ws = new WorkspaceElement(element, position);
        WorkspaceElements.Add(ws);
        return ws;
    }

    /// <summary>Удаляет экземпляр с рабочей области (не из инвентаря)</summary>
    public void Remove(WorkspaceElement ws)
        => WorkspaceElements.Remove(ws);

    /// <summary>Удаляет все элементы с рабочей области</summary>
    public void ClearWorkspace()
        => WorkspaceElements.Clear();

    // ── Крафт ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Пытается скрафтить два элемента.
    ///
    /// При успехе:
    ///   - удаляет оба элемента с рабочей области
    ///   - добавляет результат в UnlockedIds
    ///   - возвращает (результат, это_первое_открытие, позиция_спавна)
    ///
    /// При неудаче: возвращает null.
    /// </summary>
    public (Element Result, bool IsNewDiscovery, Point SpawnAt)?
        TryCraft(WorkspaceElement a, WorkspaceElement b)
    {
        var result = _craftService.TryCraft(a.Element.Id, b.Element.Id);
        if (result is null) return null;

        bool isNew = !UnlockedIds.Contains(result.Id);

        // Позиция между двумя элементами
        var spawnAt = new Point(
            (a.Position.X + b.Position.X) / 2,
            (a.Position.Y + b.Position.Y) / 2);

        Remove(a);
        Remove(b);
        UnlockedIds.Add(result.Id);
        if (isNew) LastUnlockedId = result.Id;

        return (result, isNew, spawnAt);
    }
}
// NOTE: LastUnlockedId is set inside TryCraft when isNew==true
