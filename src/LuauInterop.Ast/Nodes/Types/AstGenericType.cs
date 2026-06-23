namespace LuauInterop.Ast.Nodes.Types;

public class AstGenericType(string name, AstType? defaultValue = null)
{
    public string Name { get; } = name;
    public AstType? LuauType { get; } = defaultValue;
}
