using System.Collections.Generic;
using UnityEngine;

public sealed class Chunk_Generator :
    MonoBehaviour
{
    /// <summary>
    /// The number of vertices along the edge of
    /// each chunk. Chunks are square in size.
    /// </summary>
    public const int CHUNK_VERTEX_SIZE = 16;
    public const int CHUNK_CELL_SIZE = CHUNK_VERTEX_SIZE-1;

    /// <summary>
    /// The seed of the generator. Changing this
    /// during runtime can have some weird effects
    /// where new chunks will have no relation to
    /// already existing chunks.
    /// </summary>
    [SerializeField]
    private int seed = 0;
    public int Seed
        => seed;
    
    /// <summary>
    /// This is the level of percision the noise maps
    /// for each chunk will be generated to. A higher
    /// noise frequency will give more variation in terrain
    /// but may become chaotic. Additionally, any values above
    /// the CHUNK_VERTEX_SIZE limit (default 16) will cause issues.
    /// Increasing this will lower the game performace.
    /// </summary>
    [SerializeField]
    private int noise_Frequency = 2;
    public int Noise_Frequency
        => noise_Frequency;

    /// <summary>
    /// The inital distance which 
    /// chunks are generated out to from
    /// the player's position.
    /// </summary>
    [SerializeField]
    private int render_Distance = 1;
    private int Render_Distance_Squared
        => render_Distance * render_Distance;

    /// <summary>
    /// The player which chunk position is based
    /// off of.
    /// </summary>
    [SerializeField]
    private GameObject player;
    /// <summary>
    /// The X chunk index of the player.
    /// </summary>
    private int player_Chunk_X = -999; //Values set to -999 to cause inital world generation.
    /// <summary>
    /// The Z chunk index of the player.
    /// </summary>
    private int player_Chunk_Z = -999;
    private Chunk_Position Player_Base_Chunk_Position
        => new Chunk_Position(player_Chunk_X, player_Chunk_Z);

    /// <summary>
    /// Represents anything that cares about the
    /// formation of a chunk.
    /// </summary>
    [SerializeField]
    private Chunk_Post_Processor[] chunk_Post_Processors;

    private readonly List<Chunk> Chunk_Map = new List<Chunk>();

    public void Start()
    {
        System.Random seeder = new System.Random(seed);
        foreach(Chunk_Post_Processor processor in chunk_Post_Processors)
        {
            int procesorSeed = seeder.Next();
            processor.Initalize(procesorSeed);
        }
    }

    public void Update()
    {
        //On each update, check to see if the player
        //has updated their chunk index by moving.
        
        Vector3 delta_Player_Chunk_Pos =
            player.transform.position 
            /
            CHUNK_CELL_SIZE;

        int next_Player_Chunk_X =
            (int)delta_Player_Chunk_Pos.x;
        int next_Player_Chunk_Z =
            (int)delta_Player_Chunk_Pos.z;

        bool changeInX =
            next_Player_Chunk_X
            != 
            player_Chunk_X;

        bool changeInZ =
            next_Player_Chunk_Z
            != 
            player_Chunk_Z;

        if (changeInX || changeInZ)
        {

        Debug.Log($"COMPARE {delta_Player_Chunk_Pos} to {Player_Base_Chunk_Position}");
            player_Chunk_X = next_Player_Chunk_X;
            player_Chunk_Z = next_Player_Chunk_Z;

            IEnumerable<Chunk_Position> requiredPositions = 
                Relocate_Chunks();
            Generate_Chunks(requiredPositions);
        }
    }

    /// <summary>
    /// This will move local chunks to new logical positions
    /// if they are to be reused. Otherwise record the missing
    /// position and return all missing positions.
    /// </summary>
    private IEnumerable<Chunk_Position> Relocate_Chunks()
    {
        List<Chunk_Position> requiredPositions =
            new List<Chunk_Position>();

        for(int scan_z=-render_Distance; scan_z < render_Distance; scan_z++)
        {
            for(int scan_x=-render_Distance; scan_x < render_Distance; scan_x++)
            {
                Chunk_Position scanning_Position =
                    new Chunk_Position(scan_x, scan_z)
                    +
                    Player_Base_Chunk_Position;

                int distToPlayer =
                    Chunk_Position.Distance_Squared
                    (
                        scanning_Position,
                        Player_Base_Chunk_Position
                    );

                if (distToPlayer <= Render_Distance_Squared)
                {
                    requiredPositions.Add(scanning_Position);
                }
            }
        }

        foreach(Chunk chunk in Chunk_Map.ToArray())
        {
            int distToPlayer = 
                Chunk_Position.Distance_Squared
                (
                    chunk.Chunk_Position,
                    Player_Base_Chunk_Position
                );

            if (distToPlayer <= Render_Distance_Squared)
            {
                requiredPositions.Remove(chunk.Chunk_Position);
                continue;
            }

            chunk.Dispose();
            Chunk_Map.Remove(chunk);
        }

        return requiredPositions;
    }

    private void Generate_Chunks(IEnumerable<Chunk_Position> requiredPositions)
    {
        foreach(Chunk_Position requiredPosition in requiredPositions)
        {
            Chunk generated_Chunk = 
                Generate_Chunk(requiredPosition);

            foreach(Chunk_Post_Processor post_Processor in chunk_Post_Processors)
            {
                post_Processor.Post_Process_Chunk(generated_Chunk);
            }

            Chunk_Map.Add(generated_Chunk);
        }
    }

    private Chunk Generate_Chunk(Chunk_Position chunkWorldPosition)
    {
        NoiseMap height_Map = 
            DariusPerlinNoise.Get_Noise_Map
            (
                chunkWorldPosition,
                2,
                seed
            ); 

        for(int chunk_z=0;chunk_z<height_Map.SIZE;chunk_z++)
        {
            for(int chunk_x=0;chunk_x<height_Map.SIZE;chunk_x++)
            {
                double y = height_Map[chunk_x, chunk_z];
                height_Map[chunk_x, chunk_z] = System.Math.Floor(10 * y);
            }
        }

        Mesh ground_Mesh = ChunkMeshBuilder.Build_Height_Mesh(height_Map);

        GameObject ground_Object = new GameObject();
        MeshFilter ground_Mesh_Filter = 
            ground_Object.AddComponent<MeshFilter>();
        MeshRenderer ground_Mesh_Render =
            ground_Object.AddComponent<MeshRenderer>();
        MeshCollider ground_Mesh_Collider =
            ground_Object.AddComponent<MeshCollider>();

        ground_Mesh_Filter.mesh = ground_Mesh;
        ground_Mesh_Collider.sharedMesh = ground_Mesh;

        ground_Mesh.RecalculateBounds();
        ground_Mesh.RecalculateNormals();

        Chunk generated_Chunk = 
            new Chunk
            (
                height_Map,
                chunkWorldPosition
            )
            {
                Chunk_Game_Object = ground_Object
            };

        ground_Object.transform.position = 
            new Vector3(CHUNK_CELL_SIZE * chunkWorldPosition.CHUNK_X, 0, CHUNK_CELL_SIZE * chunkWorldPosition.CHUNK_Z);
        ground_Object.name = chunkWorldPosition.ToString();

        return generated_Chunk;
    }
}
