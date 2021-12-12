using System.Collections.Generic;
using UnityEngine;

public class Chunk : Spatial_Element
{
    private NoiseMap Chunk_HEIGHT_MAP { get; }
    internal Color[] Chunk_GROUND_COLORS { get; }
    private List<Object_Instance_Spawn> Chunk_STRUCTURES { get; }
    internal IEnumerable<Object_Instance_Spawn> Get_Structure_Object()
        => Chunk_STRUCTURES;

    public Vector3 Chunk_WORLD_POSTIION { get; }

    public Chunk
    (
        Vector3 chunkWorldPosition,
        NoiseMap heightMap,
        Color[] groundColor,
        List<Object_Instance_Spawn> structures
    )
    {
        Chunk_HEIGHT_MAP = heightMap;
        Chunk_GROUND_COLORS = groundColor;
        Chunk_STRUCTURES = structures;

        Chunk_WORLD_POSTIION = chunkWorldPosition;
    }

    public double this[Noise_Position position]
    {
        get => Chunk_HEIGHT_MAP[position];
        set => Chunk_HEIGHT_MAP[position] = value;
    }
    public double this[int x, int z]
    {
        get => Chunk_HEIGHT_MAP[x,z];
        set => Chunk_HEIGHT_MAP[x,z] = value;
    }
}
