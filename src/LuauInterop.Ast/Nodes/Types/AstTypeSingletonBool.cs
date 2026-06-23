namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeSingletonBool(Location location, bool value) : AstType(location)
{
    public bool Value { get; } = value;
}