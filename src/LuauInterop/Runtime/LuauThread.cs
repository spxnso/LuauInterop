using System.Runtime.InteropServices;

using LuauInterop.Native;

namespace LuauInterop.Runtime;

public sealed class LuauThread(Luau owner, LuaState state, int reference) : LuauBase(owner, state, reference)
{
    public LuauCoStatus Status
    {
        get
        {
            ThrowIfDisposed();
            return (LuauCoStatus)State.CoStatus(GetCoroutine());
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

        int status = co.Resume(State, args.Length);
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

        LuauStatus loadStatus = (LuauStatus)State.Load("chunk", chunk.Pointer, chunk.Size, 0);
        if (loadStatus != LuauStatus.OK)
            Owner.ThrowLastError(State);

        State.XMove(co, 1);
        PushArgs(co, args);

        LuauStatus status = (LuauStatus)co.Resume(State, args.Length);
        bool ok = status == LuauStatus.OK || status == LuauStatus.Yield;

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
        int top = State.GetTop();
        PushReference();
        LuaState coroutine = State.ToThread(-1);
        State.SetTop(top);

        if (coroutine.IsNull)
            throw new InvalidOperationException("Reference is not a valid Lua thread.");

        return coroutine;
    }

    private void ThrowLastError(LuaState coroutine)
    {
        throw new LuauException(Owner.GetErrorMessage(coroutine));
    }
}