using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Nodes.Statements;


public class AstStatDeclareGlobal(Location location, string name, Location nameLocation, AstType type) : AstStat(location)
{
    public string Name { get; } = name;
    public Location NameLocation { get; } = nameLocation;
    public AstType Type { get; } = type;
}
