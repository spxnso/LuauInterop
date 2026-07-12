namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeTable(Location location, IReadOnlyList<AstTableProp> props, AstTableIndexer? indexer) : AstType(location)
{
    public IReadOnlyList<AstTableProp> Props { get; } = props;
    public AstTableIndexer? Indexer { get; } = indexer;
}