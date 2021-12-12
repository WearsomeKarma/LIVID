using System;
using System.Collections.Generic;

/// <summary>
/// This is a directionless acyclic graph with weights.
/// It represents the possible hallway paths.
/// To use this, first construct a KDTree representing
/// the spatial paritioning of the dungeon level. Next
/// pass that KDTree into Dungeon_Hallway_Graph constructor.
/// Once constructed, pass it into the constructor for
/// Dungeon_Hallway_MST.
/// </summary>
public sealed class Dungeon_Hallway_Graph
{
    public int Graph__VERTICES { get; }
    public int Graph__EDGES { get; }

    private Dictionary<Noise_Position, int> Graph__Position_Onto_Vertex_Table { get; }
    internal int Get_Vertex_From_Position(Noise_Position position)
        => Graph__Position_Onto_Vertex_Table[position];
    internal bool Contains_Position(Noise_Position position)
        => Graph__Position_Onto_Vertex_Table.ContainsKey(position);

    private List<Dungeon_Hallway_Graph_Edge>[] Graph__ADJACENT { get; }
    internal IEnumerable<Dungeon_Hallway_Graph_Edge> Get_Adjacent_Edges(int v)
        => Graph__ADJACENT[v];

    public Dungeon_Hallway_Graph
    (
        Dungeon_KDTree dungeon_KDTree,
        int extra_rooms = 0
    )
    {
        int number_of_rooms     = dungeon_KDTree.Dungeon_KDTree__Room_Count;
        int number_of_endpoints = dungeon_KDTree.Dungeon_KDTree__Endpoint_Count;

        // For 6 rooms, 22.
        Graph__VERTICES    = 4 + (3 * dungeon_KDTree.Key_Count);
        Graph__EDGES        = Graph__VERTICES - 1;

        Graph__Position_Onto_Vertex_Table =
            new Dictionary<Noise_Position, int>();

        Graph__ADJACENT = new List<Dungeon_Hallway_Graph_Edge>[Graph__VERTICES];

        Map_Tree_Recursively(dungeon_KDTree.Dungeon_KDTree__Root); 
    }

    internal void Create_Edge(Dungeon_Hallway_Graph_Edge edge)
    {
        Assert_Vertex_Record(edge.Position__FROM);
        Assert_Vertex_Record(edge.Position__TO);

        Add_Edge(edge);
    }

    private void Map_Tree_Recursively
    (
        Dungeon_KDTree_Node node
    )
    {
        if (node == null)
            return;
        if (node.Node__Right == null)
            Map_Room(node.Node__Right_Partition, node.Node__PARTITIONING_KEY);
        else
            Map_Tree_Recursively(node.Node__Right);

        if (node.Node__Left == null)
            Map_Room(node.Node__Left_Partition, node.Node__PARTITIONING_KEY);
        else
            Map_Tree_Recursively(node.Node__Left);
    }

    private void Map_Room
    (
        Dungeon_KDTree_Partition partition, 
        Noise_Position partition_key
    )
    {
        Noise_Position minXminZ = partition.Partition__MIN;
        Noise_Position minXmaxZ = new Noise_Position(partition.Partition__MIN_X, partition.Partition__MAX_Z);
        Noise_Position maxXminZ = new Noise_Position(partition.Partition__MAX_X, partition.Partition__MIN_Z);
        Noise_Position maxXmaxZ = partition.Partition__MAX;

        Map_Edge(minXminZ, minXmaxZ, partition_key);
        Map_Edge(minXmaxZ, maxXmaxZ, partition_key);
        Map_Edge(maxXmaxZ, maxXminZ, partition_key);
        Map_Edge(maxXminZ, minXminZ, partition_key);
    }

    private void Map_Edge(Noise_Position to, Noise_Position from, Noise_Position key)
    {
        if (Check_If_Edge_Has_Key(from, to, key))
        {
            Add_Edge(to, key);
            Add_Edge(key, from);
            return;
        }

        Add_Edge(to, from);
    }

    private void Assert_Vertex_Record(Noise_Position vertex)
    {
        if (!Graph__Position_Onto_Vertex_Table.ContainsKey(vertex))
        {
            int vertIndex = Graph__Position_Onto_Vertex_Table.Keys.Count;
            if (vertIndex >= Graph__VERTICES)
                throw new InvalidOperationException("Too many vertices are being recorded as opposed to what was initialized. Use extra_rooms in ctor?");
            Graph__Position_Onto_Vertex_Table.Add(vertex, vertIndex);
        }
    }

    private bool Check_If_Edge_Has_Key
    (
        Noise_Position from,
        Noise_Position to,
        Noise_Position key
    )
    {
        bool sameX = key.NOISE_X == to.NOISE_X;
        bool sameZ = key.NOISE_Z == to.NOISE_Z;

        if (sameX)
            return 
                (key.NOISE_Z > from.NOISE_Z && key.NOISE_Z < to.NOISE_Z)
                ||
                (key.NOISE_Z > to.NOISE_Z && key.NOISE_Z < from.NOISE_Z);
        if (sameZ)
            return 
                (key.NOISE_X > from.NOISE_X && key.NOISE_X < to.NOISE_X)
                ||
                (key.NOISE_X > to.NOISE_X && key.NOISE_X < from.NOISE_X);
        return false;
    }

    private void Add_Edge
    (
        Noise_Position to,
        Noise_Position from
    )
    {
        Assert_Vertex_Record(to);
        Assert_Vertex_Record(from);

        int v = Get_Vertex_From_Position(to);
        int w = Get_Vertex_From_Position(from);

        Dungeon_Hallway_Graph_Edge edge =
            new Dungeon_Hallway_Graph_Edge
            (
                v, to,
                w, from
            );

        Add_Edge(edge);
    }

    private void Add_Edge
    (
        Dungeon_Hallway_Graph_Edge edge
    )
    {
        int from = edge.Edge__FROM;
        if (Graph__ADJACENT[from] == null)
            Graph__ADJACENT[from] = new List<Dungeon_Hallway_Graph_Edge>();
        Graph__ADJACENT[from].Add(edge);
    }
}
