namespace LuauInterop.Ast.Nodes;

public class AstAttr(Location location, string name) : AstNode(location)
{
    public string Name { get; } = name;
}