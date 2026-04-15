global using System;
global using System.Collections.Generic;
global using System.Drawing;
global using System.Linq;
global using System.Windows.Forms;
using AlchemyGame.Models;
using AlchemyGame.Services;

namespace AlchemyGame.UI;


public class InventoryPanel : Panel
{
    private readonly Game           _game;
    private readonly FlowLayoutPanel _flow;

    public event Action<string>? ElementClicked; 

    public InventoryPanel(Game game)
    {
        _game = game;

        Height    = 110;
        Dock      = DockStyle.Bottom;
        BackColor = Color.White;

        
        var title = new Label
        {
            Text      = "ИНВЕНТАРЬ  —  нажмите на элемент, чтобы добавить на поле",
            Font      = new Font("Segoe UI", 8f),
            ForeColor = Color.FromArgb(160, 160, 155),
            Location  = new Point(14, 6),
            AutoSize  = true,
        };

        var line = new Panel
        {
            Height    = 1,
            Dock      = DockStyle.Top,
            BackColor = Color.FromArgb(224, 224, 218),
        };

        _flow = new FlowLayoutPanel
        {
            Location       = new Point(8, 24),
            Size           = new Size(ClientSize.Width - 16, 80),
            Anchor         = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            AutoScroll     = true,
            FlowDirection  = FlowDirection.LeftToRight,
            WrapContents   = false,
            BackColor      = Color.Transparent,
        };

        Controls.Add(line);
        Controls.Add(title);
        Controls.Add(_flow);

        Refresh();
    }

    public new void Refresh()
    {
        _flow.SuspendLayout();
        _flow.Controls.Clear();

        foreach (var el in _game.GetUnlockedElements())
            _flow.Controls.Add(CreateItemButton(el));

        _flow.ResumeLayout();
    }

    private Button CreateItemButton(Element el)
    {
        var btn = new Button
        {
            Width     = 66,
            Height    = 70,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(248, 248, 244),
            Margin    = new Padding(3),
            Cursor    = Cursors.Hand,
            Tag       = el.Id,
        };
        btn.FlatAppearance.BorderColor = Color.FromArgb(232, 232, 226);
        btn.FlatAppearance.BorderSize  = 1;
        btn.FlatAppearance.MouseOverBackColor  = Color.White;
        btn.FlatAppearance.MouseDownBackColor  = Color.FromArgb(237, 250, 244);

        btn.Text      = $"{el.Emoji}\n{el.Name}";
        btn.Font      = new Font("Segoe UI Emoji", 8f);
        btn.TextAlign = ContentAlignment.MiddleCenter;
        btn.UseCompatibleTextRendering = true;

        btn.Click += (_, _) => ElementClicked?.Invoke(el.Id);

        return btn;
    }
}
