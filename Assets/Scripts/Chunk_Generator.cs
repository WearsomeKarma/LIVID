using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Chunk_Generator :
    MonoBehaviour
{
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
    /// The material which the ground uses.
    /// </summary>
    [SerializeField]
    private Material ground_Material;

    [SerializeField]
    Texture2D biome_Gradient;

    [SerializeField]
    private Biome[] biomes;

    private World Chunk_Generator__World { get; set; }
    private Spatial_Table<Runtime_Chunk> Runtime_Chunk_Map { get; set; }
    private Vector3 Chunk_Generator__World_Offset { get; set; }

    public void Start()
    {
        int sqrtSize = Mathf.CeilToInt(Mathf.Sqrt(biomes.Length));
        if (sqrtSize*sqrtSize <= biomes.Length)
            sqrtSize++;

        Biome[,] biomeGrid = new Biome[sqrtSize,sqrtSize];

        for(int i=0, b=0;i<sqrtSize;i++)
        {
            for(int j=0;j<sqrtSize;j++)
            {
                if (j < i)
                {
                    biomeGrid[j,i] = null;
                    continue;
                }

                int index = b;
                b++;

                index = (index < biomes.Length) ? index: biomes.Length - 1;
                
                biomeGrid[j,i] = biomes[index];
            }
        }

        Chunk_Generator__World =
            new World
            (
                Seed, 
                render_Distance,
                sqrtSize,
                biomeGrid,
                biome_Gradient,
                handleInitialGeneration: Set_Player_SpawnY
            );

        int size = Chunk_Noise_Table.CHUNK_SIZE - 1;
        Chunk_Generator__World_Offset =
            new Vector3(size * Chunk_Generator__World.Table_Size / -2, 0, size * Chunk_Generator__World.Table_Size / -2);

        Runtime_Chunk_Map =
            new Spatial_Table<Runtime_Chunk>
            (
                render_Distance,
                new Noise_Position(-999,-999),
                Chunk_Noise_Table.CHUNK_SIZE,
                Generate_Chunk,
                Delete_Chunk
            );
    }

    private void Set_Player_SpawnY()
    {
        float height =
            4
            +
            (float)Chunk_Generator__World.Get_Height(player.transform.position - Chunk_Generator__World_Offset);

        player.transform.position =
            new Vector3(player.transform.position.x, height, player.transform.position.z);
    }

    public void Update()
    {
        //On each update, check to see if the player
        //has updated their chunk index by moving.
        
        Vector3 center = 
            player.transform.position;

        Chunk_Generator__World
            .Check_For_Updates(center, out _, out _);

        List<Noise_Position> generatedPositions;

        Runtime_Chunk_Map
            .Check_For_Updates(center, out _, out generatedPositions);

        Adjust_Seams(generatedPositions);
    }

    private void Delete_Chunk(Noise_Position invalidPosition)
    {
        Runtime_Chunk invalidChunk = Runtime_Chunk_Map[invalidPosition];
        invalidChunk?.Dispose();
    }

    private Runtime_Chunk Generate_Chunk(Noise_Position position)
    {
        Chunk chunk = Chunk_Generator__World[position];

        Mesh ground_Mesh = ChunkMeshBuilder.Build_Height_Mesh(chunk, Chunk_Noise_Table.CHUNK_SIZE);

        ground_Mesh.colors = chunk.Chunk_GROUND_COLORS;

        GameObject ground_Object = new GameObject();
        MeshFilter ground_Mesh_Filter = 
            ground_Object.AddComponent<MeshFilter>();
        MeshRenderer ground_Mesh_Render =
            ground_Object.AddComponent<MeshRenderer>();
        MeshCollider ground_Mesh_Collider =
            ground_Object.AddComponent<MeshCollider>();

        ground_Mesh_Filter.mesh = ground_Mesh;
        ground_Mesh_Collider.sharedMesh = ground_Mesh;

        ground_Mesh_Render.material = ground_Material;

        Runtime_Chunk generated_Chunk = 
            new Runtime_Chunk
            (
                chunk,
                position
            )
            {
                Chunk_Game_Object = ground_Object
            };

        int size = Chunk_Noise_Table.CHUNK_SIZE -1;

        Vector3 groundPosition = 
            new Vector3(size * chunk.Spatial_Position.NOISE_X, 0, size * chunk.Spatial_Position.NOISE_Z)
            +
            Chunk_Generator__World_Offset;
        ground_Object.transform.position = groundPosition;
        ground_Object.name = position.ToString();

        foreach(Object_Instance_Spawn structure_Instance in chunk.Get_Structure_Object())
        {
            GameObject structure = structure_Instance.Create_Instance();

            Noise_Position local = 
                Noise_Position.Positive_Modulo(structure_Instance.Instance_POSITION, Chunk_Noise_Table.CHUNK_SIZE);

            float x = local.NOISE_X;
            float y = (float)chunk[local];
            float z = local.NOISE_Z;

            structure.transform.position =
                new Vector3(x,y,z)
                +
                groundPosition;

            generated_Chunk.Add_Structure(structure);
        }

        return generated_Chunk;
    }

    private void Adjust_Seams(List<Noise_Position> generatedPositions)
    {
        if (generatedPositions == null)
            return;

        foreach(Noise_Position position in generatedPositions)
            Adjust_Seams(position);
    }

    private void Adjust_Seams(Noise_Position position)
    {
        Noise_Position below =
            position
            +
            new Noise_Position(0,-1);

        Noise_Position left =
            position
            +
            new Noise_Position(-1,0);

        Runtime_Chunk belowChunk = Runtime_Chunk_Map[below];
        Runtime_Chunk leftChunk = Runtime_Chunk_Map[left];

        bool isAvailable_Below =
            belowChunk != null;
        bool isAvailable_Left =
            leftChunk != null;

        if (isAvailable_Below)
            Stitch(Runtime_Chunk_Map[position], belowChunk, Stitch_Below);
        if (isAvailable_Left)
            Stitch(Runtime_Chunk_Map[position], leftChunk, Stitch_Left);
    }

    private void Stitch
    (   
        Runtime_Chunk subject, 
        Runtime_Chunk neighbor, 
        Action<Vector3[], Vector3[], Vector3[], Vector3[], int> stitcher
    )
    {
        Mesh meshSubject = 
            subject
            .Chunk_Game_Object
            .GetComponent<MeshFilter>()
            .mesh;
        Vector3[] subject_Vertices =
            meshSubject
            .vertices;
        Vector3[] subject_Normals =
            meshSubject
            .normals;
        Mesh meshNeighbor =
            neighbor
            .Chunk_Game_Object
            .GetComponent<MeshFilter>()
            .mesh;
        Vector3[] neighbor_Vertices =
            meshNeighbor
            .vertices;
        Vector3[] neighbor_Normals =
            meshNeighbor
            .normals;
        for(int i=0;i<Chunk_Noise_Table.CHUNK_SIZE;i++)
        {
            stitcher
            (  
                subject_Vertices, 
                neighbor_Vertices, 
                subject_Normals,
                neighbor_Normals,
                i
            );
        }

        meshSubject.vertices = subject_Vertices;
        meshSubject.normals = subject_Normals;

        meshNeighbor.vertices = neighbor_Vertices;
        meshNeighbor.normals = subject_Normals;

        meshSubject.RecalculateBounds();
        meshSubject.RecalculateNormals();

        meshNeighbor.RecalculateBounds();
        meshNeighbor.RecalculateNormals();
    }

    private void Stitch_Below
    (
        Vector3[] subject_Vertices, 
        Vector3[] neighbor_Vertices, 
        Vector3[] subject_Normals,
        Vector3[] neighbor_Normals,
        int index
    )
    {
        int vertexIndex_Subject  = 
            index; 
        int vertexIndex_Neighbor = 
            subject_Vertices.Length 
            -
            Chunk_Noise_Table.CHUNK_SIZE 
            +
            index;

        Stitch
        (
            subject_Vertices, 
            neighbor_Vertices, 
            subject_Normals,
            neighbor_Normals,
            vertexIndex_Subject, 
            vertexIndex_Neighbor
        );
    }

    private void Stitch_Left
    (
        Vector3[] subject_Vertices, 
        Vector3[] neighbor_Vertices, 
        Vector3[] subject_Normals,
        Vector3[] neighbor_Normals,
        int index
    )
    {
        int vertexIndex_Subject  = 
            Chunk_Noise_Table.CHUNK_SIZE * index;
        int vertexIndex_Neighbor =
            (Chunk_Noise_Table.CHUNK_SIZE * (index + 1)) - 1;

        Stitch
        (
            subject_Vertices, 
            neighbor_Vertices, 
            subject_Normals,
            neighbor_Normals,
            vertexIndex_Subject, 
            vertexIndex_Neighbor
        );
    }

    private void Stitch
    (
        Vector3[] subject_Vertices, Vector3[] neighbor_Vertices, 
        Vector3[] subject_Normals, Vector3[] neighbor_Normals,
        int vertexIndex_Subject, int vertexIndex_Neighbor
    )
    {
        Vector3 vector_Subject = subject_Vertices[vertexIndex_Subject];
        Vector3 vector_Neighbor = neighbor_Vertices[vertexIndex_Neighbor];

        float x = vector_Subject.x;
        float z = vector_Subject.z;
        
        float y = vector_Neighbor.y;

        subject_Vertices[vertexIndex_Subject]
            = new Vector3(x,y,z);

        Vector3 normal_Subject = subject_Normals[vertexIndex_Subject];
        Vector3 normal_Neighbor = neighbor_Normals[vertexIndex_Neighbor];

        Vector3 cross = Vector3.Cross(normal_Subject, normal_Neighbor);
        Vector3 cross_normalized = cross.normalized;

        subject_Normals[vertexIndex_Subject] = cross_normalized;
        neighbor_Normals[vertexIndex_Neighbor] = cross_normalized;
    }
}
