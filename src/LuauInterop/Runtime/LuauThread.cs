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
            return (LuauCoStatus)Owner.State.CoStatus(GetCoroutine());
        }
    }

    public void Close()
    {
        ThrowIfDisposed();
        GetCoroutine().ResetThread();
        Dispose();
    }

    public object?[] Resume(params object?[] args)
    {
        ThrowIfDisposed();
        
        if (Status == LuauCoStatus.Run)
            throw new LuauException("Cannot resume a running coroutine.");

        LuaState co = GetCoroutine();
        PushArgs(co, args);

        int status = co.Resume(Owner.State, args.Length);
        bool ok = status == (int)LuauStatus.OK || status == (int)LuauStatus.Yield;

        if (!ok)
            ThrowLastError(co);

        return Owner.CollectResults(0, co);
    }

    public object?[] Resume(LuauChunk chunk, params object?[] args)
    {
        ThrowIfDisposed();

        if (Status == LuauCoStatus.Run)
            throw new LuauException("Cannot resume a running coroutine.");

        LuaState co = GetCoroutine();

        LuauStatus loadStatus = (LuauStatus)Owner.State.Load("chunk", chunk.Pointer, chunk.Size, 0);
        if (loadStatus != LuauStatus.OK)
            Owner.ThrowLastError();

        Owner.State.XMove(co, 1);
        PushArgs(co, args);

        int status = co.Resume(Owner.State, args.Length);
        bool ok = status == (int)LuauStatus.OK || status == (int)LuauStatus.Yield;

        if (!ok)
            ThrowLastError(co);

        return Owner.CollectResults(0, co);
    }

    private void PushArgs(LuaState co, object?[] args)
    {
        foreach (var arg in args)
            Owner.PushObject(arg, co);
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

    private void ThrowLastError(LuaState coroutine)
    {
        throw new LuauException(Owner.GetErrorMessage(coroutine));
    }
}