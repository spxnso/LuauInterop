using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using lua_State = System.IntPtr;

namespace LuauInterop.Native;

/// <summary>
/// Class containing P/Invoke signatures for the Luau C API.
/// </summary>
public static partial class NativeMethods
{
    private const string LuauLibraryName = "luau";

    #region State manipulation
    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial lua_State lua_newstate(IntPtr f, IntPtr ud);

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
    #endregion

    #region Basic stack manipulation
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
    #endregion

    #region Access functions (stack -> C)
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
    public static partial IntPtr lua_typename(lua_State L, int tp);

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
    public static partial IntPtr lua_tovector(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_toboolean(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial long lua_tointeger64(lua_State L, int idx, out int isinteger);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_tolstring(lua_State L, int idx, out UIntPtr len);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_tostringatom(lua_State L, int idx, out int atom);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_tolstringatom(lua_State L, int idx, out UIntPtr len, out int atom);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_namecallatom(lua_State L, out int atom);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_objlen(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_tocfunction(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_tolightuserdata(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_tolightuserdatatagged(lua_State L, int idx, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_touserdata(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_touserdatatagged(lua_State L, int idx, int tag);

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
    public static partial IntPtr lua_tobuffer(lua_State L, int idx, out UIntPtr len);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_topointer(lua_State L, int idx);
    #endregion

    #region Push functions (C -> stack)
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
    public static partial void lua_pushlstring(lua_State L, string s, UIntPtr l);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushstring(lua_State L, string s);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_pushvfstring(lua_State L, string fmt, IntPtr argp);

    [DllImport(LuauLibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr lua_pushfstringL(lua_State L, string fmt, __arglist);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushcclosurek(lua_State L, IntPtr fn, string debugname, int nup, IntPtr cont);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushboolean(lua_State L, int b);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_pushthread(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_pushlightuserdatatagged(lua_State L, IntPtr p, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_newuserdatatagged(lua_State L, UIntPtr sz, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_newuserdatataggedwithmetatable(lua_State L, UIntPtr sz, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_newuserdatadtor(lua_State L, UIntPtr sz, IntPtr dtor);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_newbuffer(lua_State L, UIntPtr sz);
    #endregion

    #region Get functions (Lua -> stack)
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
    public static partial int lua_rawgetptagged(lua_State L, int idx, IntPtr p, int tag);

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
    #endregion

    #region Set functions (stack -> Lua)
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
    public static partial void lua_rawsetptagged(lua_State L, int idx, IntPtr p, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_setmetatable(lua_State L, int objindex);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_setfenv(lua_State L, int idx);
    #endregion

    #region Load and Call functions
    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int luau_load(lua_State L, string chunkname, IntPtr data, UIntPtr size, int env);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_call(lua_State L, int nargs, int nresults);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_pcall(lua_State L, int nargs, int nresults, int errfunc);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_cpcall(lua_State L, IntPtr func, IntPtr ud);
    #endregion

    #region Coroutine functions
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
    public static partial IntPtr lua_getthreaddata(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setthreaddata(lua_State L, IntPtr data);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_costatus(lua_State L, lua_State co);
    #endregion

    #region Garbage-collection
    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_gc(lua_State L, int what, int data);
    #endregion

    #region Memory statistics
    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setmemcat(lua_State L, int category);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial UIntPtr lua_totalbytes(lua_State L, int category);
    #endregion

    #region Miscellaneous functions
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
    public static partial UIntPtr lua_encodepointer(lua_State L, UIntPtr p);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial double lua_clock();

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setuserdatatag(lua_State L, int idx, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setuserdatadtor(lua_State L, int tag, IntPtr dtor);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_getuserdatadtor(lua_State L, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setuserdatametatable(lua_State L, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_getuserdatametatable(lua_State L, int tag);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_registeruserdatadirectaccess(lua_State L, int tag, IntPtr get, IntPtr set, IntPtr namecall);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_registeruserdatadirectfieldget(lua_State L, int tag, string field, IntPtr fn);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setnumber(IntPtr result, double n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setvector(IntPtr result, float x, float y, float z);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setboolean(IntPtr result, int b);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setinteger64(IntPtr result, long n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_userdatadirectfield_setnil(IntPtr result);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_setlightuserdataname(lua_State L, int tag, string name);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_getlightuserdataname(lua_State L, int tag);

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
    public static partial IntPtr lua_getallocf(lua_State L, out IntPtr ud);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_ref(lua_State L, int idx);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_unref(lua_State L, int refid);
    #endregion

    #region Debug API
    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_stackdepth(lua_State L);

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_getinfo(lua_State L, int level, string what, IntPtr ar);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_getargument(lua_State L, int level, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_getlocal(lua_State L, int level, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_setlocal(lua_State L, int level, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_getupvalue(lua_State L, int funcindex, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_setupvalue(lua_State L, int funcindex, int n);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_singlestep(lua_State L, int enabled);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int lua_breakpoint(lua_State L, int funcindex, int line, int enabled);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_getcoverage(lua_State L, int funcindex, IntPtr context, IntPtr callback);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_getcounters(lua_State L, int funcindex, IntPtr context, IntPtr functionvisit, IntPtr countervisit);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr lua_debugtrace(lua_State L);
    #endregion

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr luaL_newstate();

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luaL_openlibs(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr luau_compile(IntPtr source, UIntPtr size, IntPtr options, out UIntPtr outsize);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void lua_free(IntPtr ptr);
    #region Library functions

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
    #endregion

    #region Sandboxing functions
    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luaL_sandbox(lua_State L);

    [LibraryImport(LuauLibraryName)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luaL_sandboxthread(lua_State L);
    #endregion

    [LibraryImport(LuauLibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void luau_setfflag(string name, int value);
}
