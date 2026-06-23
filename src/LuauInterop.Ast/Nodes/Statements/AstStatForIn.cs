using LuauInterop.Ast.Nodes.Expressions;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatForIn(Location location, IReadOnlyList<AstLocal> vars, IReadOnlyList<AstExpr> values, AstStatBlock body, bool hasIn, bool hasDo) : AstStat(location)
{
    public IReadOnlyList<AstLocal> Vars { get; } = vars;
    public IReadOnlyList<AstExpr> Values { get; } = values;
    public AstStatBlock Body { get; } = body;
    public bool HasIn { get; } = hasIn;
    public bool HasDo { get; } = hasDo;
}
