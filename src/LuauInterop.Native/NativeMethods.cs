using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using lua_State = nint;

namespace LuauInterop.Native;

#pragma warning disable IDE1006 // parameter name "L" is conventional in Lua C API

/// <summary>
/// Class containing P/Invoke signatures for the Luau C API.
/// </summary>
public static partial class NativeMethods
{
    private const string LuauLibraryName = "luau";

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial lua_State lua_newstate(nint f, nint ud);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_close(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial lua_State lua_newthread(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial lua_State lua_mainthread(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_resetthread(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_isthreadreset(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_absindex(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_gettop(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_settop(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushvalue(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_remove(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_insert(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_replace(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_checkstack(lua_State L, int sz);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_rawcheckstack(lua_State L, int sz);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_xmove(lua_State from, lua_State to, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_xpush(lua_State from, lua_State to, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_isnumber(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_isstring(lua_State L, int idx);

    // Cf. LuaState.cs
    // [LibraryImport(LuauLibraryName)]
    // [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    // public static partial int lua_isinteger64(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_iscfunction(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_isLfunction(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_isuserdata(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_type(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_typename(lua_State L, int tp);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_equal(lua_State L, int idx1, int idx2);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_rawequal(lua_State L, int idx1, int idx2);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_lessthan(lua_State L, int idx1, int idx2);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial double lua_tonumberx(lua_State L, int idx, out int isnum);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_tointegerx(lua_State L, int idx, out int isnum);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial uint lua_tounsignedx(lua_State L, int idx, out int isnum);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_tovector(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_toboolean(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial long lua_tointeger64(lua_State L, int idx, out int isinteger);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_tolstring(lua_State L, int idx, out nuint len);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_tostringatom(lua_State L, int idx, out int atom);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_tolstringatom(lua_State L, int idx, out nuint len, out int atom);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_namecallatom(lua_State L, out int atom);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_objlen(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_tocfunction(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_tolightuserdata(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_tolightuserdatatagged(lua_State L, int idx, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_touserdata(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_touserdatatagged(lua_State L, int idx, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_userdatatag(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_lightuserdatatag(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial lua_State lua_tothread(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_tobuffer(lua_State L, int idx, out nuint len);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_topointer(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushnil(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushnumber(lua_State L, double n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushinteger(lua_State L, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushinteger64(lua_State L, long n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushunsigned(lua_State L, uint n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushvector(lua_State L, float x, float y, float z);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushlstring(lua_State L, string s, nuint l);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushstring(lua_State L, string s);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_pushvfstring(lua_State L, string fmt, nint argp);

    [DllImport(LuauLibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint lua_pushfstringL(lua_State L, string fmt, __arglist);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushcclosurek(lua_State L, nint fn, string debugname, int nup, nint cont);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushboolean(lua_State L, int b);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_pushthread(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushlightuserdatatagged(lua_State L, nint p, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_newuserdatatagged(lua_State L, nuint sz, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_newuserdatataggedwithmetatable(lua_State L, nuint sz, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_newuserdatadtor(lua_State L, nuint sz, nint dtor);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_newbuffer(lua_State L, nuint sz);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_gettable(lua_State L, int idx);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_getfield(lua_State L, int idx, string k);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_rawgetfield(lua_State L, int idx, string k);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_rawget(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_rawgeti(lua_State L, int idx, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_rawgetptagged(lua_State L, int idx, nint p, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_createtable(lua_State L, int narr, int nrec);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setreadonly(lua_State L, int idx, int enabled);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_getreadonly(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setsafeenv(lua_State L, int idx, int enabled);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_getmetatable(lua_State L, int objindex);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_getfenv(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_settable(lua_State L, int idx);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setfield(lua_State L, int idx, string k);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_rawsetfield(lua_State L, int idx, string k);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_rawset(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_rawseti(lua_State L, int idx, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_rawsetptagged(lua_State L, int idx, nint p, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_setmetatable(lua_State L, int objindex);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_setfenv(lua_State L, int idx);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luau_load(lua_State L, string chunkname, nint data, nuint size, int env);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_call(lua_State L, int nargs, int nresults);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_pcall(lua_State L, int nargs, int nresults, int errfunc);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_cpcall(lua_State L, nint func, nint ud);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_yield(lua_State L, int nresults);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_break(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_resume(lua_State L, lua_State from, int narg);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_resumeerror(lua_State L, lua_State from);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_status(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_isyieldable(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_getthreaddata(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setthreaddata(lua_State L, nint data);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_costatus(lua_State L, lua_State co);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_gc(lua_State L, int what, int data);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setmemcat(lua_State L, int category);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nuint lua_totalbytes(lua_State L, int category);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_error(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_next(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_rawiter(lua_State L, int idx, int iter);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_concat(lua_State L, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nuint lua_encodepointer(lua_State L, nuint p);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial double lua_clock();

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setuserdatatag(lua_State L, int idx, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setuserdatadtor(lua_State L, int tag, nint dtor);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_getuserdatadtor(lua_State L, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setuserdatametatable(lua_State L, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_getuserdatametatable(lua_State L, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_registeruserdatadirectaccess(lua_State L, int tag, nint get, nint set, nint namecall);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_registeruserdatadirectfieldget(lua_State L, int tag, string field, nint fn);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setnumber(nint result, double n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setvector(nint result, float x, float y, float z);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setboolean(nint result, int b);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setinteger64(nint result, long n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setnil(nint result);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setlightuserdataname(lua_State L, int tag, string name);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_getlightuserdataname(lua_State L, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_clonefunction(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_cleartable(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_clonetable(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_getallocf(lua_State L, out nint ud);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_ref(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_unref(lua_State L, int refid);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_stackdepth(lua_State L);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_getinfo(lua_State L, int level, string what, nint ar);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_getargument(lua_State L, int level, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_getlocal(lua_State L, int level, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_setlocal(lua_State L, int level, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_getupvalue(lua_State L, int funcindex, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_setupvalue(lua_State L, int funcindex, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_singlestep(lua_State L, int enabled);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_breakpoint(lua_State L, int funcindex, int line, int enabled);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_getcoverage(lua_State L, int funcindex, nint context, nint callback);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_getcounters(lua_State L, int funcindex, nint context, nint functionvisit, nint countervisit);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint lua_debugtrace(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint luaL_newstate();

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luaL_openlibs(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint luau_compile(nint source, nuint size, nint options, out nuint outsize);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_base(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_coroutine(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_table(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_os(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_string(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_bit32(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_buffer(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_utf8(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_math(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_debug(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_vector(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luaopen_integer(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luaL_sandbox(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luaL_sandboxthread(lua_State L);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luau_setfflag(string name, int value);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luau_getfflag(string name);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luau_pushcsharpfunc(nint state, nint fnPtr);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial nint luau_parse(string source, nuint sourceLength);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void free(nint ptr);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void cpp_delete(nint ptr);
}
