
using System.Collections.Generic;

public sealed class Debug_Dungeon :
Dungeon
{
    private Dictionary<Noise_Position, Dungeon_Cell> Debug_Dungeon__CELLS { get; }

    System.Random Debug_Dungeon__RANDOM { get; } 

    public Debug_Dungeon(int width, int height, int room_count, int seed) 
    : base(width, height, room_count)
    {
        Debug_Dungeon__RANDOM = new System.Random(seed);
        Debug_Dungeon__CELLS = new Dictionary<Noise_Position, Dungeon_Cell>();
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
        int wiggleX = (room_partition.Partition__MAX_X - room_partition.Partition__MIN_X) / 2;
        int room_minX = Debug_Dungeon__RANDOM.Next(wiggleX) + room_partition.Partition__MIN_X;
        int room_maxX = -Debug_Dungeon__RANDOM.Next(wiggleX) + room_partition.Partition__MAX_X;
        
        int wiggleZ = (room_partition.Partition__MAX_Z - room_partition.Partition__MIN_Z) / 2;
        int room_minZ = Debug_Dungeon__RANDOM.Next(wiggleZ) + room_partition.Partition__MIN_Z;
        int room_maxZ = -Debug_Dungeon__RANDOM.Next(wiggleZ) + room_partition.Partition__MAX_Z;
        
        foreach(Noise_Position room_cell_position in room_partition.Get_Positions())
        {
            if (room_cell_position.NOISE_X < room_minX || room_cell_position.NOISE_X > room_maxX)
                continue;
            if (room_cell_position.NOISE_Z < room_minZ || room_cell_position.NOISE_Z > room_maxZ)
                continue;
            Dungeon_Cell cell = new Dungeon_Cell(room_cell_position, Dungeon_Cell_Type.Room);
            Debug_Dungeon__CELLS.Add(room_cell_position, cell);
        }
    }

    internal override IEnumerable<Dungeon_Cell> Get_Cells()
    {
        return Debug_Dungeon__CELLS.Values;
    }

    protected override Noise_Position? Suggest_Partition(Dungeon_KDTree_Partition space)
    {
        int x = Debug_Dungeon__RANDOM.Next(space.Partition__MAX_X - 3) + space.Partition__MIN_X + 3;
        int z = Debug_Dungeon__RANDOM.Next(space.Partition__MAX_Z - 3) + space.Partition__MIN_Z + 3;

        if (x <= space.Partition__MIN_X || x >= space.Partition__MAX_X)
            return null;
        if (z <= space.Partition__MIN_Z || z >= space.Partition__MAX_Z)
            return null;
        
        return new Noise_Position(x,z);
    }

    internal override IEnumerable<Dungeon_Runtime_Cell> Generate_Runtime_Cells(Dungeon_Schematic schematic)
    {
        foreach(Dungeon_Cell cell in Debug_Dungeon__CELLS.Values)
        {
            UnityEngine.GameObject floor = schematic.Hallway_Fabricator.Instanciate_Floor_Ceiling(cell.Dungeon_Cell__POSITION);

            yield return new Dungeon_Runtime_Cell(cell.Dungeon_Cell__POSITION, new UnityEngine.GameObject[] {floor});
        }
        yield break;
    }

    protected override Dungeon_KDTree_Partition Get_Initial_Room(IEnumerable<Dungeon_KDTree_Partition> endpoint_partitions)
    {
        IEnumerator<Dungeon_KDTree_Partition> enumerator = endpoint_partitions.GetEnumerator();
        enumerator.MoveNext();
        return enumerator.Current;
    }
}
