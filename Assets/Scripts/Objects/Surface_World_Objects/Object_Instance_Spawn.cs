using System;
using UnityEngine;

[Serializable]
public struct Object_Instance_Spawn
{
    /// <summary>
    /// This is an object which the biome will include
    /// into consideration during chunk generation.
    /// </summary>
    [SerializeField]
    private GameObject object_Instance;
    public Noise_Position Instance_POSITION { get; }
    internal GameObject Create_Instance()
    {
        GameObject instance = GameObject.Instantiate(object_Instance);

        return instance;
    }

    public Object_Instance_Spawn
    (
        Object_Instance_Spawn copy,
        Noise_Position instance_Position
    )
    {
        object_Instance = copy.object_Instance;
        noise_Density = copy.noise_Density;
        noise_Threshold = copy.noise_Threshold;
        temperature_Threshold = copy.temperature_Threshold;
        moisture_Threshold = copy.moisture_Threshold;

        offset_Spawn = copy.offset_Spawn;

        object_Instance = copy.object_Instance;
        Instance_POSITION = instance_Position;
    }

    /// <summary>
    /// This determines the density which this
    /// object will spawn in an area.
    /// </summary>
    [SerializeField]
    private double noise_Density;
    internal double Noise_Density
        => noise_Density;

    /// <summary>
    /// This is threshold states what value
    /// the noise must exceed for this spawn
    /// condition to be considered. A threshold of
    /// 0 would cause this spawn condition to always
    /// pass on every vertex of the chunk.
    /// </summary>
    [SerializeField]
    private double noise_Threshold;
    public double Noise_Threshold
        => noise_Threshold;

    [SerializeField]
    private double temperature_Threshold;
    public double Temperature_Threshold
        => temperature_Threshold;

    [SerializeField]
    private double moisture_Threshold;
    public double Moisture_Threshold
        => moisture_Threshold;

    /// <summary>
    /// This is the vector3 offset for where the
    /// object will spawn relative to it's chosen
    /// position. This is typically used to offset
    /// the height of a tree so it's not floating
    /// above the ground, or peeking out of the ground.
    /// </summary>
    [SerializeField]
    private Vector3 offset_Spawn;
    internal Vector3 Offset_Spawn
        => offset_Spawn;
}
