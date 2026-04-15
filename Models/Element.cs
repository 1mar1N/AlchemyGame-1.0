namespace AlchemyGame.Models;

public class Element
{
    public string Id       { get; init; } = "";
    public string Name     { get; init; } = "";
    public string Category { get; init; } = "";
    public string Emoji    { get; init; } = "";

    public override string ToString() => Name;
}
