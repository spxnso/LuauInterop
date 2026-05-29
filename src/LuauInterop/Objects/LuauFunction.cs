using LuauInterop.Runtime;

namespace LuauInterop.Objects;

public sealed class LuauFunction(Luau owner, int reference) : LuauBase(owner, reference)
{
    public object?[] Call(params object?[] arguments)
    {
        ThrowIfDisposed();

        int stackBase = Owner.State.GetTop();
        try
        {
            PushReference();

            int argCount = 0;
            if (arguments is not null)
            {
                foreach (var arg in arguments)
                    Owner.PushObject(arg);
                argCount = arguments.Length;
            }

            LuauStatus callStatus = (LuauStatus)Owner.State.PCall(argCount, -1, 0);
            if (callStatus != LuauStatus.OK)
            {
                string error = Owner.GetErrorMessage();
                throw new LuauException(error);
            }

            int resultCount = Owner.State.GetTop() - stackBase;
            var results = new List<object?>();

            try
            {
                for (int i = 0; i < resultCount; i++)
                    results.Add(Owner.GetObject(stackBase + i + 1));
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
            Owner.State.SetTop(stackBase);
        }
    }
}