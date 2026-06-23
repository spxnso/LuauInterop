namespace LuauInterop.Ast.Nodes;

public class AstArgumentName(string name, Location location)
{
    public string Name { get; } = name;
    public Location Location { get; } = location;
}