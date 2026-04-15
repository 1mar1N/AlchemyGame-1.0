global using System;
global using System.Collections.Generic;
global using System.Drawing;
global using System.Linq;
global using System.Windows.Forms;
using AlchemyGame.Models;
using AlchemyGame.Services;

namespace AlchemyGame.UI;


public class WorkspacePanel : Panel
{
    private readonly Game _game;
    private readonly Dictionary<int, ElementControl> _controls = new();

    public event Action<string, bool>? ElementCrafted; 
    public event Action? InventoryChanged;

    public WorkspacePanel(Game game)
    {
        _game = game;

        BackColor   = Color.FromArgb(248, 248, 244);
        DoubleBuffered = true;
        AllowDrop   = true;

        Paint += OnPaint;
    }


    public void SpawnElement(string elementId)
    {
        var rand = new Random();
        var pos  = new Point(
            rand.Next(40, Math.Max(50, Width  - 120)),
            rand.Next(20, Math.Max(50, Height - 120)));

        var wsElem = _game.SpawnElement(elementId, pos);
        AddControl(wsElem);
    }

    private ElementControl AddControl(WorkspaceElement wsElem)
    {
        var ctrl = new ElementControl(wsElem);
        ctrl.DragEnded += OnElementDragEnded;
        _controls[wsElem.InstanceId] = ctrl;
        Controls.Add(ctrl);
        ctrl.BringToFront();
        return ctrl;
    }


    private void OnElementDragEnded(object? s, ElementControl dragged)
    {
        foreach (var c in _controls.Values)
            c.SetHighlight(false);

        foreach (var other in _controls.Values)
        {
            if (other == dragged) continue;
            if (!dragged.Bounds.IntersectsWith(other.Bounds)) continue;

            var craftResult = _game.TryCraft(
                dragged.WorkspaceElement,
                other.WorkspaceElement);

            if (craftResult is null) continue; 

            var (result, isNew, spawnPos) = craftResult.Value;

            RemoveControl(dragged);
            RemoveControl(other);

            var newWs = _game.SpawnElement(result.Id, spawnPos);
            var newCtrl = AddControl(newWs);

            ElementCrafted?.Invoke(result.Name, isNew);
            if (isNew) InventoryChanged?.Invoke();

            return;
        }
    }

    private void RemoveControl(ElementControl ctrl)
    {
        _game.RemoveWorkspaceElement(ctrl.WorkspaceElement);
        _controls.Remove(ctrl.WorkspaceElement.InstanceId);
        Controls.Remove(ctrl);
        ctrl.Dispose();
    }


    private void OnPaint(object? s, PaintEventArgs e)
    {
        var g    = e.Graphics;
        var dotC = Color.FromArgb(60, 200, 200, 192);
        using var brush = new SolidBrush(dotC);
        for (int x = 14; x < Width;  x += 28)
        for (int y = 14; y < Height; y += 28)
            g.FillEllipse(brush, x - 1, y - 1, 2, 2);
    }
}
