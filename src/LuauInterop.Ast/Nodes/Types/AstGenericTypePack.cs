namespace LuauInterop.Ast.Nodes.Types;


public class AstGenericTypePack(string name, AstTypePack? defaultValue = null)
{
    public string Name { get; } = name;
    public AstTypePack? LuauType { get; } = defaultValue;
}
