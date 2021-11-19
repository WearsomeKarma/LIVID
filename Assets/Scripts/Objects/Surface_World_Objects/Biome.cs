using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class Biome : Spatial_Element
{
    /// <summary>
    /// The array of structures which are procedurally
    /// generated in this biome. 
    /// </summary>
    [SerializeField]
    private Object_Instance_Spawn[] biome_Structures;
    internal int Biome_Structure_Count
        => biome_Structures.Length;
    internal Object_Instance_Spawn? Get_Structure_Spawn
    (
        Noise_Position instance_Position,
        double spawnWeight, 
        double noiseDensity,
        double temperature, 
        double moisture
    )
    {
        foreach(Object_Instance_Spawn spawnInstance in biome_Structures)
        {
            bool validTemp = temperature > spawnInstance.Temperature_Threshold;
            bool validMoisture = moisture > spawnInstance.Moisture_Threshold;
            bool validWeight = spawnWeight > spawnInstance.Noise_Threshold;
            bool validDensity = noiseDensity > spawnInstance.Noise_Density;

            bool validSpawn = validTemp && validMoisture && validWeight && validDensity;

            if (!validSpawn)
                continue;

            Object_Instance_Spawn copy = 
                new Object_Instance_Spawn(spawnInstance, instance_Position);

            return copy;
        }
        return null;
    }

    /// <summary>
    /// The array of mobs which are randomly
    /// generated in this biome.
    /// </summary>
    [SerializeField]
    private Object_Instance_Spawn[] biome_Mobs;
    internal IEnumerable<Object_Instance_Spawn> Biome_Mobs
        => biome_Mobs;
}
