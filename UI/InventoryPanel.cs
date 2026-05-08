using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AlchemyGame.Models;
using AlchemyGame.Services;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace AlchemyGame.UI;

public class InventoryPanel : Panel
{
    private readonly Game            _game;
    private readonly FlowLayoutPanel _flow;
    private readonly HashSet<string> _newIds = new(); 

    public event Action<string>? ElementClicked;

    public InventoryPanel(Game game)
    {
        _game     = game;
        Height    = 118;
        Dock      = DockStyle.Bottom;
        BackColor = Color.White;

        var topLine = new Panel
        {
            Dock      = DockStyle.Top,
            Height    = 1,
            BackColor = Color.FromArgb(218, 218, 212),
        };

        var titlePanel = new Panel
        {
            Dock      = DockStyle.Top,
            Height    = 22,
            BackColor = Color.Transparent,
        };
        titlePanel.Controls.Add(new Label
        {
            Text      = "ИНВЕНТАРЬ  —  нажмите на элемент, чтобы добавить на поле",
            Font      = new Font("Segoe UI", 7.5f),
            ForeColor = Color.FromArgb(155, 155, 150),
            Location  = new Point(14, 5),
            AutoSize  = true,
        });

        _flow = new FlowLayoutPanel
        {
            Dock          = DockStyle.Fill,
            AutoScroll    = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents  = false,
            Padding       = new Padding(6, 2, 6, 4),
            BackColor     = Color.Transparent,
        };
        _flow.HorizontalScroll.Visible = false;

        Controls.Add(_flow);
        Controls.Add(titlePanel);
        Controls.Add(topLine);

        Refresh();
    }
    class CardPanel : Panel
    {
        public CardPanel()
        {
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor,
                true);
        }
    }
    public new void Refresh()
    {
        _flow.SuspendLayout();

        foreach (Control c in _flow.Controls)
            c.Dispose();
        _flow.Controls.Clear();

        foreach (var el in _game.GetUnlocked())
            _flow.Controls.Add(MakeCard(el));

        _flow.ResumeLayout();
    }

    public void MarkNew(string elementId) => _newIds.Add(elementId);


    private Control MakeCard(Element el)
    {
        bool isNew = _newIds.Contains(el.Id);

        var card = new CardPanel()
        {
            Width     = 70,
            Height    = 80,
            Margin    = new Padding(3, 3, 3, 0),
            Cursor    = Cursors.Hand,
            Tag       = el.Id,
        };
    

        bool hovered = false;
        var baseColor = ParseHex(el.Color);
        var lightColor = Lighten(baseColor, 0.88f);

        card.Paint += (_, pe) => DrawCard(pe.Graphics, card, el, baseColor, lightColor, hovered, isNew);
        card.MouseEnter += (_, _) => { hovered = true;  card.Invalidate(); };
        card.MouseLeave += (_, _) => { hovered = false; card.Invalidate(); };
        card.Click      += (_, _) =>
        {
            _newIds.Remove(el.Id);
            ElementClicked?.Invoke(el.Id);
        };

        if (isNew)
        {
            var t = new System.Windows.Forms.Timer { Interval = 5000 };
            t.Tick += (_, _) =>
            {
                _newIds.Remove(el.Id);
                card.Invalidate();
                t.Stop();
                t.Dispose();
            };
            t.Start();
        }

        return card;
    }

    private static void DrawCard(Graphics g, Panel card,
        Element el, Color baseColor, Color lightColor, bool hovered, bool isNew)
    {
        g.SmoothingMode     = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAlias;

        var rect = new Rectangle(1, 1, card.Width - 2, card.Height - 4);

        using var bgPath = RoundRect(rect, 10);
        Color bg = hovered ? Color.White : Color.FromArgb(248, 248, 244);
        using var bgBrush = new SolidBrush(bg);
        g.FillPath(bgBrush, bgPath);

        Color borderColor = isNew
            ? Color.FromArgb(29, 158, 117)
            : Color.FromArgb(hovered ? 60 : 30, 0, 0, 0);
        using var borderPen = new Pen(borderColor, isNew ? 1.5f : 1f);
        g.DrawPath(borderPen, bgPath);

        using var dot = new SolidBrush(Color.FromArgb(160, baseColor));
        g.FillEllipse(dot, rect.Right - 12, rect.Top + 5, 7, 7);

        using var emojiFont = new Font("Segoe UI Emoji", 20f);
        var emojiSz = g.MeasureString(el.Emoji, emojiFont);
        g.DrawString(el.Emoji, emojiFont, Brushes.Black,
            (card.Width - emojiSz.Width) / 2f, 8f);

        using var nameFont  = new Font("Segoe UI", 7f, FontStyle.Bold);
        using var nameBrush = new SolidBrush(Color.FromArgb(70, 50, 50));
        var nameSz = g.MeasureString(el.Name, nameFont);
        g.DrawString(el.Name, nameFont, nameBrush,
            (card.Width - nameSz.Width) / 2f, card.Height - 20f);

        if (isNew)
        {
            using var badgeBrush = new SolidBrush(Color.FromArgb(29, 158, 117));
            using var badgeFont  = new Font("Segoe UI", 6.5f, FontStyle.Bold);
            const string badge   = "NEW!";
            var bsz = g.MeasureString(badge, badgeFont);
            float bx = (card.Width - bsz.Width) / 2f;
            float by = card.Height - 32f;
            g.FillRectangle(badgeBrush, bx - 2, by - 1, bsz.Width + 4, bsz.Height + 2);
            g.DrawString(badge, badgeFont, Brushes.White, bx, by);
        }
    }


    private static GraphicsPath RoundRect(Rectangle r, int d)
    {
        var p = new GraphicsPath();
        p.AddArc(r.X, r.Y, d * 2, d * 2, 180, 90);
        p.AddArc(r.Right - d * 2, r.Y, d * 2, d * 2, 270, 90);
        p.AddArc(r.Right - d * 2, r.Bottom - d * 2, d * 2, d * 2, 0, 90);
        p.AddArc(r.X, r.Bottom - d * 2, d * 2, d * 2, 90, 90);
        p.CloseFigure();
        return p;
    }

    private static Color ParseHex(string h)
    {
        try { return ColorTranslator.FromHtml(h); }
        catch { return Color.Gray; }
    }

    private static Color Lighten(Color c, float f)
        => Color.FromArgb(
            (int)Math.Min(255, c.R + (255 - c.R) * f),
            (int)Math.Min(255, c.G + (255 - c.G) * f),
            (int)Math.Min(255, c.B + (255 - c.B) * f));
}
