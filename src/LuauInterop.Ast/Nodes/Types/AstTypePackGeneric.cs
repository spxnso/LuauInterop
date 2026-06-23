namespace LuauInterop.Ast.Nodes.Types;

public class AstTypePackGeneric(Location location, string genericName) : AstTypePack(location)
{
    public string GenericName { get; } = genericName;
}