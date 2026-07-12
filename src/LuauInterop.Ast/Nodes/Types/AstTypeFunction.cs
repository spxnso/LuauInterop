namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeFunction(
    Location location,
    IReadOnlyList<AstAttr> attributes,
    IReadOnlyList<AstGenericType> generics,
    IReadOnlyList<AstGenericTypePack> genericPacks,
    AstTypeList argTypes,
    IReadOnlyList<AstArgumentName?> argNames,
    AstTypeList returnTypes
) : AstType(location)
{
    public IReadOnlyList<AstAttr> Attributes { get; } = attributes;
    public IReadOnlyList<AstGenericType> Generics { get; } = generics;
    public IReadOnlyList<AstGenericTypePack> GenericPacks { get; } = genericPacks;
    public AstTypeList ArgTypes { get; } = argTypes;
    public IReadOnlyList<AstArgumentName?> ArgNames { get; } = argNames;
    public AstTypeList ReturnTypes { get; } = returnTypes;
}