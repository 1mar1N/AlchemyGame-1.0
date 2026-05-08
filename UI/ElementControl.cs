using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AlchemyGame.Models;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace AlchemyGame.UI;

public class ElementControl : Panel
{
    public WorkspaceElement WorkspaceElement { get; }

    private bool  _dragging;
    private Point _dragOffset;

    private bool _highlighted;

    private static readonly Font _emojiFont = new("Segoe UI Emoji", 22f);
    private static readonly Font _nameFont  = new("Segoe UI", 7.5f, FontStyle.Bold);

    public event Action<ElementControl>? DragEnded;

    public ElementControl(WorkspaceElement ws)
    {
        WorkspaceElement = ws;
        Size      = new Size(80, 80);
        Location  = ws.Position;
        BackColor = Color.Transparent;
        Cursor    = Cursors.Hand;

        SetStyle(
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint  |
            ControlStyles.UserPaint             |
            ControlStyles.SupportsTransparentBackColor,
            true);

        MouseDown += OnMouseDown;
        MouseMove += OnMouseMove;
        MouseUp   += OnMouseUp;
    }

    public void SetHighlight(bool on)
    {
        if (_highlighted == on) return;
        _highlighted = on;
        Invalidate();
    }


    private void OnMouseDown(object? s, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        _dragging   = true;
        _dragOffset = e.Location;
        Capture     = true; 
        BringToFront();
        Invalidate();
    }

    private void OnMouseMove(object? s, MouseEventArgs e)
    {
        if (!_dragging || Parent is null) return;

        int nx = Math.Max(0, Math.Min(Parent.Width  - Width,  Left + e.X - _dragOffset.X));
        int ny = Math.Max(0, Math.Min(Parent.Height - Height, Top  + e.Y - _dragOffset.Y));

        Location = new Point(nx, ny);
        WorkspaceElement.Position = Location;
    }

    private void OnMouseUp(object? s, MouseEventArgs e)
    {
        if (!_dragging) return;
        _dragging = false;
        Capture   = false;
        Invalidate();
        DragEnded?.Invoke(this);
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode     = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAlias;

        var baseColor  = ParseHex(WorkspaceElement.Element.Color);
        var lightColor = Lighten(baseColor, 0.86f);

        if (_dragging)
        {
            using var shadow = new SolidBrush(Color.FromArgb(35, 0, 0, 0));
            using var sp = BuildRoundRect(new Rectangle(4, 6, Width - 3, Height - 3), 12);
            g.FillPath(shadow, sp);
        }

        var cardRect = new Rectangle(2, 2, Width - 5, Height - 5);
        using var bgPath = BuildRoundRect(cardRect, 12);
        using var grad = new LinearGradientBrush(
            cardRect, Color.White, lightColor, LinearGradientMode.Vertical);
        g.FillPath(grad, bgPath);

        Color borderColor = _highlighted
            ? Color.FromArgb(29, 158, 117)
            : Color.FromArgb(45, 0, 0, 0);
        float borderW = _highlighted ? 2.2f : 1f;
        using var borderPen = new Pen(borderColor, borderW);
        g.DrawPath(borderPen, bgPath);

        using var accentBrush = new SolidBrush(Color.FromArgb(190, baseColor));
        g.FillRectangle(accentBrush, cardRect.X + 6, cardRect.Y, cardRect.Width - 12, 4);

        var emoji    = WorkspaceElement.Element.Emoji;
        var emojiSz  = g.MeasureString(emoji, _emojiFont);
        g.DrawString(emoji, _emojiFont, Brushes.Black,
            (Width - emojiSz.Width) / 2f, 8f);

        var name   = WorkspaceElement.Element.Name;
        var nameSz = g.MeasureString(name, _nameFont);
        using var nameBrush = new SolidBrush(Color.FromArgb(65, 40, 40));
        g.DrawString(name, _nameFont, nameBrush,
            (Width - nameSz.Width) / 2f, Height - 18f);
    }


    private static GraphicsPath BuildRoundRect(Rectangle r, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    private static Color ParseHex(string hex)
    {
        try { return ColorTranslator.FromHtml(hex); }
        catch { return Color.LightGray; }
    }

    private static Color Lighten(Color c, float factor)
        => Color.FromArgb(
            (int)Math.Min(255, c.R + (255 - c.R) * factor),
            (int)Math.Min(255, c.G + (255 - c.G) * factor),
            (int)Math.Min(255, c.B + (255 - c.B) * factor));
}
