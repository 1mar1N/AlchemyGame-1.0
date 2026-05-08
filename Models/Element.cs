using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
namespace AlchemyGame.Models;
public class Element
{
    /// <summary>Уникальный ключ, например "fire", "water", "steam"</summary>
    public string Id       { get; init; } = "";

    /// <summary>Имя на русском, которое видит игрок</summary>
    public string Name     { get; init; } = "";

    /// <summary>Категория для группировки: "Природа", "Еда", "Технологии" и т.д.</summary>
    public string Category { get; init; } = "";

    /// <summary>Эмодзи-иконка для отрисовки карточки</summary>
    public string Emoji    { get; init; } = "";

    /// <summary>HEX-цвет карточки, например "#FF6B35"</summary>
    public string Color    { get; init; } = "#9E9E9E";

    public override string ToString() => Name;
}
