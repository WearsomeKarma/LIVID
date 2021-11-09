using System;
using UnityEngine;

[Serializable]
public class Object_Instance_Spawn
{
    /// <summary>
    /// This is an object which the biome will include
    /// into consideration during chunk generation.
    /// </summary>
    [SerializeField]
    private GameObject object_Instance;
    internal GameObject Object_Instance
        => object_Instance;

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
}
