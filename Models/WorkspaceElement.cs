using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
namespace AlchemyGame.Models;

public class WorkspaceElement
{
    private static int _nextId = 1;

    /// <summary>Уникальный id этого конкретного экземпляра на поле</summary>
    public int InstanceId { get; }

    /// <summary>Ссылка на тип элемента (имя, эмодзи, цвет)</summary>
    public Element Element { get; }

    /// <summary>Текущая позиция левого верхнего угла карточки на поле</summary>
    public Point Position { get; set; }

    public WorkspaceElement(Element element, Point position)
    {
        InstanceId = _nextId++;
        Element    = element;
        Position   = position;
    }

    /// <summary>
    /// Прямоугольник карточки (80×80 px) для проверки пересечений через IntersectsWith().
    /// </summary>
    public Rectangle Bounds => new Rectangle(Position, new Size(80, 80));
}
