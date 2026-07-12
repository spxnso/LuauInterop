using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstStatDeclareFunction(
    Location location,
    IReadOnlyList<AstAttr> attributes,
    string name,
    Location nameLocation,
    AstTypeList @params,
    IReadOnlyList<AstArgumentName?> paramNames,
    AstArgumentName? vararg,
    Location? varargLocation,
    AstTypeList retTypes,
    IReadOnlyList<AstGenericType> generics,
    IReadOnlyList<AstGenericTypePack> genericPacks
) : AstStat(location)
{
    public IReadOnlyList<AstAttr> Attributes { get; } = attributes;
    public string Name { get; } = name;
    public Location NameLocation { get; } = nameLocation;
    public AstTypeList Params { get; } = @params;
    public IReadOnlyList<AstArgumentName?> ParamNames { get; } = paramNames;
    public AstArgumentName? Vararg { get; } = vararg;
    public Location? VarargLocation { get; } = varargLocation;
    public AstTypeList RetTypes { get; } = retTypes;
    public IReadOnlyList<AstGenericType> Generics { get; } = generics;
    public IReadOnlyList<AstGenericTypePack> GenericPacks { get; } = genericPacks;
}
