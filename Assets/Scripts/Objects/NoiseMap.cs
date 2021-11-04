public sealed class NoiseMap
{
    private double[,] _NOISE_MAP { get; }
    public int SIZE { get; }

    public NoiseMap(int size)
    {
        SIZE = size;
        _NOISE_MAP = new double[size,size];
    }

    public double this[int indexCol, int indexRow]
    {
        get => _NOISE_MAP[indexCol, indexRow];
        set => _NOISE_MAP[indexCol, indexRow] = value;
    }
}
