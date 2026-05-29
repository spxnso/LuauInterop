using System.Runtime.InteropServices;
using LuauInterop.Native;

namespace LuauInterop.Runtime;

public sealed class LuauThread(Luau owner, int reference) : LuauBase(owner, reference)
{
    public LuauCoStatus Status
    {
        get
        {
            ThrowIfDisposed();
            LuaState coroutine = GetCoroutine();
            return (LuauCoStatus)Owner.State.CoStatus(coroutine);
        }
    }

    public void Close()
    {
        ThrowIfDisposed();
        GetCoroutine().Close();
    }

    public object?[] Resume(params object?[] args)
    {
        ThrowIfDisposed();

        LuaState coroutine = GetCoroutine();
        PushArgs(coroutine, args);

        int status = coroutine.Resume(Owner.State, args.Length);
        bool ok = status == (int)LuauStatus.OK || status == (int)LuauStatus.Yield;

        if (!ok)
        {
            ThrowLastError(coroutine);
        }

        int resultCount = coroutine.GetTop();
        object?[] results = new object?[resultCount];

        for (int i = 0; i < resultCount; i++)
            results[i] = Owner.GetObject(i + 1, coroutine);

        coroutine.SetTop(0);
        return results;
    }


    private void PushArgs(LuaState co, object?[] args)
    {
        if (args.Length == 0) return;

        foreach (var arg in args)
            Owner.PushObject(arg);

        Owner.State.XMove(co, args.Length);
    }


    private LuaState GetCoroutine()
    {
        int top = Owner.State.GetTop();
        PushReference();
        LuaState coroutine = Owner.State.ToThread(-1);
        Owner.State.SetTop(top);

        if (coroutine.IsNull)
            throw new InvalidOperationException("Reference is not a valid Lua thread.");

        return coroutine;
    }

    private static string GetErrorMessage(LuaState coroutine)
    {
        IntPtr ptr = coroutine.ToLString(-1, out _);
        return ptr != IntPtr.Zero ? Marshal.PtrToStringUTF8(ptr) ?? "Unknown coroutine error" : "Unknown coroutine error";
    }

    private void ThrowLastError(LuaState coroutine)
    {
        string message = GetErrorMessage(coroutine);
        coroutine.SetTop(0);
        throw new LuauException(message);
    }
}
