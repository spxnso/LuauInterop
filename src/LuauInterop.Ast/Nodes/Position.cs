namespace LuauInterop.Ast.Nodes;

public readonly record struct Position(int Line, int Column)
{
    public override string ToString() => $"{Line}:{Column}";
}
