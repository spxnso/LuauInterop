namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatBlock(Location location, IReadOnlyList<AstStat> body, bool hasEnd) : AstStat(location)
{
    public IReadOnlyList<AstStat> Body { get; } = body;
    public bool HasEnd { get; } = hasEnd;
}
