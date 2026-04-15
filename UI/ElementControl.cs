global using System;
global using System.Collections.Generic;
global using System.Drawing;
global using System.Linq;
global using System.Windows.Forms;
using AlchemyGame.Models;

namespace AlchemyGame.UI;


public class ElementControl : Panel
{
    public WorkspaceElement WorkspaceElement { get; }

    private bool  _isDragging;
    private Point _dragOffset;

    public event EventHandler<ElementControl>? DragEnded;

    public ElementControl(WorkspaceElement wsElem)
    {
        WorkspaceElement = wsElem;
        Size        = new Size(76, 76);
        Location    = wsElem.Position;
        BackColor   = Color.White;
        Cursor      = Cursors.Hand;

        SetStyle(ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint  |
                 ControlStyles.UserPaint, true);

        MouseDown += OnMouseDown;
        MouseMove += OnMouseMove;
        MouseUp   += OnMouseUp;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var rect = new Rectangle(1, 1, Width - 2, Height - 2);
        using var brush = new SolidBrush(BackColor);
        using var pen   = new Pen(Color.FromArgb(220, 220, 215), 1f);
        FillRoundedRect(g, brush, rect, 10);
        DrawRoundedRect(g, pen,   rect, 10);

        var emoji = WorkspaceElement.Element.Emoji;
        using var emojiFont = new Font("Segoe UI Emoji", 20f);
        var emojiSize = g.MeasureString(emoji, emojiFont);
        g.DrawString(emoji, emojiFont, Brushes.Black,
            (Width  - emojiSize.Width)  / 2,
            (Height - emojiSize.Height) / 2 - 8);

        var name = WorkspaceElement.Element.Name;
        using var nameFont = new Font("Segoe UI", 7.5f);
        using var nameBrush = new SolidBrush(Color.FromArgb(100, 100, 100));
        var nameSize = g.MeasureString(name, nameFont);
        g.DrawString(name, nameFont, nameBrush,
            (Width - nameSize.Width) / 2,
            Height - 18);
    }


    public void SetHighlight(bool on)
    {
        BackColor = on
            ? Color.FromArgb(237, 250, 244)
            : Color.White;
        Invalidate();
    }


    private void OnMouseDown(object? s, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        _isDragging = true;
        _dragOffset = e.Location;
        BringToFront();
    }

    private void OnMouseMove(object? s, MouseEventArgs e)
    {
        if (!_isDragging) return;
        var parent = Parent;
        if (parent is null) return;

        int nx = Left + e.X - _dragOffset.X;
        int ny = Top  + e.Y - _dragOffset.Y;

        nx = Math.Max(0, Math.Min(parent.Width  - Width,  nx));
        ny = Math.Max(0, Math.Min(parent.Height - Height, ny));

        Location = new Point(nx, ny);
        WorkspaceElement.Position = Location;
    }

    private void OnMouseUp(object? s, MouseEventArgs e)
    {
        if (!_isDragging) return;
        _isDragging = false;
        DragEnded?.Invoke(this, this);
    }


    private static void FillRoundedRect(Graphics g, Brush b, Rectangle r, int radius)
    {
        using var path = RoundedPath(r, radius);
        g.FillPath(b, path);
    }

    private static void DrawRoundedRect(Graphics g, Pen p, Rectangle r, int radius)
    {
        using var path = RoundedPath(r, radius);
        g.DrawPath(p, path);
    }

    private static System.Drawing.Drawing2D.GraphicsPath RoundedPath(Rectangle r, int d)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
