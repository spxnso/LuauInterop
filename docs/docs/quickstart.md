# Quick Start

## Creating a VM

```csharp
using var luau = new Luau();
luau.OpenLibraries();
```

Always wrap the `Luau` instance in a `using` block, it owns the native state and must be disposed.

## Running a Script

```csharp
var results = luau.DoString("return 1 + 1");
Console.WriteLine(results[0]); // 2
```

`DoString` returns every value the script returns as `object?[]`.

## Compiling Bytecode

Pre-compiling is useful when you want to run the same script multiple times without recompiling:

```csharp
using var chunk = luau.Compile("return function(x) return x * x end");
var results = luau.DoChunk(chunk);
```

## Calling a Luau Function

```csharp
luau.DoString(@"
    function greet(name)
        return 'Hello, ' .. name .. '!'
    end
");

var greet = luau["greet"] as LuauFunction ?? throw new InvalidOperationException("greet is not a function");;
using (greet)
{
    var result = greet.Call("World");
    Console.WriteLine(result[0]); // Hello, World!
}
```

## Working with Tables

```csharp
luau.DoString("player = { name = 'Alice', score = 100 }");

var player = luau["player"] as LuauTable ?? throw new InvalidOperationException("player is not a table.");
using (player)
{
    Console.WriteLine(player["name"]);  // Alice
    Console.WriteLine(player["score"]); // 100

    player["score"] = 200L;
}
```

## Global Variables

```csharp
// Set a global
luau["version"] = "1.0.0";

// Get a global
var version = luau["version"];
Console.WriteLine(version); // 1.0.0
```

## Coroutines

```csharp
luau.DoString(@"
    co = coroutine.create(function(x)
        coroutine.yield(x + 1)
        return x + 2
    end)
");

var co = luau["co"] as LuauThread;
using (co)
{
    var r1 = co.Resume(10); // Don't forget to cast to double!
    Console.WriteLine(r1[0]); // 11

    var r2 = co.Resume();
    Console.WriteLine(r2[0]); // 12
}
```

## Using FFlags

Here's an example using fflags to use the newest Integer type and library.

```csharp
using var luau = new Luau();
        
luau.SetFFlag("LuauIntegerType", true);
luau.SetFFlag("LuauIntegerLibrary", true);

Console.WriteLine(luau.GetFFlag("LuauIntegerType")) // True

luau.OpenLibraries();

var results = luau.DoString(@"
    return integer.fromstring('1011100', 2) -- 92
");

Console.WriteLine(results[0]);
```

## Callbacks

```csharp
luau.RegisterCallback("add", (vm, state) =>
{
    double a = (double)(vm.GetObject(1, state) ?? 0);
    double b = (double)(vm.GetObject(2, state) ?? 0);
    state.PushNumber(a + b);
    return 1; // number of return values
});

var results = luau.DoString("return add(10, 20)");
Console.WriteLine(results[0]); // 30
```

## Error Handling

Runtime and compilation errors are surfaced as `LuauException`:

```csharp
try
{
    luau.DoString("error('something went wrong')");
}
catch (LuauException ex)
{
    Console.WriteLine(ex.Message);
}
```