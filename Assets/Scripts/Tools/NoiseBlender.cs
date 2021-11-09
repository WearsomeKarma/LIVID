
/// <summary>
/// Here we treat noisemaps as 2D waves
/// and perform constructive/destructive
/// wave interaction, then we floor it to
/// get biome regions.
/// </summary>
public static class NoiseBlender
{
    public static NoiseMap Blend(NoiseMap[] maps, double peak)
    {
        NoiseMap blendMap = new NoiseMap(maps[0].SIZE);
        NoiseMap previousMap = maps[0];

        Copy(previousMap, blendMap);

        for(int i = 1;i<maps.Length;i++)
        {
            Blend(maps[i], blendMap, peak);
        }

        return blendMap;
    }

    public static void Copy(NoiseMap source, NoiseMap target)
    {
        for(int chunk_z=0;chunk_z<source.SIZE;chunk_z++)
        {
            for(int chunk_x=0;chunk_x<source.SIZE;chunk_x++)
            {
                target[chunk_x, chunk_z] = target[chunk_x, chunk_z];
            }
        }
    }

    public static void Blend(NoiseMap source, NoiseMap target, double peak)
    {
        for(int chunk_z = 0; chunk_z < target.SIZE; chunk_z++)
        {
            for(int chunk_x = 0; chunk_x < target.SIZE; chunk_x++)
            {
                double diff =
                    target[chunk_x, chunk_z] 
                    -
                    source[chunk_x, chunk_z];

                double val = target[chunk_x, chunk_z] - diff;
                val = (val < 0) ? 0 : ((val > peak) ? peak : val);
                target[chunk_x, chunk_z] = val;
            }
        }
    }
}
