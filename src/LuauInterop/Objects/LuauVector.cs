namespace LuauInterop.Objects;

public readonly struct LuauVector(float x, float y, float z)
{
    public float X { get; } = x;
    public float Y { get; } = y;
    public float Z { get; } = z;

    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }
}
