using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatTypeAlias(
    Location location,
    string name,
    IReadOnlyList<AstGenericType> generics,
    IReadOnlyList<AstGenericTypePack> genericPacks,
    AstType value,
    bool exported
) : AstStat(location)
{
    public string Name { get; } = name;
    public IReadOnlyList<AstGenericType> Generics { get; } = generics;
    public IReadOnlyList<AstGenericTypePack> GenericPacks { get; } = genericPacks;
    public AstType Value { get; } = value;
    public bool Exported { get; } = exported;
}
