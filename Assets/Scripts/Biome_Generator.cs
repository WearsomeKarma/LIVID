using UnityEngine;

public class Biome_Generator :
    Chunk_Post_Processor
{
    /// <summary>
    /// For this instance of Biome_Generator this
    /// array indicates the biomes which the generator
    /// will take into consideration during chunk
    /// generation.
    /// </summary>
    [SerializeField]
    private Biome[] defined_Biomes;

    internal override void Post_Process_Chunk(Chunk c)
    {
        int biomeSeed = Seed * defined_Biomes[0].Biome_Seed;
        NoiseMap baseMap = DariusPerlinNoise.Get_Noise_Map(c.Chunk_Position, 2, biomeSeed);
        NoiseMap staticNoise = StaticNoise.Get_Noise_Map(c.Chunk_Position, biomeSeed);
        NoiseMap idNoiseMap  = StaticNoise.Get_Noise_Map(c.Chunk_Position, Seed);

        for(int i=1;i<defined_Biomes.Length;i++)
        {
            biomeSeed = Seed * defined_Biomes[i].Biome_Seed;

            NoiseMap biomeMap = DariusPerlinNoise.Get_Noise_Map(c.Chunk_Position, 2, biomeSeed);
            
            NoiseBlender.Blend(biomeMap, baseMap, 10);
        }

        for(int chunk_z=0;chunk_z<baseMap.SIZE;chunk_z++)
        {
            for(int chunk_x=0;chunk_x<baseMap.SIZE;chunk_x++)
            {
                Post_Process_Position(c, baseMap, staticNoise, idNoiseMap, chunk_x, chunk_z);
            }
        }
    }

    private void Post_Process_Position(Chunk c, NoiseMap biomeMap, NoiseMap staticNoise, NoiseMap idNoiseMap, int x, int z)
    {
        int biomeID = (int)(defined_Biomes.Length * biomeMap[x,z]);

        Biome biome = defined_Biomes[biomeID];

        c.Chunk_Game_Object.GetComponent<MeshRenderer>()
            .materials = new Material[] {biome.Biome_Ground};

        double densityNoise = staticNoise[x,z];

        int structureId = (int)System.Math.Floor(biome.Biome_Structure_Count * idNoiseMap[x,z]);

        Debug.Log($"biome on pos. StructID {structureId} on idNoiseMap {idNoiseMap[x,z]} for density {staticNoise[x,z]}");

        Structure_Instance_Spawn structureSchem =
            biome.Get_Structure_Spawn(structureId);

        if (structureSchem.Noise_Density > densityNoise)
            return;

        float y = (float)c.Chunk_HEIGHT_MAP[x,z];

        Vector3 spawnPos =
            c.Chunk_Game_Object.transform.position
            +
            new Vector3
            (
                x,y,z
            );

        GameObject instance = GameObject.Instantiate(structureSchem.Object_Instance);

        instance.transform.position = spawnPos;

        c.Add_Structure(instance);
    }
}
