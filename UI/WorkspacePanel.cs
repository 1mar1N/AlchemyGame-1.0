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

    // Словарь InstanceId → ElementControl для быстрого поиска
    private readonly Dictionary<int, ElementControl> _controls = new();

    // ── События для MainForm ──────────────────────────────────────────────
    /// <summary>Вызывается при любом успешном крафте: (имя результата, это_открытие?)</summary>
    public event Action<string, bool>? CraftHappened;

    /// <summary>Вызывается только при открытии нового элемента (для обновления инвентаря)</summary>
    public event Action? NewElementUnlocked;

    public WorkspacePanel(Game game)
    {
        _game          = game;
        DoubleBuffered = true;
        BackColor      = Color.FromArgb(248, 248, 244);

        Paint += DrawDotGrid;
    }

    // ── Публичные методы ─────────────────────────────────────────────────

    /// <summary>Создаёт карточку элемента в случайном месте поля</summary>
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

    /// <summary>Удаляет все карточки с поля</summary>
    public void ClearAll()
    {
        foreach (var ctrl in _controls.Values)
            ctrl.Dispose();
        _controls.Clear();
        _game.ClearWorkspace();
    }

    // ── Создание и удаление карточек ────────────────────────────────────

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

    // ── Обработка окончания перетаскивания ───────────────────────────────

    private void OnDragEnded(ElementControl dragged)
    {
        // Снимаем подсветку со всех карточек
        foreach (var c in _controls.Values)
            c.SetHighlight(false);

        // Ищем первую карточку, с которой есть пересечение
        foreach (var other in _controls.Values.ToList())
        {
            if (other == dragged) continue;
            if (!dragged.Bounds.IntersectsWith(other.Bounds)) continue;

            // Пробуем скрафтить
            var craftResult = _game.TryCraft(
                dragged.WorkspaceElement,
                other.WorkspaceElement);

            if (craftResult is null) continue; // рецепта нет, пропускаем

            var (result, isNew, spawnAt) = craftResult.Value;

            // Удаляем оба контрола
            DestroyControl(dragged);
            DestroyControl(other);

            // Небольшая задержка для визуальной чёткости
            BeginInvoke(() =>
            {
                // Спавним результат
                var newWs   = _game.Spawn(result.Id, spawnAt);
                var newCtrl = CreateControl(newWs);
                newCtrl.BringToFront();

                // Звук и оповещения
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

            return; // обрабатываем только первое пересечение
        }
    }

    // ── Подсветка при перетаскивании (вызывается из ElementControl) ──────
    // (Реализовано через DragEnded — каждый кадр мы не обновляем подсветку,
    //  только при завершении. При желании можно добавить MouseMove на WorkspacePanel.)

    // ── Отрисовка точечного фона ─────────────────────────────────────────

    private void DrawDotGrid(object? sender, PaintEventArgs e)
    {
        using var dot = new SolidBrush(Color.FromArgb(50, 180, 180, 170));
        for (int x = 14; x < Width;  x += 28)
        for (int y = 14; y < Height; y += 28)
            e.Graphics.FillEllipse(dot, x - 1, y - 1, 2, 2);
    }
}
