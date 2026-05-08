using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AlchemyGame.Services;

namespace AlchemyGame.UI;

public class MainForm : Form
{
    private readonly Game           _game;
    private readonly WorkspacePanel _workspace;
    private readonly InventoryPanel _inventory;
    private readonly Label          _discLabel;
    private readonly Panel          _toast;
    private readonly Label          _toastLabel;
    private readonly System.Windows.Forms.Timer _toastTimer;

    public MainForm()
    {
        _game = new Game();

        Text           = "✦ Алхимия";
        Size           = new Size(1000, 680);
        MinimumSize    = new Size(700, 500);
        StartPosition  = FormStartPosition.CenterScreen;
        BackColor      = Color.FromArgb(244, 244, 240);
        DoubleBuffered = true;
        Icon           = SystemIcons.Application;

        var header = new Panel
        {
            Dock      = DockStyle.Top,
            Height    = 46,
            BackColor = Color.White,
        };

        var headerLine = new Panel
        {
            Dock      = DockStyle.Bottom,
            Height    = 1,
            BackColor = Color.FromArgb(220, 220, 215),
        };
        header.Controls.Add(headerLine);

        header.Controls.Add(new Label
        {
            Text      = "✦  Алхимия",
            Font      = new Font("Segoe UI", 14f),
            ForeColor = Color.FromArgb(28, 28, 26),
            Location  = new Point(16, 10),
            AutoSize  = true,
        });

        _discLabel = new Label
        {
            Text      = DiscText(),
            Font      = new Font("Segoe UI", 9f),
            ForeColor = Color.FromArgb(150, 150, 145),
            AutoSize  = true,
        };
        header.Controls.Add(_discLabel);

        var btnClear = MakeHeaderButton("Очистить поле");
        btnClear.Click += (_, _) => _workspace.ClearAll();

        var btnSound = MakeHeaderButton("🔊 Звук: вкл");
        btnSound.Click += (_, _) =>
        {
            SoundService.Enabled = !SoundService.Enabled;
            btnSound.Text = SoundService.Enabled ? "🔊 Звук: вкл" : "🔇 Звук: выкл";
        };

        header.Controls.Add(btnClear);
        header.Controls.Add(btnSound);

        header.Resize += (_, _) =>
        {
            btnClear.Location  = new Point(header.Width - 130, 10);
            btnSound.Location  = new Point(header.Width - 260, 10);
            _discLabel.Location = new Point(header.Width / 2 - _discLabel.Width / 2, 14);
        };

        _inventory = new InventoryPanel(_game);
        _inventory.ElementClicked += id => _workspace.SpawnElement(id);

        _workspace = new WorkspacePanel(_game) { Dock = DockStyle.Fill };
        _workspace.CraftHappened      += OnCraftHappened;
        _workspace.NewElementUnlocked += OnNewElementUnlocked;

        var hint = new Label
        {
            Text      = "Нажмите на элемент в инвентаре снизу, чтобы добавить его на поле\n" +
                        "Перетащите один элемент на другой — и получите новый!",
            Font      = new Font("Segoe UI", 11f),
            ForeColor = Color.FromArgb(185, 185, 180),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock      = DockStyle.Fill,
            BackColor = Color.Transparent,
        };
        _workspace.Controls.Add(hint);
        _inventory.ElementClicked += _ => hint.Visible = false;

        _toast = new Panel
        {
            Size      = new Size(300, 34),
            BackColor = Color.FromArgb(25, 120, 95),
            Visible   = false,
        };
        _toastLabel = new Label
        {
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(230, 250, 244),
            Font      = new Font("Segoe UI Emoji", 9.5f, FontStyle.Bold),
            BackColor = Color.Transparent,
        };
        _toast.Controls.Add(_toastLabel);
        _workspace.Controls.Add(_toast);

        _toastTimer = new System.Windows.Forms.Timer { Interval = 2400 };
        _toastTimer.Tick += (_, _) =>
        {
            _toast.Visible = false;
            _toastTimer.Stop();
        };

        _workspace.Resize += (_, _) => PositionToast();

        Controls.Add(_workspace);
        Controls.Add(_inventory);
        Controls.Add(header);
    }


    private void OnCraftHappened(string resultName, bool isNew)
    {
        if (isNew)
            ShowToast($"✦ Новый элемент: {resultName}!");
        else
            ShowToast($"Получено: {resultName}");
    }

    private void OnNewElementUnlocked()
    {
        _inventory.MarkNew(_game.LastUnlockedId);
        _inventory.Refresh();
        _discLabel.Text = DiscText();
        PositionToast(); 
    }


    private void ShowToast(string text)
    {
        _toastLabel.Text  = text;
        _toast.Width      = Math.Min(420, text.Length * 10 + 50);
        _toast.Visible    = true;
        _toast.BringToFront();
        PositionToast();
        _toastTimer.Stop();
        _toastTimer.Start();
    }

    private void PositionToast()
    {
        _toast.Location = new Point(
            (_workspace.Width  - _toast.Width)  / 2,
            (_workspace.Height - _toast.Height) / 2 - 50);
    }


    private string DiscText() =>
        $"Открыто: {_game.DiscoveredCount} / {_game.TotalElements}";

    private static Button MakeHeaderButton(string text)
    {
        var btn = new Button
        {
            Text      = text,
            Font      = new Font("Segoe UI", 8.5f),
            FlatStyle = FlatStyle.Flat,
            Cursor    = Cursors.Hand,
            Size      = new Size(118, 26),
            Location  = new Point(0, 10),
        };
        btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 195);
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 240);
        btn.ForeColor = Color.FromArgb(80, 80, 78);
        return btn;
    }
}
