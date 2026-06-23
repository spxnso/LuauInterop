namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeSingletonString(Location location, string value) : AstType(location)
{
    public string Value { get; } = value;
}