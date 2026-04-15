namespace AlchemyGame.Models;


public class Recipe
{
    public string InputA   { get; init; } = "";
    public string InputB   { get; init; } = "";
    public string ResultId { get; init; } = "";

    public bool Matches(string a, string b)
        => (InputA == a && InputB == b) || (InputA == b && InputB == a);
}
