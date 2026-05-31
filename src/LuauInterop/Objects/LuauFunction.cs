using LuauInterop.Native;
using LuauInterop.Runtime;

namespace LuauInterop.Objects;

public sealed class LuauFunction(Luau owner, LuaState state, int reference) : LuauBase(owner, state, reference)
{
    /// <summary>
    /// Calls the Lua function with the specified arguments and returns the results.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the Lua function.</param>
    /// <returns>An array of results returned by the Lua function.</returns>
    /// <exception cref="LuauException">Thrown if the Lua function call results in an error.</exception>
    /// <exception cref="ObjectDisposedException">Thrown if this function or its owner has been disposed.</exception>
    public object?[] Call(params object?[] arguments)
    {
        ThrowIfDisposed();

        int stackBase = State.GetTop();
        try
        {
            PushReference();

            int argCount = 0;
            if (arguments is not null)
            {
                foreach (var arg in arguments)
                    Owner.PushObject(arg, State);
                argCount = arguments.Length;
            }

            LuauStatus callStatus = (LuauStatus)State.PCall(argCount, -1, 0);

            if (callStatus != LuauStatus.OK && LuauCallback.PendingException is Exception ex)
            {
                // clear error message
                Owner.GetErrorMessage(State);

                // throw the pending exception instead of the Lua error
                // This allows C# exceptions to propagate properly through Lua callbacks
                LuauCallback.PendingException = null;
                throw ex;
            }
            else if (callStatus != LuauStatus.OK)
            {
                string error = Owner.GetErrorMessage(State);
                throw new LuauException(error);
            }

            int resultCount = State.GetTop() - stackBase;
            var results = new List<object?>(resultCount);

            try
            {
                for (int i = 0; i < resultCount; i++)
                    results.Add(Owner.GetObject(stackBase + i + 1, State));
            }
            catch
            {
                foreach (var r in results.OfType<IDisposable>())
                    r.Dispose();
                throw;
            }

            return [.. results];
        }
        finally
        {
            State.SetTop(stackBase);
        }
    }
}