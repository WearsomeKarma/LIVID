using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a chunk at runtime. It holds
/// references to various game objects which
/// must be Destroyed via Runtime_Chunk.Dispose()
/// when this chunk is no longer relevant.
/// </summary>
public sealed class Runtime_Chunk :
    Spatial_Element,
    IDisposable
{
    internal Chunk Chunk_CHUNK { get; }
    internal GameObject Chunk_Game_Object { get; set; } 
    internal Noise_Position Chunk_Position { get; set; }

    private List<GameObject> Chunk_STRUCTURES { get; }

    internal Runtime_Chunk
    (
        Chunk chunk, Noise_Position chunk_Position)
    {
        Chunk_CHUNK = chunk;
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
