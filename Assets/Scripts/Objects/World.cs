using System;
using System.Collections.Generic;
using UnityEngine;

public class World :
    Spatial_Table<Chunk>
{
    private Chunk_Noise_Table World__CHUNK_HEIGHT_MAPS;
    private Chunk_Noise_Table World__CHUNK_STRUCTURE_NOISE { get; }

    private World_Noise_Table World__BIOME_HEIGHT_MAPS;
    private World_Noise_Table World__BIOME_TEMPERATURE_MAPS { get; }
    private World_Noise_Table World__BIOME_MOISTURE_MAPS { get; }

    private int World__BIOME_TABLE_SIZE { get; }
    private int World__BIOME_RANGE { get; }
    private Biome[,] World__BIOME_TABLE { get; }
    private double World__BIOME_VARIANCE { get; }

    private Texture2D World__BIOME_GRADIENT { get; }

    private Action Initial_Generation_Handler;

    public World
    (
        int seed,

        int table_Size,
        int biomeTableSize,
        Biome[,] biomeMap,
        Texture2D biomeGradient,
        int world_Elevation_Weight = 10,
        Action handleInitialGeneration = null
    )
    : base
    (
        table_Size,   
        new Noise_Position(-999,-999),
        Chunk_Noise_Table.CHUNK_SIZE
    )
    {
        System.Random seeder = new System.Random(seed);
        int seed_BiomeHeightMap = seed;
        int seed_ChunkHeightMap = seeder.Next();
        int seed_ChunkStructureNoise = seeder.Next();
        int seed_BiomeTempMap = seeder.Next();
        int seed_BiomeMoistureMap = seeder.Next();
        Element_Generator = Generate_Chunk;

        World__BIOME_TABLE_SIZE = biomeTableSize;
        World__BIOME_RANGE = (biomeTableSize <= 0) ? 0 : (biomeTableSize-1);
        World__BIOME_TABLE = biomeMap;

        World__BIOME_VARIANCE = 1.0/biomeTableSize;

        World__BIOME_GRADIENT = biomeGradient;

        Initial_Generation_Handler = handleInitialGeneration;

        Noise_Position init_Pos   
            = new Noise_Position(-999,-999);

        World__CHUNK_HEIGHT_MAPS =
            new Chunk_Noise_Table
            (
                seed_ChunkHeightMap,
                table_Size,
                init_Pos
            );

        World__CHUNK_STRUCTURE_NOISE =
            new Chunk_Noise_Table
            (
                seed_ChunkStructureNoise,
                table_Size,
                init_Pos
            );

        Noise_Position world_Offset
            = new Noise_Position(World_Noise_Table.WORLD_NOISE_SCALE/2, World_Noise_Table.WORLD_NOISE_SCALE/2);

        int world_Table_Size =
            (Table_Size * Chunk_Noise_Table.CHUNK_SIZE) / World_Noise_Table.WORLD_NOISE_SCALE + 4;
        Noise_Position world_Table_Offset =
            new Noise_Position(world_Table_Size/-2, world_Table_Size/-2);

        World__BIOME_HEIGHT_MAPS =
            new World_Noise_Table
            (
                seed_BiomeHeightMap,
                world_Table_Size + 1,
                init_Pos,
                offset: world_Table_Offset
            );

        World__BIOME_TEMPERATURE_MAPS =
            new World_Noise_Table
            (
                seed_BiomeTempMap,
                world_Table_Size + 1,
                init_Pos,
                offset: world_Table_Offset
            );

        World__BIOME_MOISTURE_MAPS =
            new World_Noise_Table
            (
                seed_BiomeMoistureMap,
                world_Table_Size + 1,
                init_Pos,
                offset: world_Table_Offset
            );
    }

    protected override void Handle_Initial_Generation()
        => Initial_Generation_Handler?.Invoke();

    internal override void Check_For_Updates 
    (
        Vector3 worldSpaceCenter, 
        out List<Noise_Position> invalidPositions, 
        out List<Noise_Position> generatedPositions
    )
    {
        List<Noise_Position> returned;

        World__BIOME_HEIGHT_MAPS 
            .Check_For_Updates(worldSpaceCenter, out returned, out _);
        World__BIOME_TEMPERATURE_MAPS
            .Check_For_Updates(worldSpaceCenter, out returned, out _);
        World__BIOME_MOISTURE_MAPS
            .Check_For_Updates(worldSpaceCenter, out returned, out _);

        World__CHUNK_HEIGHT_MAPS
            .Check_For_Updates(worldSpaceCenter, out returned, out _);
        World__CHUNK_STRUCTURE_NOISE
            .Check_For_Updates(worldSpaceCenter, out returned, out _);

        base.Check_For_Updates(worldSpaceCenter, out invalidPositions, out generatedPositions);
    }

    private Chunk Generate_Chunk(Noise_Position position)
    {
        // Positional Data.
        Noise_Position gamespacePosition =
            Scale_To_Gamespace(position);
        Noise_Position biomeTablePosition =
            World__BIOME_HEIGHT_MAPS.Scale_To_Tablespace(gamespacePosition);

        NoiseMap world_Elevation_Map =
            World__BIOME_HEIGHT_MAPS[biomeTablePosition];

        NoiseMap world_Temp_Map =
            World__BIOME_TEMPERATURE_MAPS[biomeTablePosition];
        
        NoiseMap world_Moisture_Map =
            World__BIOME_MOISTURE_MAPS[biomeTablePosition];


        NoiseMap chunk_Base_Height_Map =
            World__CHUNK_HEIGHT_MAPS[position];

        NoiseMap chunk_Structure_Noise =
            World__CHUNK_STRUCTURE_NOISE[position];

        NoiseMap chunk_Final_Height_Map =
            new NoiseMap(Chunk_Noise_Table.CHUNK_SIZE);

        List<Object_Instance_Spawn> structures =
            new List<Object_Instance_Spawn>();

        Color[] chunkGroundColor = new Color[Chunk_Noise_Table.CHUNK_SIZE * Chunk_Noise_Table.CHUNK_SIZE];

        for(int z=0;z<Chunk_Noise_Table.CHUNK_SIZE;z++)
        {
            for(int x=0;x<Chunk_Noise_Table.CHUNK_SIZE;x++)
            {
                //Get locational data
                Noise_Position chunk_Local_Position =
                    new Noise_Position(x,z);

                Noise_Position gamespace_Specific_Position =
                    chunk_Local_Position
                    +
                    gamespacePosition;

                Noise_Position biome_Specific_Position =
                    Noise_Position
                    .Positive_Modulo(gamespace_Specific_Position, World_Noise_Table.WORLD_NOISE_SCALE);

                double world_Height =
                    world_Elevation_Map[biome_Specific_Position];

                //interpolate height noisemap between chunk and biome.
                double chunkHeight =
                    NoiseBlender
                    .Blend_Height__Chunk_And_Biome
                    (
                        chunk_Base_Height_Map[chunk_Local_Position],
                        5,
                        world_Height,
                        50
                    );

                chunk_Final_Height_Map[chunk_Local_Position] =
                    chunkHeight;

                //clamp moisture under temp
                double temperature  = world_Temp_Map[biome_Specific_Position];
                double moisture     = world_Moisture_Map[biome_Specific_Position];

                //reduce moisture with biome height
                moisture = NoiseBlender.Moderate_By_Height(moisture, world_Height);
                temperature = NoiseBlender.Moderate_By_Height(temperature, world_Height);

                moisture = (moisture < 0) ? 0 : (moisture > temperature ? temperature : moisture); 

                //(temp,moisture) indexing get a
                //biome id.
                int indexTemp       = Mathf.RoundToInt(World__BIOME_RANGE * (float)temperature);
                int indexMoisture   = Mathf.RoundToInt(World__BIOME_RANGE * (float)moisture);

                //Debug.Log($"BIOME ON VERT: temp: {temperature} moisture: {moisture} tempID: {indexTemp} moistureID: {indexMoisture}");
                Biome biome = World__BIOME_TABLE[indexTemp, indexMoisture];
                
                //structure lookup by biomeID
                Object_Instance_Spawn? structureInstance =
                    biome?.Get_Structure_Spawn
                    (
                        new Vector3(gamespace_Specific_Position.NOISE_X, (float)chunkHeight, gamespace_Specific_Position.NOISE_Z),
                        chunk_Structure_Noise[chunk_Local_Position],
                        temperature,
                        moisture
                    );

                if (structureInstance != null)
                    structures.Add((Object_Instance_Spawn)structureInstance);
                
                //ground color by biomeID.
                Color biomeGroundColor = 
                    Get_Biome_Color
                    (
                        temperature,
                        moisture
                    );

                int colorIndex = z * Chunk_Noise_Table.CHUNK_SIZE + x;
                chunkGroundColor[colorIndex] = biomeGroundColor;
            }
        }

        Vector3 chunkWorldPosition =
            new Vector3
            (
                gamespacePosition.NOISE_X * Chunk_Noise_Table.CHUNK_SIZE,
                0,
                gamespacePosition.NOISE_Z * Chunk_Noise_Table.CHUNK_SIZE
            );

        Chunk generated_Chunk =
            new Chunk(chunkWorldPosition, chunk_Final_Height_Map, chunkGroundColor, structures);

        return generated_Chunk;
    }

    private Color Get_Biome_Color(double gradient_Temp, double gradient_moisture)
    {
        int index_Temp = Mathf.FloorToInt(World__BIOME_GRADIENT.height * (float)gradient_Temp);
        int index_Moisture = Mathf.FloorToInt(World__BIOME_GRADIENT.height * (float)gradient_moisture);

        return World__BIOME_GRADIENT.GetPixel(index_Temp, index_Moisture);
    }

    public double Get_Height(Vector3 position)
    {
        Noise_Position gamespacePosition, chunkPos;
        Chunk chunk = Get_Chunk(position, out gamespacePosition, out chunkPos);

        if (chunk == null)
        {
            throw new Exception($"Could not find height on {position} -> gspace {gamespacePosition} -> cspace {chunkPos}");
        }

        Noise_Position local = Noise_Position.Positive_Modulo(gamespacePosition, Chunk_Noise_Table.CHUNK_SIZE);

        return chunk[local];
    }

    public Chunk Get_Chunk(Vector3 position)
    {
        return Get_Chunk(position, out _, out _);
    }

    public Chunk Get_Chunk(Vector3 position, out Noise_Position gamespacePosition, out Noise_Position chunkPos)
    {
        gamespacePosition = new Noise_Position(position);
        chunkPos =
            World__CHUNK_HEIGHT_MAPS.Scale_To_Tablespace(gamespacePosition);

        return this[chunkPos];
    }
}
