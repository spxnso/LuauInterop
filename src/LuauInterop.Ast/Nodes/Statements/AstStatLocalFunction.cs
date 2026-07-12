
using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;


public class AstStatLocalFunction(Location location, AstLocal name, AstExprFunction func) : AstStat(location)
{
    public AstLocal Name { get; } = name;
    public AstExprFunction Func { get; } = func;
}
