namespace LuauInterop.Ast.Nodes;

public readonly record struct Location(Position Begin, Position End)
{
    public override string ToString() => $"{Begin}: {End}";
}