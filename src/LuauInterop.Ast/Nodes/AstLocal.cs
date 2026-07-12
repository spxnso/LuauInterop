using LuauInterop.Ast.Nodes.Types;

namespace LuauInterop.Ast.Nodes;

public class AstLocal(Location location, string name, bool isConst = false) : AstNode(location)
{
    public string Name { get; } = name;
    public bool IsConst { get; } = isConst;
    public AstType? LuauType { get; set; }
}
