using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
namespace AlchemyGame.Models;
public class Element
{
    public string Id       { get; init; } = "";

    public string Name     { get; init; } = "";

    public string Category { get; init; } = "";

    public string Emoji    { get; init; } = "";

    public string Color    { get; init; } = "#9E9E9E";

    public override string ToString() => Name;
}
