namespace LuauInterop.Ast.Nodes.Types;

public class AstTypeReference(
    Location location,
    string? prefix,
    Location? prefixLocation,
    string name,
    Location nameLocation,
    IReadOnlyList<AstTypeOrPack> parameters
) : AstType(location)
{
    public string? Prefix { get; } = prefix;
    public Location? PrefixLocation { get; } = prefixLocation;
    public string Name { get; } = name;
    public Location NameLocation { get; } = nameLocation;
    public IReadOnlyList<AstTypeOrPack> Parameters { get; } = parameters;
}