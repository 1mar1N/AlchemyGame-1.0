global using System;
global using System.Collections.Generic;
global using System.Drawing;
global using System.Linq;
global using System.Windows.Forms;
using AlchemyGame.Data;
using AlchemyGame.Models;
using AlchemyGame.Services;

namespace AlchemyGame.Services;


public class Game
{
    private readonly CraftService _craftService = new();

    public HashSet<string> UnlockedIds { get; } = new(ElementData.StarterElements);

    public List<WorkspaceElement> WorkspaceElements { get; } = new();

    public int TotalElements => ElementData.Elements.Count;
    public int DiscoveredCount => UnlockedIds.Count;


    public IEnumerable<Element> GetUnlockedElements()
        => UnlockedIds
            .Where(id => ElementData.Elements.ContainsKey(id))
            .Select(id => ElementData.Elements[id]);


    public WorkspaceElement SpawnElement(string elementId, Point position)
    {
        var element = ElementData.Elements[elementId];
        var wsElem  = new WorkspaceElement(element, position);
        WorkspaceElements.Add(wsElem);
        return wsElem;
    }

    public void RemoveWorkspaceElement(WorkspaceElement wsElem)
        => WorkspaceElements.Remove(wsElem);

    public void MoveElement(WorkspaceElement wsElem, Point newPosition)
        => wsElem.Position = newPosition;


    public (Element Result, bool IsNew, Point SpawnPosition)? TryCraft(
        WorkspaceElement a, WorkspaceElement b)
    {
        var result = _craftService.TryCraft(a.Element.Id, b.Element.Id);
        if (result is null) return null;

        var spawnPos = new Point(
            (a.Position.X + b.Position.X) / 2,
            (a.Position.Y + b.Position.Y) / 2);

        bool isNew = !UnlockedIds.Contains(result.Id);

        RemoveWorkspaceElement(a);
        RemoveWorkspaceElement(b);

        UnlockedIds.Add(result.Id);

        return (result, isNew, spawnPos);
    }


    public void ResetWorkspace()
        => WorkspaceElements.Clear();
}
