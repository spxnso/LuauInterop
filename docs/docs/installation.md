# Installation
LuauInterop is split into packages:

| Package | Description |
|---------|-------------|
| `LuauInterop` | The managed bindings |
| `LuauInterop.Native` | The native Luau runtime libraries |
| `LuauInterop.Sandbox` | Sandboxing utilities for untrusted code (optional) |

Install the core packages:
```bash
dotnet add package LuauInterop
dotnet add package LuauInterop.Native
```

Optionally, add the sandbox package:
```bash
dotnet add package LuauInterop.Sandbox
```

### Supported Platforms
| Platform | Runtime ID |
|----------|------------|
| Windows x64 | `win-x64` |
| Linux x64 | `linux-x64` |
| macOS x64 | `osx-x64` |