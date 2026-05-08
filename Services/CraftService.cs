using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AlchemyGame.Data;
using AlchemyGame.Models;
namespace AlchemyGame.Services;

public class CraftService
{
    private readonly List<Recipe> _recipes = ElementData.Recipes;

    /// <summary>
    /// Ищет рецепт для двух элементов.
    /// Возвращает Element-результат, или null если рецепта нет.
    /// </summary>
    public Element? TryCraft(string idA, string idB)
    {
        foreach (var recipe in _recipes)
        {
            if (!recipe.Matches(idA, idB)) continue;
            ElementData.Elements.TryGetValue(recipe.ResultId, out var result);
            return result; // null если ResultId не найден в словаре (ошибка данных)
        }
        return null;
    }
}
