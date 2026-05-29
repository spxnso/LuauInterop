namespace LuauInterop.Objects;

public sealed class LuauBuffer(byte[] data)
{
    public byte[] Data { get; } = data ?? throw new ArgumentNullException(nameof(data));

    public int Length => Data.Length;

    public override string ToString()
    {
        return $"Buffer[{Length}]";
    }
}
