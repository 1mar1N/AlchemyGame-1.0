global using System;
global using System.Collections.Generic;
global using System.Drawing;
global using System.Linq;
global using System.Windows.Forms;
using AlchemyGame.Services;

namespace AlchemyGame.UI;


public class MainForm : Form
{
    private readonly Game           _game;
    private readonly WorkspacePanel _workspace;
    private readonly InventoryPanel _inventory;
    private readonly Label          _statusLabel;
    private readonly System.Windows.Forms.Timer _toastTimer;

    public MainForm()
    {
        _game = new Game();

        Text            = "✦ Алхимия";
        Size            = new Size(900, 640);
        MinimumSize     = new Size(640, 480);
        StartPosition   = FormStartPosition.CenterScreen;
        BackColor       = Color.FromArgb(244, 244, 240);
        DoubleBuffered  = true;

        var header = new Panel
        {
            Dock      = DockStyle.Top,
            Height    = 44,
            BackColor = Color.White,
        };

        var titleLabel = new Label
        {
            Text      = "✦  Алхимия",
            Font      = new Font("Segoe UI", 13f, FontStyle.Regular),
            ForeColor = Color.FromArgb(30, 30, 28),
            Location  = new Point(16, 10),
            AutoSize  = true,
        };

        _statusLabel = new Label
        {
            Text      = DiscoveredText(),
            Font      = new Font("Segoe UI", 9f),
            ForeColor = Color.FromArgb(150, 150, 145),
            Anchor    = AnchorStyles.Right | AnchorStyles.Top,
            AutoSize  = true,
        };
        _statusLabel.Location = new Point(header.Width - _statusLabel.Width - 16, 13);

        var headerLine = new Panel
        {
            Dock      = DockStyle.Bottom,
            Height    = 1,
            BackColor = Color.FromArgb(224, 224, 218),
        };

        header.Controls.Add(titleLabel);
        header.Controls.Add(_statusLabel);
        header.Controls.Add(headerLine);
        header.Resize += (_, _) =>
            _statusLabel.Location = new Point(header.Width - _statusLabel.Width - 16, 13);

        var clearBtn = new Button
        {
            Text      = "Очистить поле",
            Font      = new Font("Segoe UI", 8.5f),
            FlatStyle = FlatStyle.Flat,
            Cursor    = Cursors.Hand,
            Size      = new Size(110, 26),
            Location  = new Point(header.Width - 240, 9),
            Anchor    = AnchorStyles.Right | AnchorStyles.Top,
        };
        clearBtn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 195);
        clearBtn.ForeColor = Color.FromArgb(80, 80, 78);
        clearBtn.Click += (_, _) => { _game.ResetWorkspace(); _workspace.Controls.Clear(); };
        header.Controls.Add(clearBtn);

        _inventory = new InventoryPanel(_game);
        _inventory.ElementClicked += id => _workspace.SpawnElement(id);

        _workspace = new WorkspacePanel(_game)
        {
            Dock = DockStyle.Fill,
        };
        _workspace.ElementCrafted  += OnElementCrafted;
        _workspace.InventoryChanged += () =>
        {
            _inventory.Refresh();
            _statusLabel.Text = DiscoveredText();
        };

        var hint = new Label
        {
            Text      = "Нажмите на элемент в инвентаре, чтобы добавить его на поле\n" +
                        "Перетащите один элемент на другой — и получите новый!",
            Font      = new Font("Segoe UI", 10f),
            ForeColor = Color.FromArgb(180, 180, 175),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock      = DockStyle.Fill,
        };
        _workspace.Controls.Add(hint);
        _inventory.ElementClicked += _ => hint.Visible = false;

        _toastTimer = new System.Windows.Forms.Timer { Interval = 2200 };
        _toastTimer.Tick += (_, _) => { _toastPanel.Visible = false; _toastTimer.Stop(); };

        _toastPanel = CreateToastPanel();
        _workspace.Controls.Add(_toastPanel);
        _toastPanel.BringToFront();

        Controls.Add(_workspace);
        Controls.Add(_inventory);
        Controls.Add(header);
    }


    private readonly Panel _toastPanel;
    private Label?         _toastLabel;

    private Panel CreateToastPanel()
    {
        var p = new Panel
        {
            Height    = 32,
            Width     = 280,
            BackColor = Color.FromArgb(15, 110, 86),
            Visible   = false,
        };
        _toastLabel = new Label
        {
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(237, 250, 244),
            Font      = new Font("Segoe UI Emoji", 9.5f),
            BackColor = Color.Transparent,
        };
        p.Controls.Add(_toastLabel);
        return p;
    }

    private void ShowToast(string message)
    {
        if (_toastLabel is null) return;
        _toastLabel.Text = message;
        _toastPanel.Width    = Math.Min(360, message.Length * 11 + 40);
        _toastPanel.Location = new Point(
            (_workspace.Width  - _toastPanel.Width)  / 2,
            (_workspace.Height - _toastPanel.Height) / 2 - 60);
        _toastPanel.Visible = true;
        _toastPanel.BringToFront();
        _toastTimer.Stop();
        _toastTimer.Start();
    }

    private void OnElementCrafted(string resultName, bool isNew)
    {
        string msg = isNew
            ? $"✦ Открыт новый элемент: {resultName}!"
            : $"Получено: {resultName}";
        ShowToast(msg);
    }

    private string DiscoveredText()
        => $"Открыто: {_game.DiscoveredCount} / {_game.TotalElements}";
}
