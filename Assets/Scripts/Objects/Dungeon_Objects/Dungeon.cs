
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
    private int Dungeon_ROOM_COUNT { get; }

    private Dungeon_KDTree Dungeon__KDTREE { get; }
    private Dungeon_Hallway_Graph Dungeon__Hallway_Graph { get; set; }
    private Dungeon_Hallway_Linker Dungeon__Hallway_Linker { get; set; }

    //private Dictionary<Dungeon_KDTree_Node, Dungeon_Room> Dungeon__ROOMS { get; }

    public Dungeon(int width, int height, int room_count)
    {
        Dungeon_ROOM_COUNT = room_count;

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
    }

    internal void Generate_Dungeon()
    {
        int generated_rooms = 0;
        UnityEngine.Debug.Log("Generating Rooms.");
        while(generated_rooms < Dungeon_ROOM_COUNT)
        {
            foreach(Dungeon_KDTree_Partition endpoint_partition in Dungeon__KDTREE.Endpoints_Partitions)
            {
                if (generated_rooms >= Dungeon_ROOM_COUNT)
                    break;
                
                UnityEngine.Debug.Log($"Suggesting a partition on {endpoint_partition}");
                Noise_Position? nullable_partition_key = 
                    Suggest_Partition(endpoint_partition);

                if (nullable_partition_key != null)
                {
                    UnityEngine.Debug.Log("Generating the suggested parition!");
                    generated_rooms++;
                    Dungeon__KDTREE.Partition((Noise_Position)nullable_partition_key);
                }
            }
        }

        UnityEngine.Debug.Log("Getting Endpoints.");
        IEnumerable<Dungeon_KDTree_Partition> endpoint_partitions =
            Dungeon__KDTREE.Endpoints_Partitions;

        UnityEngine.Debug.Log("Creating Hallway graph.");
        Dungeon__Hallway_Graph =
            new Dungeon_Hallway_Graph
            (
                Dungeon__KDTREE
            );

        Dungeon_KDTree_Partition initial_shared_space =
            Get_Initial_Room(endpoint_partitions);

        Dungeon_KDTree_Node initial_node =
            Dungeon__KDTREE.Get_Node(initial_shared_space);

        int initial_room_vertex = 
            Dungeon__Hallway_Graph
            .Get_Vertex_From_Position(initial_node.Node__PARTITIONING_KEY);

        UnityEngine.Debug.Log("Linking Hallways");
        Dungeon__Hallway_Linker =
            new Dungeon_Hallway_Linker
            (
                Dungeon__Hallway_Graph,
                Dungeon__KDTREE.Keys,
                initial_room_vertex
            );

        IEnumerable<Dungeon_Hallway_Graph_Edge> linked_edges =
            Dungeon__Hallway_Linker.Get_Linked_Edges();

        UnityEngine.Debug.Log("Creating rooms in partitions.");
        foreach(Dungeon_KDTree_Partition endpoint_partition in endpoint_partitions)
        {
            Construct_Room_Cells(endpoint_partition, endpoint_partition.Partition__Key);
        }

        UnityEngine.Debug.Log("Creating hallway cells.");
        //Construct_Hallway_Cells(linked_edges);
    }

    protected abstract void Construct_Room_Cells(Dungeon_KDTree_Partition room_partition, Noise_Position hallway_link);

    protected abstract void Construct_Hallway_Cells(IEnumerable<Dungeon_Hallway_Graph_Edge> hallway_edges);

    protected abstract Dungeon_KDTree_Partition Get_Initial_Room(IEnumerable<Dungeon_KDTree_Partition> endpoint_partitions);

    protected abstract Noise_Position? Suggest_Partition(Dungeon_KDTree_Partition space);

    /// <summary>
    /// This should return all cells of Hallways, and Rooms.
    /// </summary>
    internal abstract IEnumerable<Dungeon_Cell> Get_Cells();

    internal abstract IEnumerable<Dungeon_Runtime_Cell> Generate_Runtime_Cells
    (
        Dungeon_Schematic schematic
    );
}
