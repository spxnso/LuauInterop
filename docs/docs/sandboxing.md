## Sandboxing

## Installation

The sandbox is an optional package, make sure to install it:

```bash
dotnet add package LuauInterop.Sandbox
```

`LuauSandbox` wraps a `Luau` instance.

```csharp
using var sandbox = new LuauSandbox();
var results = sandbox.Execute("return 1 + 1");
Console.WriteLine(results[0]); // 2
```

### Presets

Use `LuauSandboxOptions.Strict` for fully untrusted code, it removes dangerous globals like `setfenv`, `rawset`, and limits available libraries:

```csharp
using var sandbox = new LuauSandbox(LuauSandboxOptions.Strict);
var results = sandbox.Execute("return math.sqrt(16)");
Console.WriteLine(results[0]); // 4
```

### Customizing Options

Options are immutable records, chain methods to build your configuration:

```csharp
var options = LuauSandboxOptions.Strict
    .AllowLibrary(LuauLibrary.Coroutine)
    .DenyGlobal("collectgarbage")
    .AllowGlobal("rawget");

using var sandbox = new LuauSandbox(options);
```

### Injecting Globals

Expose values to the sandboxed script via `AllowedGlobals`:

```csharp
var options = new LuauSandboxOptions
{
    AllowedGlobals = new Dictionary<string, object?>
    {
        ["VERSION"] = "1.0.0",
        ["MAX_SCORE"] = 100.0,
    }
};

using var sandbox = new LuauSandbox(options);
var results = sandbox.Execute("return VERSION");
Console.WriteLine(results[0]); // 1.0.0
```

### Script Isolation

By default each `Execute` call gets its own proxied `_G` via `luaL_sandboxthread`, so scripts cannot share state between calls. Disable this only if you explicitly want shared globals:

```csharp
var options = new LuauSandboxOptions
{
    IsolateScripts = false
};
```

### Error Handling

Runtime errors surface as `LuauException`:

```csharp
try
{
    sandbox.Execute("error('not allowed')");
}
catch (LuauException ex)
{
    Console.WriteLine(ex.Message);
}
```