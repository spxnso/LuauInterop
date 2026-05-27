using Luau.Native;

namespace Luau
{
    public abstract class LuauBase(Luau owner, int reference) : IDisposable
    {
        public Luau Owner { get; } = owner;
        public int Reference { get; } = reference;

        public bool IsDisposed { get; private set; }

        ~LuauBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (!Owner.State.IsNull)
                Owner.State.Unref(Reference);

            IsDisposed = true;
        }

        protected void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(
                IsDisposed || Owner.State.IsNull,
                GetType().Name
            );
        }

        internal protected void PushReference()
        {
            ThrowIfDisposed();
            Owner.State.RawGetI(LuaConstants.LUA_REGISTRYINDEX, Reference);
        }
    }
}
