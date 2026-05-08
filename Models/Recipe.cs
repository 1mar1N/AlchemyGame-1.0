using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
namespace AlchemyGame.Models;

public class Recipe
{
    /// <summary>Id первого входного элемента</summary>
    public string InputA   { get; init; } = "";

    /// <summary>Id второго входного элемента</summary>
    public string InputB   { get; init; } = "";

    /// <summary>Id элемента-результата</summary>
    public string ResultId { get; init; } = "";

    /// <summary>
    /// Проверяет, совпадает ли пара (a, b) с этим рецептом.
    /// Порядок аргументов не важен.
    /// </summary>
    public bool Matches(string a, string b)
        => (InputA == a && InputB == b)
        || (InputA == b && InputB == a);
}
