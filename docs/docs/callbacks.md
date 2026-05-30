## Callbacks

Register a C# function as a global callable from Luau:

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

The second parameter `state` is the calling state. Always use it to read arguments and push return values, so callbacks work correctly from both the main state and coroutines.

### Reading Arguments

Arguments are on the stack starting at index 1:

```csharp
luau.RegisterCallback("greet", (vm, state) =>
{
    string name = vm.GetObject(1, state) as string ?? "world";
    state.PushString($"Hello, {name}!");
    return 1;
});

var results = luau.DoString("return greet('Alice')");
Console.WriteLine(results[0]); // Hello, Alice!
```

### Using Delegates

For simpler callbacks, you can assign a delegate directly via the indexer and argument marshalling is handled automatically:

```csharp
luau["multiply"] = (Func<double, double, double>)((a, b) => a * b);

var results = luau.DoString("return multiply(6, 7)");
Console.WriteLine(results[0]); // 42
```

### Exception Handling

Exceptions thrown inside a callback are safely caught and re-thrown on the C# side after the VM unwinds:

```csharp
luau.RegisterCallback("fail", (vm, state) =>
{
    throw new Exception("something went wrong");
});

try
{
    luau.DoString("fail()");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message); // something went wrong
}
```