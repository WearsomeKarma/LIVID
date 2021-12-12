
public static class StaticNoise
{
    public static NoiseMap Get_Noise_Map(Noise_Position cpos, int seed)
    {
        NoiseMap ret =
            new NoiseMap(Chunk_Noise_Table.CHUNK_SIZE);

        for(int chunk_z=0; chunk_z < Chunk_Noise_Table.CHUNK_SIZE; chunk_z++)
        {
            for(int chunk_x=0; chunk_x < Chunk_Noise_Table.CHUNK_SIZE; chunk_x++)
            {
                int x = 
                    cpos.NOISE_X * Chunk_Noise_Table.CHUNK_SIZE 
                    +
                    chunk_x;
                int z =
                    cpos.NOISE_Z * Chunk_Noise_Table.CHUNK_SIZE 
                    +
                    chunk_z;
                float noise = DariusPerlinNoise.Get_Noise(x, z, seed);

                ret[chunk_x, chunk_z] =
                    noise;
            }
        }

        return ret;
    }
}
