
using System.Collections.Generic;

public sealed class Debug_Dungeon :
Dungeon
{
    private Dictionary<Noise_Position, Dungeon_Cell> Debug_Dungeon__CELLS { get; }

    System.Random Debug_Dungeon__RANDOM { get; } 

    public Debug_Dungeon(int width, int height, int room_count, int seed = 0)
    : base(width, height, room_count)
    {
        Debug_Dungeon__RANDOM = new System.Random(seed);

        Debug_Dungeon__CELLS =
            new Dictionary<Noise_Position, Dungeon_Cell>();
    }

    protected override void Construct_Hallway_Cells(IEnumerable<Dungeon_Hallway_Graph_Edge> hallway_edges)
    {
        foreach(Dungeon_Hallway_Graph_Edge edge in hallway_edges)
        {
            Construct_Hallway_Edge_Cells(edge);
        }
    }

    private void Construct_Hallway_Edge_Cells(Dungeon_Hallway_Graph_Edge edge)
    {
        foreach(Noise_Position cell_position in edge.Get_Edge_Points())
        {
            if (Debug_Dungeon__CELLS.ContainsKey(cell_position))
                continue;

            Debug_Dungeon__CELLS.Add
            (
                cell_position,
                new Dungeon_Cell(cell_position, Dungeon_Cell_Type.Hallway)
            );
        }
    }

    protected override void Construct_Room_Cells(Dungeon_KDTree_Partition room_partition, Noise_Position hallway_link)
    {
        //TODO: implement
    }

    protected override IEnumerable<Dungeon_Cell> Get_Cells()
    {
        return Debug_Dungeon__CELLS.Values;
    }

    protected override Dungeon_KDTree_Node Get_Initial_Room(IEnumerable<Dungeon_KDTree_Node> endpoints)
    {
        IEnumerator<Dungeon_KDTree_Node> enumerator = endpoints.GetEnumerator();
        enumerator.MoveNext();
        return enumerator.Current;
    }

    protected override Noise_Position? Suggest_Partition(Dungeon_KDTree_Partition space)
    {
        int x = Debug_Dungeon__RANDOM.Next(space.Partition__MAX_X);
        int z = Debug_Dungeon__RANDOM.Next(space.Partition__MAX_Z);

        return new Noise_Position(x,z);
    }
}
