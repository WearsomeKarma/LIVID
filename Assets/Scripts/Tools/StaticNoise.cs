
public static class StaticNoise
{
    public static NoiseMap Get_Noise_Map(Chunk_Position cpos, int seed)
    {
        NoiseMap ret =
            new NoiseMap(Chunk_Generator.CHUNK_VERTEX_SIZE);

        for(int chunk_z=0; chunk_z < Chunk_Generator.CHUNK_VERTEX_SIZE; chunk_z++)
        {
            for(int chunk_x=0; chunk_x < Chunk_Generator.CHUNK_VERTEX_SIZE; chunk_x++)
            {
                float noise = DariusPerlinNoise.Get_Noise(cpos.CHUNK_X + chunk_x, cpos.CHUNK_Z + chunk_z, seed);

                ret[chunk_x, chunk_z] =
                    noise;
            }
        }

        return ret;
    }
}
