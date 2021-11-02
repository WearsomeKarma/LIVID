using System;
using UnityEngine;

[Serializable]
public class Structure_Instance_Spawn :
    Object_Instance_Spawn
{
    /// <summary>
    /// This is threshold states what value
    /// the noise must exceed for this spawn
    /// condition to be considered. A threshold of
    /// 0 would cause this spawn condition to always
    /// pass on every vertex of the chunk.
    /// </summary>
    [SerializeField]
    private float noise_Threshold;
    public float Noise_Threshold
        => noise_Threshold;
}
