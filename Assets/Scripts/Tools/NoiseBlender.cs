
/// <summary>
/// Here we treat noisemaps as 2D waves
/// and perform constructive/destructive
/// wave interaction, then we floor it to
/// get biome regions.
/// </summary>
public static class NoiseBlender
{
    public static double Get_World_Elevation_Weight(double gradiant, double peak=1)
    {
        // Lowlands weight.
        if (gradiant < 0.333)
        {
            return 
                (
                    0.33 -
                    (
                        0.33 *
                        System.Math.Sqrt
                        (
                            peak -
                            System.Math.Pow
                            (
                                3 * gradiant,
                                2
                            )
                        )
                    )
                );
        }
        // Highlands weight.
        if (gradiant < 0.75 * peak)
        {
            return 
                (
                    0.32 +
                    (
                        0.1 *
                        System.Math.Sqrt
                        (
                            peak -
                            System.Math.Pow
                            (
                                3 *
                                (gradiant - 0.666*peak),
                                2
                            )
                        )
                    )
                );
        }

        // Mountain weight.
        return
            (
                1 -
                (
                    0.58 *
                    System.Math.Sqrt
                    (
                        peak -
                        System.Math.Pow
                        (
                            4 * (gradiant - (0.75*peak)),
                            2
                        )
                    )
                )
            );
    }

    public static double Expand_Biome_Weight(double biomeGradiant, double biomeWeight)
    {
        //\left(\frac{1}{\left(1.6-x\right)}-0.6\right)\cdot0.5
        double pow = 1 + (1/(1.6-biomeGradiant)-0.6) * 0.5;
        return System.Math.Pow(biomeWeight, pow);
    }

    public static double Blend_Height__Chunk_And_Biome
    (
        double heightChunk, 
        double weightChunk, 
        double heightGradiantBiome,
        double weightBiome
    )
    {
        double heightBiome =
            Get_World_Elevation_Weight(heightGradiantBiome) * Expand_Biome_Weight(heightGradiantBiome, weightBiome);

        return (heightChunk * weightChunk) + heightBiome;
    }

    public static double Moderate_By_Height
    (
        double attributeWeight, 
        double worldElevationGradiant, 
        double peak_attribute=1,
        double peak_world=1
    )
    {
        return attributeWeight;
        double peakDelta = peak_attribute - attributeWeight;

        //If we are high on the gradient
        //then we will moderate the weight
        //towards 0.
        double highModeration = Moderate(1,worldElevationGradiant,1.3);

        //If we are low on the gradient
        //then we will moderate the weight
        //towards the peak.
        double lowModeration = Moderate(1,worldElevationGradiant - peak_world, 1.3);

        double considerHigh = attributeWeight * highModeration;
        double considerLow  = peakDelta * lowModeration;

        double moderation = attributeWeight + considerLow - considerHigh;

        return moderation;
    }

    private static double Moderate
    (
        double amplitude,
        double gradient,
        double curvingPower
    )
    {
        double semiCircle = - System.Math.Sqrt(1-gradient*gradient) + 1;
        double curved = System.Math.Pow(semiCircle, curvingPower);

        return curved;
    }

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
