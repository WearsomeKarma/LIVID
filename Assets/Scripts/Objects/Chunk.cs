using System;
using UnityEngine;

public sealed class Chunk :
    IDisposable
{
    private NoiseMap Chunk_HEIGHT_MAP { get; }
    private GameObject Chunk_GAME_OBJECT { get; set; } 
    public int Chunk_X { get; }
    public int Chunk_Y { get; }

    internal Chunk(NoiseMap heightMap, int cx, int cy)
    {
        Chunk_HEIGHT_MAP = heightMap;
        Chunk_X = cx;
        Chunk_Y = cy;
    }

    public void Dispose()
    {
        GameObject.Destroy(Chunk_GAME_OBJECT);
    }
}
