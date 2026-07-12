using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatDeclareClass(
    Location location,
    string name,
    string? superName,
    IReadOnlyList<AstDeclaredClassProp> props,
    AstTableIndexer? indexer
) : AstStat(location)
{
    public string Name { get; } = name;
    public string? SuperName { get; } = superName;
    public IReadOnlyList<AstDeclaredClassProp> Props { get; } = props;
    public AstTableIndexer? Indexer { get; } = indexer;
}