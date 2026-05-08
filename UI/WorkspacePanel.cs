using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AlchemyGame.Models;
using AlchemyGame.Services;
using System.Drawing.Drawing2D;

namespace AlchemyGame.UI;

public class WorkspacePanel : Panel
{
    private readonly Game _game;

    private readonly Dictionary<int, ElementControl> _controls = new();

    public event Action<string, bool>? CraftHappened;

    public event Action? NewElementUnlocked;

    public WorkspacePanel(Game game)
    {
        _game          = game;
        DoubleBuffered = true;
        BackColor      = Color.FromArgb(248, 248, 244);

        Paint += DrawDotGrid;
    }


    public void SpawnElement(string elementId)
    {
        var rng = new Random();
        var pos = new Point(
            rng.Next(20, Math.Max(40, Width  - 110)),
            rng.Next(20, Math.Max(40, Height - 110)));

        var ws   = _game.Spawn(elementId, pos);
        var ctrl = CreateControl(ws);
        ctrl.BringToFront();

        SoundService.PlaySpawn();
    }

    public void ClearAll()
    {
        foreach (var ctrl in _controls.Values)
            ctrl.Dispose();
        _controls.Clear();
        _game.ClearWorkspace();
    }


    private ElementControl CreateControl(WorkspaceElement ws)
    {
        var ctrl = new ElementControl(ws);
        ctrl.DragEnded += OnDragEnded;
        _controls[ws.InstanceId] = ctrl;
        Controls.Add(ctrl);
        return ctrl;
    }

    private void DestroyControl(ElementControl ctrl)
    {
        _game.Remove(ctrl.WorkspaceElement);
        _controls.Remove(ctrl.WorkspaceElement.InstanceId);
        Controls.Remove(ctrl);
        ctrl.Dispose();
    }


    private void OnDragEnded(ElementControl dragged)
    {
        foreach (var c in _controls.Values)
            c.SetHighlight(false);

        foreach (var other in _controls.Values.ToList())
        {
            if (other == dragged) continue;
            if (!dragged.Bounds.IntersectsWith(other.Bounds)) continue;

            var craftResult = _game.TryCraft(
                dragged.WorkspaceElement,
                other.WorkspaceElement);

            if (craftResult is null) continue;

            var (result, isNew, spawnAt) = craftResult.Value;

            DestroyControl(dragged);
            DestroyControl(other);

            BeginInvoke(() =>
            {
                var newWs   = _game.Spawn(result.Id, spawnAt);
                var newCtrl = CreateControl(newWs);
                newCtrl.BringToFront();

                if (isNew)
                {
                    SoundService.PlayNewDiscovery();
                    NewElementUnlocked?.Invoke();
                }
                else
                {
                    SoundService.PlayCraft();
                }

                CraftHappened?.Invoke(result.Name, isNew);
            });

            return; 
        }
    }



    private void DrawDotGrid(object? sender, PaintEventArgs e)
    {
        using var dot = new SolidBrush(Color.FromArgb(50, 180, 180, 170));
        for (int x = 14; x < Width;  x += 28)
        for (int y = 14; y < Height; y += 28)
            e.Graphics.FillEllipse(dot, x - 1, y - 1, 2, 2);
    }
}
