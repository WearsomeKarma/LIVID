using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class Biome
{
    [SerializeField]
    private int biome_Seed;
    internal int Biome_Seed
        => biome_Seed;

    [SerializeField]
    private Material biome_Ground;
    internal Material Biome_Ground
        => biome_Ground;

    /// <summary>
    /// The array of structures which are procedurally
    /// generated in this biome. 
    /// </summary>
    [SerializeField]
    private Structure_Instance_Spawn[] biome_Structures;
    internal int Biome_Structure_Count
        => biome_Structures.Length;
    internal Structure_Instance_Spawn Get_Structure_Spawn(int id)
        => biome_Structures[id];

    /// <summary>
    /// The array of mobs which are randomly
    /// generated in this biome.
    /// </summary>
    [SerializeField]
    private Object_Instance_Spawn[] biome_Mobs;
    internal IEnumerable<Object_Instance_Spawn> Biome_Mobs
        => biome_Mobs;
}
