namespace AlchemyGame.Models;

/// <summary>
/// Описывает один игровой элемент (Огонь, Вода, Пар и т.д.)
/// </summary>
public class Element
{
    public string Id       { get; init; } = "";
    public string Name     { get; init; } = "";
    public string Category { get; init; } = "";
    public string Emoji    { get; init; } = "";

    public override string ToString() => Name;
}
