namespace LuauInterop.Ast.Nodes.Types;


public class AstTableIndexer(Location location, AstType indexType, AstType resultType)
{
    public Location Location { get; } = location;
    public AstType IndexType { get; } = indexType;
    public AstType ResultType { get; } = resultType;
}
