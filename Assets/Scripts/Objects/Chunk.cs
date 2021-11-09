using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Chunk :
    IDisposable
{
    internal NoiseMap Chunk_HEIGHT_MAP { get; }
    internal GameObject Chunk_Game_Object { get; set; } 
    internal Chunk_Position Chunk_Position { get; set; }

    private List<GameObject> Chunk_STRUCTURES { get; }

    internal Chunk(NoiseMap heightMap, Chunk_Position chunk_Position)
    {
        Chunk_HEIGHT_MAP = heightMap;
        Chunk_Position = chunk_Position;

        Chunk_STRUCTURES = new List<GameObject>();
    }

    internal void Add_Structure(GameObject structure)
    {
        Chunk_STRUCTURES.Add(structure);
    }

    public void Dispose()
    {
        GameObject.Destroy(Chunk_Game_Object);
        foreach(GameObject structure in Chunk_STRUCTURES)
            GameObject.Destroy(structure);
    }
}
