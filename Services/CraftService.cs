global using System;
global using System.Collections.Generic;
global using System.Drawing;
global using System.Linq;
global using System.Windows.Forms;
using AlchemyGame.Data;
using AlchemyGame.Models;

namespace AlchemyGame.Services;


public class CraftService
{
    private readonly List<Recipe> _recipes;

    public CraftService()
    {
        _recipes = ElementData.Recipes;
    }

  
    public Element? TryCraft(string elementIdA, string elementIdB)
    {
        foreach (var recipe in _recipes)
        {
            if (recipe.Matches(elementIdA, elementIdB))
            {
                ElementData.Elements.TryGetValue(recipe.ResultId, out var result);
                return result;
            }
        }
        return null;
    }
}
