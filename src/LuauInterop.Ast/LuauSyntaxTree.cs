using LuauInterop.Ast.Nodes.Statements;

namespace LuauInterop.Ast;

public sealed class SyntaxTree(AstStatBlock root, IReadOnlyCollection<LuauComment> comments, IReadOnlyCollection<LuauParseError> errors)
{
    public AstStatBlock Root { get; } = root;

    public IReadOnlyCollection<LuauComment> Comments { get; } = comments;

    public IReadOnlyCollection<LuauParseError> Errors { get; } = errors;

    public bool HasErrors => Errors.Count > 0;
}