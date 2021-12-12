
using System;

public sealed class Chunk_Noise_Table : Spatial_Table<NoiseMap>
{
    public const int CHUNK_SIZE = 16;

    private int SEED { get; }

    public Chunk_Noise_Table
    (
        int seed,
        int tableSize, 
        Noise_Position initalPosition, 
        Func<Noise_Position, NoiseMap> generator = null
    )
    : 
    base
    (
        tableSize, 
        initalPosition, 
        CHUNK_SIZE
    )
    {
        Element_Generator =
            generator
            ??
            Generate_Chunk;
    }

    private NoiseMap Generate_Chunk(Noise_Position position)
    {
        NoiseMap noiseMap =
            DariusPerlinNoise
            .Get_Noise_Map(position, 2, CHUNK_SIZE, SEED);

        return noiseMap;
    }
}
