
using System;
using UnityEngine;

public sealed class Dungeon_Runtime_Cell :
    IDisposable
{
    private GameObject[] Runtime_Cell__GAMEOBJECT_INSTANCES { get; }

    public Noise_Position Runtime_Cell__POSITION { get; }

    public Dungeon_Runtime_Cell
    (
        Noise_Position cell_Position,
        GameObject[] gameObject_Instances 
    )
    {
        Runtime_Cell__POSITION = cell_Position;
        Runtime_Cell__GAMEOBJECT_INSTANCES = gameObject_Instances;
    }

    public void Dispose()
    {
        foreach(GameObject gobj in Runtime_Cell__GAMEOBJECT_INSTANCES)
            GameObject.Destroy(gobj);
    }
}
