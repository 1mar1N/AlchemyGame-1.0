using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace AlchemyGame.Models;


public class WorkspaceElement
{
    private static int _nextId = 1;

    public int     InstanceId { get; }
    public Element Element    { get; }
    public Point   Position   { get; set; }

    public WorkspaceElement(Element element, Point position)
    {
        InstanceId = _nextId++;
        Element    = element;
        Position   = position;
    }

    public Rectangle Bounds => new Rectangle(Position, new Size(72, 72));
}
