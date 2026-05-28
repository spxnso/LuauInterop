![LuauInterop](assets/icon-200x200.png)

# LuauInterop

Luau bindings for .NET

[![NuGet](https://img.shields.io/nuget/v/LuauInterop.svg)](https://www.nuget.org/packages/LuauInterop)
[![NuGet Downloads](https://img.shields.io/nuget/dt/LuauInterop.svg)](https://www.nuget.org/packages/LuauInterop)
[![CI](https://github.com/spxnso/LuauInterop/actions/workflows/ci.yml/badge.svg)](https://github.com/spxnso/LuauInterop/actions/workflows/ci.yml)
[![License](https://img.shields.io/github/license/spxnso/LuauInterop.svg)](https://github.com/spxnso/LuauInterop/blob/master/LICENSE)

---

Luau bindings for .NET. Run Luau scripts, compile bytecode, and interact with the Luau VM directly from C#.

## Installation

```bash
dotnet add package LuauInterop
dotnet add package LuauInterop.Native
```

## Quick Start

```csharp
using var luau = new Luau.Luau();
luau.OpenLibraries();

var results = luau.DoString("return 1 + 1");
Console.WriteLine(results[0]); // 2
```

