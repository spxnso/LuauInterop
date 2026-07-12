
using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Nodes.Statements;

public class AstDeclaredClassProp(string name, Location nameLocation, AstType luauType, Location location)
{
    public string Name { get; } = name;
    public Location NameLocation { get; } = nameLocation;
    public AstType LuauType { get; } = luauType;
    public Location Location { get; } = location;
}
