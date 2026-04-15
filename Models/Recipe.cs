namespace AlchemyGame.Models;

/// <summary>
/// Описывает рецепт: два входных элемента → результат
/// </summary>
public class Recipe
{
    public string InputA   { get; init; } = "";
    public string InputB   { get; init; } = "";
    public string ResultId { get; init; } = "";

    /// <summary>
    /// Проверяет совпадение рецепта (порядок не важен)
    /// </summary>
    public bool Matches(string a, string b)
        => (InputA == a && InputB == b) || (InputA == b && InputB == a);
}
