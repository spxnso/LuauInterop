namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprCall(Location location, AstExpr func, IReadOnlyList<AstExpr> args, bool self, Location argLocation) : AstExpr(location)
{
    public AstExpr Func { get; } = func;
    public IReadOnlyList<AstExpr> Args { get; } = args;
    public bool Self { get; } = self;
    public Location ArgLocation { get; } = argLocation;
}
