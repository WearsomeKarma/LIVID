
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class Dungeon_Hallway_Linker
{
    /// <summary>
    /// Used for determining if an edge was already linked.
    /// </summary>
    private Dictionary<Dungeon_Hallway_Graph_Edge, bool> Linker__EDGE_TABLE { get; }
    private List<Dungeon_Hallway_Graph_Edge> Linker__EDGES { get; }
    public IEnumerable<Dungeon_Hallway_Graph_Edge> Get_Linked_Edges()
        => Linker__EDGES.ToList();

    internal Dungeon_Hallway_Linker
    (
        Dungeon_Hallway_Graph hallway_Graph,
        IEnumerable<Noise_Position> room_vertices,
        int primary_source
    )
    {
        Dungeon_Hallway_Paths source_paths =
            new Dungeon_Hallway_Paths
            (
                hallway_Graph,
                primary_source
            );

        foreach(Noise_Position room_vertex_position in room_vertices)
        {
            int room_vertex = hallway_Graph.Get_Vertex_From_Position(room_vertex_position); 
            Link_Rooms(hallway_Graph, source_paths, room_vertex);
        }
    }

    internal Dungeon_Hallway_Linker
    (
        Dungeon_Hallway_Graph hallway_Graph,
        IEnumerable<int> room_vertices,
        int primary_source
    )
    {
        Dungeon_Hallway_Paths source_paths =
            new Dungeon_Hallway_Paths
            (
                hallway_Graph,
                primary_source
            );

        foreach(int room_vertex in room_vertices)
        {
            Link_Rooms(hallway_Graph, source_paths, room_vertex);
        }
    }

    private void Link_Rooms
    (
        Dungeon_Hallway_Graph hallway_Graph, 
        Dungeon_Hallway_Paths source_paths,
        int room_vertex
    )
    {
        int target = source_paths.Paths__SOURCE;
        int vertex = room_vertex;

        while(vertex != target)
            Link_Edge(hallway_Graph, source_paths, ref vertex);
    }
    
    private void Link_Edge
    (
        Dungeon_Hallway_Graph hallway_Graph,
        Dungeon_Hallway_Paths source_paths,
        ref int vertex
    )
    {
        IEnumerable<Dungeon_Hallway_Graph_Edge> vertex_edges =
            hallway_Graph.Get_Adjacent_Edges(vertex);

        int closest_distance = int.MaxValue;
        Dungeon_Hallway_Graph_Edge? nullable_closest_edge = null;

        foreach(Dungeon_Hallway_Graph_Edge edge in vertex_edges)
        {
            int dist = source_paths.Get_Distance_To(edge.Edge__TO);
            if (dist < closest_distance)
            {
                closest_distance = dist;
                vertex = edge.Edge__TO;
                nullable_closest_edge = edge;
            }
        }

        if (nullable_closest_edge == null)
            throw new InvalidOperationException("No edges detected - is the KDTree for the dungeon invalid?");

        Dungeon_Hallway_Graph_Edge closest_edge = (Dungeon_Hallway_Graph_Edge)nullable_closest_edge;

        if (Linker__EDGE_TABLE.ContainsKey(closest_edge))
            return;

        Linker__EDGE_TABLE.Add(closest_edge, true);
        Linker__EDGES.Add(closest_edge);
    }
}
