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
    private Vector3 Instance_POSITION { get; }
    internal GameObject Create_Instance()
    {
        GameObject instance = GameObject.Instantiate(object_Instance);
        instance.transform.position =
            Instance_POSITION;

        return instance;
    }

    public Object_Instance_Spawn
    (
        Object_Instance_Spawn copy,
        Vector3 instance_Position
    )
    {
        object_Instance = copy.object_Instance;
        empty_Allocation_Radius = copy.empty_Allocation_Radius;
        offset_Radius = copy.offset_Radius;
        noise_Density = copy.noise_Density;
        noise_Threshold = copy.noise_Threshold;
        temperature_Threshold = copy.temperature_Threshold;
        moisture_Threshold = copy.moisture_Threshold;

        offset_Spawn = copy.offset_Spawn;

        object_Instance = copy.object_Instance;
        Instance_POSITION = 
            instance_Position
            +
            offset_Spawn;
    }

    /// <summary>
    /// This is the area which the object will prevent other
    /// objects from spawning. This is used to prevent 
    /// entities from spawning inside of trees for example.
    /// </summary>
    [SerializeField]
    private int empty_Allocation_Radius;
    internal int Empty_Allocation_Radius
        => empty_Allocation_Radius;

    /// <summary>
    /// This is the radius from the spawn point which
    /// the instance object can deviate from. This
    /// shouldn't be greater than Empty_Allocation_Radius.
    /// </summary>
    [SerializeField]
    private float offset_Radius;
    internal float Offset_Radius
        => offset_Radius;

    /// <summary>
    /// This determines the density which this
    /// object will spawn in an area.
    /// </summary>
    [SerializeField]
    private double noise_Density;
    internal double Noise_Density
        => noise_Density;

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
}
