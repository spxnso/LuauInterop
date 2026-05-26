namespace Luau.Native
{
    public readonly struct LuaState(IntPtr handle)
    {
        public readonly IntPtr Handle = handle;

        public static implicit operator IntPtr(LuaState state) => state.Handle;
        public static implicit operator LuaState(IntPtr handle) => new(handle);

        public bool IsNull => Handle == IntPtr.Zero;

        #region State manipulation
        public void Close()
        {
            if (!IsNull)
            {
                NativeMethods.lua_close(Handle);
            }
        }

        public LuaState NewThread()
        {
            return new LuaState(NativeMethods.lua_newthread(Handle));
        }
        
        public LuaState MainThread()
        {
            return new LuaState(NativeMethods.lua_mainthread(Handle));
        }

        public void ResetThread()
        {
            NativeMethods.lua_resetthread(Handle);
        }

        public bool IsThreadReset()
        {
            return NativeMethods.lua_isthreadreset(Handle) != 0;
        }
        #endregion 

        #region Basic stack manipulation
        public int AbsIndex(int idx)
        {
            return NativeMethods.lua_absindex(Handle, idx);
        }

        public int GetTop()
        {
            return NativeMethods.lua_gettop(Handle);
        }

        public void SetTop(int idx)
        {
            NativeMethods.lua_settop(Handle, idx);
        }

        public void PushValue(int idx)
        {
            NativeMethods.lua_pushvalue(Handle, idx);
        }

        public void Remove(int idx)
        {
            NativeMethods.lua_remove(Handle, idx);
        }

        public void Insert(int idx)
        {
            NativeMethods.lua_insert(Handle, idx);
        }

        public void Replace(int idx)
        {
            NativeMethods.lua_replace(Handle, idx);
        }
        public int CheckStack(int sz)
        {
            return NativeMethods.lua_checkstack(Handle, sz);
        }

        public void RawCheckStack(int extra, string? message = null)
        {
            NativeMethods.lua_rawcheckstack(Handle, extra);
        }

        public void XMove(LuaState to, int n)
        {
            NativeMethods.lua_xmove(Handle, to.Handle, n);
        }

        public void XPush(LuaState to, int idx)
        {
            NativeMethods.lua_xpush(Handle, to.Handle, idx);
        }
        #endregion

        #region Access functions (stack -> C)
        public bool IsNumber(int idx) => NativeMethods.lua_isnumber(Handle, idx) != 0;
        public bool IsString(int idx) => NativeMethods.lua_isstring(Handle, idx) != 0;
        public bool IsInteger64(int idx) => NativeMethods.lua_isinteger64(Handle, idx) != 0;
        public bool IsCFunction(int idx) => NativeMethods.lua_iscfunction(Handle, idx) != 0;
        public bool IsLFunction(int idx) => NativeMethods.lua_isLfunction(Handle, idx) != 0;
        public bool IsUserdata(int idx) => NativeMethods.lua_isuserdata(Handle, idx) != 0;
        public int Type(int idx) => NativeMethods.lua_type(Handle, idx);
        public IntPtr TypeName(int tp) => NativeMethods.lua_typename(Handle, tp);
        public bool Equal(int idx1, int idx2) => NativeMethods.lua_equal(Handle, idx1, idx2) != 0;
        public bool RawEqual(int idx1, int idx2) => NativeMethods.lua_rawequal(Handle, idx1, idx2) != 0;
        public bool LessThan(int idx1, int idx2) => NativeMethods.lua_lessthan(Handle, idx1, idx2) != 0;
        
        public double ToNumberX(int idx, out bool isnum)
        {
            double res = NativeMethods.lua_tonumberx(Handle, idx, out int is_num);
            isnum = is_num != 0;
            return res;
        }

        public int ToIntegerX(int idx, out bool isnum)
        {
            int res = NativeMethods.lua_tointegerx(Handle, idx, out int is_num);
            isnum = is_num != 0;
            return res;
        }

        public uint ToUnsignedX(int idx, out bool isnum)
        {
            uint res = NativeMethods.lua_tounsignedx(Handle, idx, out int is_num);
            isnum = is_num != 0;
            return res;
        }

        public IntPtr ToVector(int idx) => NativeMethods.lua_tovector(Handle, idx);
        public bool ToBoolean(int idx) => NativeMethods.lua_toboolean(Handle, idx) != 0;

        public long ToInteger64(int idx, out bool isinteger)
        {
            long res = NativeMethods.lua_tointeger64(Handle, idx, out int is_int);
            isinteger = is_int != 0;
            return res;
        }

        public IntPtr ToLString(int idx, out UIntPtr len) => NativeMethods.lua_tolstring(Handle, idx, out len);
        public IntPtr ToStringAtom(int idx, out int atom) => NativeMethods.lua_tostringatom(Handle, idx, out atom);
        public IntPtr ToLStringAtom(int idx, out UIntPtr len, out int atom) => NativeMethods.lua_tolstringatom(Handle, idx, out len, out atom);
        public IntPtr NameCallAtom(out int atom) => NativeMethods.lua_namecallatom(Handle, out atom);

        public int ObjLen(int idx) => NativeMethods.lua_objlen(Handle, idx);
        public IntPtr ToCFunction(int idx) => NativeMethods.lua_tocfunction(Handle, idx);
        public IntPtr ToLightUserdata(int idx) => NativeMethods.lua_tolightuserdata(Handle, idx);
        public IntPtr ToLightUserdataTagged(int idx, int tag) => NativeMethods.lua_tolightuserdatatagged(Handle, idx, tag);
        public IntPtr ToUserdata(int idx) => NativeMethods.lua_touserdata(Handle, idx);
        public IntPtr ToUserdataTagged(int idx, int tag) => NativeMethods.lua_touserdatatagged(Handle, idx, tag);
        
        public int UserdataTag(int idx) => NativeMethods.lua_userdatatag(Handle, idx);
        public int LightUserdataTag(int idx) => NativeMethods.lua_lightuserdatatag(Handle, idx);
        public LuaState ToThread(int idx) => new LuaState(NativeMethods.lua_tothread(Handle, idx));
        public IntPtr ToBuffer(int idx, out UIntPtr len) => NativeMethods.lua_tobuffer(Handle, idx, out len);
        public IntPtr ToPointer(int idx) => NativeMethods.lua_topointer(Handle, idx);
        #endregion

        #region Push functions
        public void PushNil() => NativeMethods.lua_pushnil(Handle);
        public void PushNumber(double n) => NativeMethods.lua_pushnumber(Handle, n);
        public void PushInteger(int n) => NativeMethods.lua_pushinteger(Handle, n);
        public void PushInteger64(long n) => NativeMethods.lua_pushinteger64(Handle, n);
        public void PushUnsigned(uint n) => NativeMethods.lua_pushunsigned(Handle, n);
        public void PushVector(float x, float y, float z) => NativeMethods.lua_pushvector(Handle, x, y, z);
        public void PushLString(string s, UIntPtr l) => NativeMethods.lua_pushlstring(Handle, s, l);
        public void PushString(string s) => NativeMethods.lua_pushstring(Handle, s);
        public IntPtr PushVFString(string fmt, IntPtr argp) => NativeMethods.lua_pushvfstring(Handle, fmt, argp);
        public void PushCClosureK(IntPtr fn, string debugname, int nup, IntPtr cont) => NativeMethods.lua_pushcclosurek(Handle, fn, debugname, nup, cont);
        public void PushBoolean(bool b) => NativeMethods.lua_pushboolean(Handle, b ? 1 : 0);
        public int PushThread() => NativeMethods.lua_pushthread(Handle);
        public void PushLightUserdataTagged(IntPtr p, int tag) => NativeMethods.lua_pushlightuserdatatagged(Handle, p, tag);
        public IntPtr NewUserdataTagged(UIntPtr sz, int tag) => NativeMethods.lua_newuserdatatagged(Handle, sz, tag);
        public IntPtr NewUserdataTaggedWithMetatable(UIntPtr sz, int tag) => NativeMethods.lua_newuserdatataggedwithmetatable(Handle, sz, tag);
        public IntPtr NewUserdataDtor(UIntPtr sz, IntPtr dtor) => NativeMethods.lua_newuserdatadtor(Handle, sz, dtor);
        public IntPtr NewBuffer(UIntPtr sz) => NativeMethods.lua_newbuffer(Handle, sz);
        #endregion

        #region Get functions
        public int GetTable(int idx) => NativeMethods.lua_gettable(Handle, idx);
        public int GetField(int idx, string k) => NativeMethods.lua_getfield(Handle, idx, k);
        public int RawGetField(int idx, string k) => NativeMethods.lua_rawgetfield(Handle, idx, k);
        public int RawGet(int idx) => NativeMethods.lua_rawget(Handle, idx);
        public int RawGetI(int idx, int n) => NativeMethods.lua_rawgeti(Handle, idx, n);
        public int RawGetPTagged(int idx, IntPtr p, int tag) => NativeMethods.lua_rawgetptagged(Handle, idx, p, tag);
        public void CreateTable(int narr, int nrec) => NativeMethods.lua_createtable(Handle, narr, nrec);
        public void SetReadonly(int idx, bool enabled) => NativeMethods.lua_setreadonly(Handle, idx, enabled ? 1 : 0);
        public bool GetReadonly(int idx) => NativeMethods.lua_getreadonly(Handle, idx) != 0;
        public void SetSafeEnv(int idx, bool enabled) => NativeMethods.lua_setsafeenv(Handle, idx, enabled ? 1 : 0);
        public int GetMetatable(int objindex) => NativeMethods.lua_getmetatable(Handle, objindex);
        public void GetFEnv(int idx) => NativeMethods.lua_getfenv(Handle, idx);
        #endregion

        #region Set functions
        public void SetTable(int idx) => NativeMethods.lua_settable(Handle, idx);
        public void SetField(int idx, string k) => NativeMethods.lua_setfield(Handle, idx, k);
        public void RawSetField(int idx, string k) => NativeMethods.lua_rawsetfield(Handle, idx, k);
        public void RawSet(int idx) => NativeMethods.lua_rawset(Handle, idx);
        public void RawSetI(int idx, int n) => NativeMethods.lua_rawseti(Handle, idx, n);
        public void RawSetPTagged(int idx, IntPtr p, int tag) => NativeMethods.lua_rawsetptagged(Handle, idx, p, tag);
        public int SetMetatable(int objindex) => NativeMethods.lua_setmetatable(Handle, objindex);
        public int SetFEnv(int idx) => NativeMethods.lua_setfenv(Handle, idx);
        #endregion

        #region Load and call
        public int Load(string chunkname, IntPtr data, UIntPtr size, int env) => NativeMethods.luau_load(Handle, chunkname, data, size, env);
        public void Call(int nargs, int nresults) => NativeMethods.lua_call(Handle, nargs, nresults);
        public int PCall(int nargs, int nresults, int errfunc) => NativeMethods.lua_pcall(Handle, nargs, nresults, errfunc);
        public int CPCall(IntPtr func, IntPtr ud) => NativeMethods.lua_cpcall(Handle, func, ud);
        #endregion

        #region Coroutines
        public int Yield(int nresults) => NativeMethods.lua_yield(Handle, nresults);
        public int Break() => NativeMethods.lua_break(Handle);
        public int Resume(LuaState from, int narg) => NativeMethods.lua_resume(Handle, from.Handle, narg);
        public int ResumeError(LuaState from) => NativeMethods.lua_resumeerror(Handle, from.Handle);
        public int Status() => NativeMethods.lua_status(Handle);
        public bool IsYieldable() => NativeMethods.lua_isyieldable(Handle) != 0;
        public IntPtr GetThreadData() => NativeMethods.lua_getthreaddata(Handle);
        public void SetThreadData(IntPtr data) => NativeMethods.lua_setthreaddata(Handle, data);
        public int CoStatus(LuaState co) => NativeMethods.lua_costatus(Handle, co.Handle);
        #endregion

        #region GC and Memory
        public int GC(int what, int data) => NativeMethods.lua_gc(Handle, what, data);
        public void SetMemCat(int category) => NativeMethods.lua_setmemcat(Handle, category);
        public UIntPtr TotalBytes(int category) => NativeMethods.lua_totalbytes(Handle, category);
        #endregion

        #region Miscellaneous
        public void Error() => NativeMethods.lua_error(Handle);
        public int Next(int idx) => NativeMethods.lua_next(Handle, idx);
        public int RawIter(int idx, int iter) => NativeMethods.lua_rawiter(Handle, idx, iter);
        public void Concat(int n) => NativeMethods.lua_concat(Handle, n);
        public UIntPtr EncodePointer(UIntPtr p) => NativeMethods.lua_encodepointer(Handle, p);
        public void SetUserdataTag(int idx, int tag) => NativeMethods.lua_setuserdatatag(Handle, idx, tag);
        public void SetUserdataDtor(int tag, IntPtr dtor) => NativeMethods.lua_setuserdatadtor(Handle, tag, dtor);
        public IntPtr GetUserdataDtor(int tag) => NativeMethods.lua_getuserdatadtor(Handle, tag);
        public void SetUserdataMetatable(int tag) => NativeMethods.lua_setuserdatametatable(Handle, tag);
        public void GetUserdataMetatable(int tag) => NativeMethods.lua_getuserdatametatable(Handle, tag);
        public int RegisterUserdataDirectAccess(int tag, IntPtr get, IntPtr set, IntPtr namecall) => NativeMethods.lua_registeruserdatadirectaccess(Handle, tag, get, set, namecall);
        public void RegisterUserdataDirectFieldGet(int tag, string field, IntPtr fn) => NativeMethods.lua_registeruserdatadirectfieldget(Handle, tag, field, fn);
        public void SetLightUserdataName(int tag, string name) => NativeMethods.lua_setlightuserdataname(Handle, tag, name);
        public IntPtr GetLightUserdataName(int tag) => NativeMethods.lua_getlightuserdataname(Handle, tag);
        public void CloneFunction(int idx) => NativeMethods.lua_clonefunction(Handle, idx);
        public void ClearTable(int idx) => NativeMethods.lua_cleartable(Handle, idx);
        public void CloneTable(int idx) => NativeMethods.lua_clonetable(Handle, idx);
        public IntPtr GetAllocF(out IntPtr ud) => NativeMethods.lua_getallocf(Handle, out ud);
        public int Ref(int idx) => NativeMethods.lua_ref(Handle, idx);
        public void Unref(int refid) => NativeMethods.lua_unref(Handle, refid);
        #endregion

        #region Library functions
        public void OpenLibraries()
        {
            NativeMethods.luaL_openlibs(Handle);
        }

        public int OpenBase() => NativeMethods.luaopen_base(Handle);
        public int OpenCoroutine() => NativeMethods.luaopen_coroutine(Handle);
        public int OpenTable() => NativeMethods.luaopen_table(Handle);
        public int OpenOS() => NativeMethods.luaopen_os(Handle);
        public int OpenString() => NativeMethods.luaopen_string(Handle);
        public int OpenBit32() => NativeMethods.luaopen_bit32(Handle);
        public int OpenBuffer() => NativeMethods.luaopen_buffer(Handle);
        public int OpenUtf8() => NativeMethods.luaopen_utf8(Handle);
        public int OpenMath() => NativeMethods.luaopen_math(Handle);
        public int OpenDebug() => NativeMethods.luaopen_debug(Handle);
        public int OpenVector() => NativeMethods.luaopen_vector(Handle);
        public int OpenInteger() => NativeMethods.luaopen_integer(Handle);
        #endregion
        
        #region Debug API
        public int StackDepth() => NativeMethods.lua_stackdepth(Handle);
        public int GetInfo(int level, string what, IntPtr ar) => NativeMethods.lua_getinfo(Handle, level, what, ar);
        public int GetArgument(int level, int n) => NativeMethods.lua_getargument(Handle, level, n);
        public IntPtr GetLocal(int level, int n) => NativeMethods.lua_getlocal(Handle, level, n);
        public IntPtr SetLocal(int level, int n) => NativeMethods.lua_setlocal(Handle, level, n);
        public IntPtr GetUpvalue(int funcindex, int n) => NativeMethods.lua_getupvalue(Handle, funcindex, n);
        public IntPtr SetUpvalue(int funcindex, int n) => NativeMethods.lua_setupvalue(Handle, funcindex, n);
        public void SingleStep(bool enabled) => NativeMethods.lua_singlestep(Handle, enabled ? 1 : 0);
        public int Breakpoint(int funcindex, int line, bool enabled) => NativeMethods.lua_breakpoint(Handle, funcindex, line, enabled ? 1 : 0);
        public void GetCoverage(int funcindex, IntPtr context, IntPtr callback) => NativeMethods.lua_getcoverage(Handle, funcindex, context, callback);
        public void GetCounters(int funcindex, IntPtr context, IntPtr functionvisit, IntPtr countervisit) => NativeMethods.lua_getcounters(Handle, funcindex, context, functionvisit, countervisit);
        public IntPtr DebugTrace() => NativeMethods.lua_debugtrace(Handle);
        #endregion
    }
}