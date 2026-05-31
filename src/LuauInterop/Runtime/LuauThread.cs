using System.Runtime.InteropServices;

using LuauInterop.Native;

namespace LuauInterop.Runtime;

public sealed class LuauThread(Luau owner, LuaState coroutineState, LuaState state, int reference) : LuauBase(owner, state, reference)
{
    /// <summary>
    /// The <see cref="LuaState"/> of this thread.
    /// </summary>
    public LuaState CoroutineState = coroutineState;

    public LuauCoStatus Status
    {
        get
        {
            ThrowIfDisposed();
            return (LuauCoStatus)State.CoStatus(CoroutineState);
        }
    }

    public void Close()
    {
        ThrowIfDisposed();
        CoroutineState.ResetThread();
        Dispose();
    }

    public object?[] Resume(params object?[] args)
    {
        ThrowIfDisposed();

        if (Status == LuauCoStatus.Run)
            throw new LuauException("Cannot resume a running coroutine.");

        PushArgs(CoroutineState, args);

        int status = CoroutineState.Resume(State, args.Length);
        bool ok = status == (int)LuauStatus.OK || status == (int)LuauStatus.Yield;

        if (!ok)
            ThrowLastError(CoroutineState);

        return Owner.CollectResults(0, CoroutineState);
    }

    public object?[] Resume(LuauChunk chunk, params object?[] args)
    {
        ThrowIfDisposed();

        if (Status == LuauCoStatus.Run)
            throw new LuauException("Cannot resume a running coroutine.");

        LuauStatus loadStatus = (LuauStatus)State.Load("chunk", chunk.Pointer, chunk.Size, 0);
        if (loadStatus != LuauStatus.OK)
            Owner.ThrowLastError(State);

        State.XMove(CoroutineState, 1);
        PushArgs(CoroutineState, args);

        LuauStatus status = (LuauStatus)CoroutineState.Resume(State, args.Length);
        bool ok = status == LuauStatus.OK || status == LuauStatus.Yield;

        if (!ok)
            ThrowLastError(CoroutineState);

        return Owner.CollectResults(0, CoroutineState);
    }

    private void PushArgs(LuaState co, object?[] args)
    {
        foreach (var arg in args)
            Owner.PushObject(arg, co);
    }

    private void ThrowLastError(LuaState coroutine)
    {
        throw new LuauException(Owner.GetErrorMessage(coroutine));
    }
}