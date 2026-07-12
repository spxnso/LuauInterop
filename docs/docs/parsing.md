## Parsing

## Installation

The AST parser is provided as a separate package, make sure to install it:

```bash
dotnet add package LuauInterop.Ast
```

> **Note:** `LuauInterop.Ast` is a 1:1 mapping of Luau's native AST structure. If you're familiar with Luau's C++ AST or official documentation, the managed node hierarchy mirrors it directly, making it easy to translate examples and tooling between the two.
>
> Luau AST documentation:
> https://deepwiki.com/luau-lang/luau/3.1-ast-node-types-and-visitor-pattern

### Parsing Source Code

Use `LuauParser.Parse` to parse Luau source code into a strongly-typed syntax tree:

```csharp
using LuauInterop.Ast;

SyntaxTree tree = LuauParser.Parse("""
local x = 10
return x * 2
""");
```

### Accessing the Root Node

```csharp
SyntaxTree tree = LuauParser.Parse("return 42");

var root = tree.Root;
Console.WriteLine(root.GetType()); // AstStatBlock
```

### Traversing the Syntax Tree

```csharp
SyntaxTree tree = LuauParser.Parse("""
local x = 5
local y = 10
return x + y
""");

foreach (var statement in tree.Root.Body)
{
    Console.WriteLine(statement.GetType());
}
```

### Error Handling

```csharp
var tree = LuauParser.Parse(source);
var errors = tree.Errors;

if (errors.Count > 0) 
{
    throw new Exception(
        "LuauInterop reported parsing errors:\n" +
        string.Join("\n", errors)
    );
}
```