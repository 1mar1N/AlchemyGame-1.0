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


    public HashSet<string> UnlockedIds { get; } = new(ElementData.StarterElements);

    public string LastUnlockedId { get; private set; } = "";

    public List<WorkspaceElement> WorkspaceElements { get; } = new();


    public int TotalElements     => ElementData.Elements.Count;
    public int DiscoveredCount   => UnlockedIds.Count;


    public IEnumerable<Element> GetUnlocked()
        => UnlockedIds
            .Where(id => ElementData.Elements.ContainsKey(id))
            .Select(id => ElementData.Elements[id]);


    public WorkspaceElement Spawn(string elementId, Point position)
    {
        var element = ElementData.Elements[elementId];
        var ws = new WorkspaceElement(element, position);
        WorkspaceElements.Add(ws);
        return ws;
    }

    public void Remove(WorkspaceElement ws)
        => WorkspaceElements.Remove(ws);

    public void ClearWorkspace()
        => WorkspaceElements.Clear();

 
    public (Element Result, bool IsNewDiscovery, Point SpawnAt)?
        TryCraft(WorkspaceElement a, WorkspaceElement b)
    {
        var result = _craftService.TryCraft(a.Element.Id, b.Element.Id);
        if (result is null) return null;

        bool isNew = !UnlockedIds.Contains(result.Id);

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
