using LuauInterop.Ast.Nodes.Statements;
using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Nodes.Expressions;

public class AstExprFunction(
    Location location,
    IReadOnlyList<AstAttr> attributes,
    IReadOnlyList<AstGenericType> generics,
    IReadOnlyList<AstGenericTypePack> genericPacks,
    AstLocal? self,
    IReadOnlyList<AstLocal> args,
    AstTypeList? returnAnnotation,
    bool? vararg,
    Location? varargLocation,
    AstTypePack? varargAnnotation,
    AstStatBlock body,
    int functionDepth,
    string? debugname
) : AstExpr(location)
{
    public IReadOnlyList<AstAttr> Attributes { get; } = attributes;
    public IReadOnlyList<AstGenericType> Generics { get; } = generics;
    public IReadOnlyList<AstGenericTypePack> GenericPacks { get; } = genericPacks;
    public AstLocal? Self { get; } = self;
    public IReadOnlyList<AstLocal> Args { get; } = args;
    public AstTypeList? ReturnAnnotation { get; } = returnAnnotation;
    public bool? Vararg { get; } = vararg;
    public Location? VarargLocation { get; } = varargLocation;
    public AstTypePack? VarargAnnotation { get; } = varargAnnotation;
    public AstStatBlock Body { get; } = body;
    public int FunctionDepth { get; } = functionDepth;
    public string? DebugName { get; } = debugname;
}
