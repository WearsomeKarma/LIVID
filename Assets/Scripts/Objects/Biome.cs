using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class Biome
{
    /// <summary>
    /// The array of structures which are procedurally
    /// generated in this biome. 
    /// </summary>
    [SerializeField]
    private Structure_Instance_Spawn[] biome_Structures;
    internal IEnumerable<Structure_Instance_Spawn> Biome_Structures
        => biome_Structures;

    /// <summary>
    /// The array of mobs which are randomly
    /// generated in this biome.
    /// </summary>
    [SerializeField]
    private Object_Instance_Spawn[] biome_Mobs;
    internal IEnumerable<Object_Instance_Spawn> Biome_Mobs
        => biome_Mobs;
}
