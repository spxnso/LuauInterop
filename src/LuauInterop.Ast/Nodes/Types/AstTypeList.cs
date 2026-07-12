namespace LuauInterop.Ast.Nodes.Types;


public class AstTypeList(IReadOnlyList<AstType> types, AstTypePack? tailType = null)
{
    public IReadOnlyList<AstType> Types { get; } = types;
    public AstTypePack? TailType { get; } = tailType;
}
