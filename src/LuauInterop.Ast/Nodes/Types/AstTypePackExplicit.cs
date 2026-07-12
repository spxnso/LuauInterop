namespace LuauInterop.Ast.Nodes.Types;

public class AstTypePackExplicit(Location location, AstTypeList typeList) : AstTypePack(location)
{
    public AstTypeList TypeList { get; } = typeList;
}