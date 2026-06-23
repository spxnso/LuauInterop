namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprTableItem(string kind, AstExpr? key, AstExpr value)
{
    public string Kind { get; } = kind;
    public AstExpr? Key { get; } = key;
    public AstExpr Value { get; } = value;
}
