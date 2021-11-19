
using System.Collections.Generic;

/// <summary>
/// Logical representation of a Dungeon in LIVID.
///
/// Classes which implement Dungeon construct
/// a spanning dungeon utilizing Dungeon_KDTree
/// for space partitioning, Dungeon_Hallway_Graph
/// for mapping critical hallway corners, and
/// Dungeon_Hallway_Linker to create hallways.
/// </summary>
public abstract class Dungeon
{
    private Dungeon_KDTree Dungeon__KDTREE { get; }
    private Dungeon_Hallway_Graph Dungeon__HALLWAY_GRAPH { get; }
    private Dungeon_Hallway_Linker Dungeon__HALLWAY_LINKER { get; }

    //private Dictionary<Dungeon_KDTree_Node, Dungeon_Room> Dungeon__ROOMS { get; }

    public Dungeon(int width, int height, int room_count)
    {
        Dungeon_KDTree_Partition initial_partition =
            new Dungeon_KDTree_Partition
            (
                new Noise_Position(width, height)
            );

        Dungeon__KDTREE =
            new Dungeon_KDTree
            (
                initial_partition
            );

        int generated_rooms = 0;
        while(generated_rooms < room_count)
        {
            foreach(Dungeon_KDTree_Node endpoint in Dungeon__KDTREE.Endpoints)
            {
                Noise_Position? nullable_partition_key = 
                    Suggest_Partition(endpoint.Node__PARTITION);

                if (nullable_partition_key != null)
                {
                    generated_rooms++;
                    Dungeon__KDTREE.Partition((Noise_Position)nullable_partition_key);
                }
            }
        }

        IEnumerable<Dungeon_KDTree_Node> endpoints =
            Dungeon__KDTREE.Endpoints;

        Dungeon__HALLWAY_GRAPH =
            new Dungeon_Hallway_Graph
            (
                Dungeon__KDTREE
            );

        Dungeon_KDTree_Node initial_shared_space =
            Get_Initial_Room(endpoints);

        int initial_room_vertex = 
            Dungeon__HALLWAY_GRAPH
            .Get_Vertex_From_Position(initial_shared_space.Node__PARTITIONING_KEY);

        Dungeon__HALLWAY_LINKER =
            new Dungeon_Hallway_Linker
            (
                Dungeon__HALLWAY_GRAPH,
                Dungeon__KDTREE.Keys,
                initial_room_vertex
            );

        IEnumerable<Dungeon_Hallway_Graph_Edge> linked_edges =
            Dungeon__HALLWAY_LINKER.Get_Linked_Edges();

        foreach(Dungeon_KDTree_Node endpoint in endpoints)
        {
            if (endpoint.Node__Left == null)
                Construct_Room_Cells(endpoint.Node__Left_Partition, endpoint.Node__PARTITIONING_KEY);
            if (endpoint.Node__Right == null)
                Construct_Room_Cells(endpoint.Node__Right_Partition, endpoint.Node__PARTITIONING_KEY);
        }

        Construct_Hallway_Cells(linked_edges);
    }

    protected abstract void Construct_Room_Cells(Dungeon_KDTree_Partition room_partition, Noise_Position hallway_link);

    protected abstract void Construct_Hallway_Cells(IEnumerable<Dungeon_Hallway_Graph_Edge> hallway_edges);

    protected abstract Dungeon_KDTree_Node Get_Initial_Room(IEnumerable<Dungeon_KDTree_Node> endpoints);

    protected abstract Noise_Position? Suggest_Partition(Dungeon_KDTree_Partition space);

    /// <summary>
    /// This should return all cells of Hallways, and Rooms.
    /// </summary>
    protected abstract IEnumerable<Dungeon_Cell> Get_Cells();
}
