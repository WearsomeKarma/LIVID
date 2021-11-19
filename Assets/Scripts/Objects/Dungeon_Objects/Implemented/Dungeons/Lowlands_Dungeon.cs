
using System.Collections.Generic;

public sealed class Lowlands_Dungeon :
    Dungeon
{
    public Lowlands_Dungeon(int width, int height, int room_count) 
    : base(width, height, room_count)
    {
    }

    protected override void Construct_Hallway_Cells(IEnumerable<Dungeon_Hallway_Graph_Edge> hallway_edges)
    {
        throw new System.NotImplementedException();
    }

    protected override void Construct_Room_Cells(Dungeon_KDTree_Partition room_partition, Noise_Position hallway_link)
    {
        throw new System.NotImplementedException();
    }

    protected override Dungeon_KDTree_Node Get_Initial_Room(IEnumerable<Dungeon_KDTree_Node> endpoints)
    {
        throw new System.NotImplementedException();
    }

    protected override Noise_Position? Suggest_Partition(Dungeon_KDTree_Partition space)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerable<Dungeon_Cell> Get_Cells()
    {
        throw new System.NotImplementedException();
    }
}
