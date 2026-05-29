using LuauInterop.Objects;

namespace LuauInterop.Objects;

public sealed class LuauTable(Luau owner, int reference) : LuauBase(owner, reference)
{
    public object? this[string key]
    {
        get
        {
            ThrowIfDisposed();
            int stackBase = Owner.State.GetTop();
            try
            {
                PushReference();
                Owner.State.GetField(-1, key);
                return Owner.GetObject(-1);
            }
            finally
            {
                Owner.State.SetTop(stackBase);
            }
        }
        set
        {
            ThrowIfDisposed();
            int stackBase = Owner.State.GetTop();
            try
            {
                PushReference();
                Owner.PushObject(value);
                Owner.State.SetField(-2, key);
            }
            finally
            {
                Owner.State.SetTop(stackBase);
            }
        }
    }

    public object? this[int key]
    {
        get
        {
            ThrowIfDisposed();
            int stackBase = Owner.State.GetTop();
            try
            {
                PushReference();
                Owner.State.RawGetI(-1, key);
                return Owner.GetObject(-1);
            }
            finally
            {
                Owner.State.SetTop(stackBase);
            }
        }
        set
        {
            ThrowIfDisposed();
            int stackBase = Owner.State.GetTop();
            try
            {
                PushReference();
                Owner.PushObject(value);
                Owner.State.RawSetI(-2, key);
            }
            finally
            {
                Owner.State.SetTop(stackBase);
            }
        }
    }

    public int Length
    {
        get
        {
            ThrowIfDisposed();
            int stackBase = Owner.State.GetTop();
            try
            {
                PushReference();
                return Owner.State.ObjLen(-1);
            }
            finally
            {
                Owner.State.SetTop(stackBase);
            }
        }
    }

    public IEnumerable<KeyValuePair<object, object?>> Pairs()
    {
        ThrowIfDisposed();

        var results = new List<KeyValuePair<object, object?>>();

        int stackBase = Owner.State.GetTop();
        try
        {
            PushReference();
            Owner.State.PushNil();

            while (Owner.State.Next(-2) != 0)
            {
                object? key = Owner.GetObject(-2);
                object? value = Owner.GetObject(-1);

                if (key is not null)
                    results.Add(new KeyValuePair<object, object?>(key, value));

                Owner.Pop(1);
            }
        }
        finally
        {
            Owner.State.SetTop(stackBase);
        }

        return results;
    }
}