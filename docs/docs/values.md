# Values and Types

## LuauType

Luau has the following value kinds:

| Type | Description |
|------|-------------|
| `Nil` | Absence of a value |
| `Boolean` | `true` or `false` |
| `Integer` | 64-bit integer |
| `Number` | 64-bit float |
| `String` | UTF-8 string |
| `Vector` | 3-component float vector |
| `Table` | Key-value table |
| `Function` | Callable function |
| `UserData` | Native userdata |
| `Thread` | Coroutine |
| `Buffer` | Byte buffer |
| `LightUserData` | Unmanaged pointer |

## LuauValue

`LuauValue` is a lightweight typed carrier for moving values between C# and Luau without boxing for common types.

```csharp
LuauValue number = 3.14;
LuauValue integer = 42L;
LuauValue text = "hello";
LuauValue flag = true;
LuauValue nothing = LuauValue.Nil;
```

Convert back to a CLR object:

```csharp
object? obj = value.ToObject();
```

## CLR to Luau Type Mapping

| CLR Type | Luau Type |
|----------|-----------|
| `null` | `Nil` |
| `bool` | `Boolean` |
| `string`, `char` | `String` |
| `int`, `long`, `short`, etc. | `Integer` |
| `float`, `double`, `decimal` | `Number` |
| `Vector3` | `Vector` |
| `byte[] ` | `Buffer` |
| `Func<Luau, int>` or `Delegate` | `Function` (by reference) |
| `LuauTable` | `Table` (by reference) |
| `LuauThread` | `Thread` (by reference) |