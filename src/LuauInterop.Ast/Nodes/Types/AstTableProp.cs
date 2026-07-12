namespace LuauInterop.Ast.Nodes.Types;


public class AstTableProp(string name, Location location, AstType propType)
{
    public string Name { get; } = name;
    public Location Location { get; } = location;
    public AstType PropType { get; } = propType;
}
