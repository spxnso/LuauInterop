using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeTypeof(Location location, AstExpr expr) : AstType(location)
{
    public AstExpr Expression { get; } = expr;
}