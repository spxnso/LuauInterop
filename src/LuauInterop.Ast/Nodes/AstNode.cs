namespace LuauInterop.Ast.Nodes;

public abstract class AstNode(Location location)
{
    public Location Location { get; } = location;
    // Accept(LuauVisitor visitor)
}