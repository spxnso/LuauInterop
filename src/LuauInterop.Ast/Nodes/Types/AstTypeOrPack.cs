namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeOrPack(AstType? type = null, AstTypePack? typePack = null)
{
    public AstType? Type { get; } = type;
    public AstTypePack? TypePack { get; } = typePack;
}
