
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a prefab collection used for
/// creating dungeon cells.
/// </summary>
[Serializable]
public sealed class Dungeon_Schematic
{
    [SerializeField]
    private GameObject[] wall_Prefabs;
    /// <summary>
    /// Represents a collection of wall prefabs.
    /// These will be rotated around and fitted with
    /// Dungeon_Runtime_Cell_Instanciator.
    /// </summary>
    internal IEnumerable<GameObject> Wall_Prefabs
        => wall_Prefabs.ToArray();

    [SerializeField]
    private GameObject[] wall_Clutter_Prefabs;
    /// <summary>
    /// Represents a collection of wall clutter objects such as torches
    /// or wall chains. Spooky.
    /// </summary>
    internal IEnumerable<GameObject> Wall_Clutter_Prefabs
        => wall_Clutter_Prefabs.ToArray();

    [SerializeField]
    private GameObject[] floor_Prefabs;
    
    internal IEnumerable<GameObject> Floor_Prefabs
        => floor_Prefabs.ToArray();

    [SerializeField]
    private GameObject[] floor_Clutter_Prefabs;

    internal IEnumerable<GameObject> Floor_Clutter_Prefabs
        => floor_Clutter_Prefabs.ToArray();

    [SerializeField]
    private GameObject[] ceiling_Clutter_Prefabs;

    internal IEnumerable<GameObject> Ceiling_Clutter_Prefabs
        => ceiling_Clutter_Prefabs.ToArray();
}
