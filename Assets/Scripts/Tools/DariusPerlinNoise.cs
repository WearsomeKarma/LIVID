using UnityEngine;

public static class DariusPerlinNoise
{
    public static NoiseMap Get_Noise_Map
    (
        Noise_Position chunk_Position, 
        int frequency, 
        int size,
        int seed, 
        double peak = 1
    )
    {
        NoiseMap ret = 
            new NoiseMap(size);

        int stride = size;

        Vector2 chunkTilePos = 
            new Vector2(chunk_Position.NOISE_X, chunk_Position.NOISE_Z) 
            * 
            size;

        Vector2 strideOffset;

        //interprolation values for stride regions.
        float h0, h1, h2, h3;
        //Currently we end up recalucating these A LOT... will work to fix that later on.


        for (int period = 0; period < frequency; period++)
        {
            for (int x_stride = 0; x_stride * stride < size; x_stride++)
            {
                for (int z_stride = 0; z_stride * stride < size; z_stride++)
                {
                    strideOffset = chunkTilePos + new Vector2(x_stride * stride, z_stride * stride);

                    // get region h corners.
                    h0 = Get_Noise((int) strideOffset.x, (int) strideOffset.y, seed);
                    h1 = Get_Noise((int) strideOffset.x + stride, (int) strideOffset.y, seed);
                    h2 = Get_Noise((int) strideOffset.x, (int) strideOffset.y + stride, seed);
                    h3 = Get_Noise((int) strideOffset.x + stride, (int) strideOffset.y + stride, seed);

                    for (int x = stride * x_stride; x < (x_stride + 1) * stride; x++)
                    {
                        for (int z = z_stride * stride; z < (z_stride + 1) * stride; z++)
                        {
                            ret[x, z] += (Get_Weight(x % stride, z % stride, stride, h0, h1, h2, h3)) / frequency;
                        }
                    }
                }
            }

            stride /= 2;
        }

        return ret;
    }

    private static float Get_Weight(float x, float z, int stride, float h0, float h1, float h2, float h3)
    {
        float xw = (x) / (stride - 1);
        float zw = (z) / (stride - 1);

        float xw_1 = (1-xw);
        float zw_1 = (1-zw);

        float weight_h0 = xw_1 * zw_1 * h0;
        float weight_h1 = xw * zw_1 * h1;
        float weight_h2 = xw_1 * zw * h2;
        float weight_h3 = xw * zw * h3;

        return weight_h0 + weight_h1 + weight_h2 + weight_h3;

        //Lerping function
        //return ((1f - zw) * (h0 + (xw * (h1 - h0)))) + (zw * (h2 + (xw * (h3 - h2))));
    }

    public static float Get_Noise(int x, int z, int seed)
    {
        int heightSeed = (int)Map__Coordinates_To_Unique_Float(x, z);
        System.Random rand = new System.Random(heightSeed + seed);
        return rand.Next(100) / 100f;
    }
    
    public static float Map__Coordinates_To_Unique_Float(int x, int z)
    {
        // Wikipedia magic
        return 2920 * (float)Mathf.Sin(x * 21942 + z * 171324 + 8912) * (float)Mathf.Cos(x * 23157 * z * 217832 + 9758);
    }
}
