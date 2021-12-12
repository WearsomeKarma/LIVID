
using System;
using UnityEngine;

/// <summary>
/// Represents a prefab collection used for
/// creating dungeon cells.
/// </summary>
[Serializable]
public class Dungeon_Schematic
{
    [SerializeField]
    private Dungeon_Runtime_Cell_Fabricator room_Fabricator;

    internal Dungeon_Runtime_Cell_Fabricator Room_Fabricator => room_Fabricator;


    [SerializeField]
    private Dungeon_Runtime_Cell_Fabricator hallway_Fabricator;

    internal Dungeon_Runtime_Cell_Fabricator Hallway_Fabricator => hallway_Fabricator;
}
