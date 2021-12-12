using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Generator : MonoBehaviour 
{
    private Dungeon dungeon ;
    [SerializeField]
    private Dungeon_Schematic dungeon_Schematic;

    private Dictionary<Noise_Position, Dungeon_Runtime_Cell> Dungeon_RUNTIME_CELLS =
        new Dictionary<Noise_Position, Dungeon_Runtime_Cell>();

    public void Start()
    {
        dungeon = new Debug_Dungeon(10,10,2,12345);
        dungeon.Generate_Dungeon();
        IEnumerable<Dungeon_Cell> dungeon_Cells = 
            dungeon.Get_Cells();

        IEnumerable<Dungeon_Runtime_Cell> runtime_Cells = 
            dungeon.Generate_Runtime_Cells(dungeon_Schematic);

        foreach(Dungeon_Runtime_Cell runtime_Cell in runtime_Cells)
            Dungeon_RUNTIME_CELLS.Add(runtime_Cell.Runtime_Cell__POSITION, runtime_Cell);
    }

    public void Update()
    {
        
    }
}
