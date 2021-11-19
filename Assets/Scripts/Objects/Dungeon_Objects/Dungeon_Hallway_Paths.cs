
using System.Collections.Generic;

/// <summary>
/// Using a constructed Dungeon_Hallway_Graph and a vertex
/// to serve as "source" - Dungeon_Hallway_Paths finds all
/// paths to all other vertices from source. This records
/// the distance, and whether or not a given vertex is
/// reachable.
///
/// To link a room to an initial room, create a DHP of
/// the initial room's doorway to a target room's doorway.
///
/// To handle overlapping hallways use Dungeon_Hallway_Linker.
/// </summary>
public sealed class Dungeon_Hallway_Paths
{
    public int Paths__SOURCE { get; }

    public int Paths__VERTICES { get; }
    private bool[] Paths__MARKED { get; }
    public bool Has_Path_To(int vertex)
        => Paths__MARKED[vertex];
    private int[]  Paths__DISTANCE_TO { get; }
    public int Get_Distance_To(int vertex)
        => Paths__DISTANCE_TO[vertex];

    internal Dungeon_Hallway_Paths
    (
        Dungeon_Hallway_Graph hallway_Graph,
        int source
    )
    {
        Paths__SOURCE = source;

        Paths__VERTICES = hallway_Graph.Graph__VERTICES;

        Paths__MARKED = new bool[hallway_Graph.Graph__VERTICES];
        Paths__DISTANCE_TO = new int[hallway_Graph.Graph__VERTICES];

        for(int v=0;v<hallway_Graph.Graph__VERTICES;v++)
        {
            Paths__MARKED[v] = v == source;
            Paths__DISTANCE_TO[v] = (Paths__MARKED[v]) ? 0 : int.MaxValue;
        }

        Find_Paths__Breadth_First_Search(hallway_Graph, source);
    }

    private void Find_Paths__Breadth_First_Search(Dungeon_Hallway_Graph hallway_Graph, int source)
    {
        Queue<Dungeon_Hallway_Graph_Edge> edges =
            new Queue<Dungeon_Hallway_Graph_Edge>
            (
                hallway_Graph.Get_Adjacent_Edges(source)
            );

        while(edges.Count > 0)
        {
            Dungeon_Hallway_Graph_Edge edge = edges.Dequeue();

            if (Paths__MARKED[edge.Edge__TO])
                continue;

            Paths__MARKED[edge.Edge__TO] = true;

            int distance_From = Paths__DISTANCE_TO[edge.Edge__FROM];
            int distance_To = distance_From + edge.Edge__WEIGHT;

            Paths__DISTANCE_TO[edge.Edge__TO] = distance_To;

            foreach(Dungeon_Hallway_Graph_Edge adjacentEdge in hallway_Graph.Get_Adjacent_Edges(edge.Edge__TO))
                edges.Enqueue(adjacentEdge);
        }
    }
}
