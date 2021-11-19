
public sealed class World_Noise_Table :
    Spatial_Table<NoiseMap>
{
    public const int WORLD_NOISE_SCALE
        = 128;

    private int SEED { get; }

    public World_Noise_Table
    (
        int seed,
        int tableSize, 
        Noise_Position initalPosition,
        Noise_Position? offset = null
    ) 
    : base
    (
        tableSize, 
        initalPosition, 
        WORLD_NOISE_SCALE,
        offset: offset
    )
    {
        SEED = seed;

        Element_Generator =
            Generate_Elevation_Map;
    }

    private NoiseMap Generate_Elevation_Map(Noise_Position position)
    {
        NoiseMap elevation_Map = 
            DariusPerlinNoise
            .Get_Noise_Map(position, 2, WORLD_NOISE_SCALE, SEED);

        return elevation_Map;
    }

    internal override NoiseMap this[Noise_Position position]
    {
        get 
        {
            NoiseMap nm = base[position];

            if (nm == null)
            {
                foreach(NoiseMap map in Element_Table)
                    UnityEngine.Debug.Log($"Invalid at {position} I have: {map.Spatial_Position}");
            }

            return nm;
        }
    }
}
