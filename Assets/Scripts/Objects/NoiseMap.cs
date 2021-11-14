public sealed class NoiseMap : Spatial_Element 
{
    private double[,] _NOISE_MAP { get; }
    public int SIZE { get; }

    public NoiseMap(int size)
    {
        SIZE = size;
        _NOISE_MAP = new double[size,size];
    }

    public double this[Noise_Position position]
    {
        get => this[position.NOISE_X, position.NOISE_Z];
        set => this[position.NOISE_X, position.NOISE_Z] = value;
    }

    public double this[int x, int z]
    {
        get => _NOISE_MAP[x, z];
        set => _NOISE_MAP[x, z] = value;
    }
}
