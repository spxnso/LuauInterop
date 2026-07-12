using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprTypeAssertion(Location location, AstExpr expr, AstType annotation) : AstExpr(location)
{
    public AstExpr Expression { get; } = expr;
    public AstType Annotation { get; } = annotation;
}
