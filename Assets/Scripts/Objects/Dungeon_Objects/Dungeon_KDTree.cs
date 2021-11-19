using System;
using System.Collections.Generic;
using System.Linq;

public sealed class Dungeon_KDTree
{
    public const int CORNER_COUNT = 4;

    internal Dungeon_KDTree_Partition Dungeon_KDTree__SPACE { get; }
    internal Dungeon_KDTree_Node Dungeon_KDTree__Root { get; private set; }
    public int Dungeon_KDTree__Room_Count { get; private set; }
    /// <summary>
    /// This refers to the number of nodes which has at least one null branch.
    /// </summary>
    public int Dungeon_KDTree__Endpoint_Count { get; private set; }

    private List<Dungeon_KDTree_Node> Dungeon_KDTree__ENDPOINTS { get; }
    public IEnumerable<Dungeon_KDTree_Node> Endpoints
        => Dungeon_KDTree__ENDPOINTS.ToList();

    private List<Noise_Position> Dungeon_KDTree__KEYS { get; }
    public IEnumerable<Noise_Position> Keys
        => Dungeon_KDTree__KEYS.ToList();

    internal Dungeon_KDTree
    (
        Dungeon_KDTree_Partition initial_partition
    )
    {
        Dungeon_KDTree__SPACE =
            initial_partition;

        Dungeon_KDTree__Endpoint_Count = 1;
        Dungeon_KDTree__Room_Count = 1;

        Dungeon_KDTree__ENDPOINTS =
            new List<Dungeon_KDTree_Node>();
    }

    internal void Partition(Noise_Position position)
    {
        Dungeon_KDTree__KEYS
            .Add(position);

        Dungeon_KDTree_Node node;
        if (Dungeon_KDTree__Root == null)
            node = Initial_Partition(position);
        else
            node = Traverse_Partition(position);
        
        Dungeon_KDTree__Room_Count++;
        Dungeon_KDTree__Endpoint_Count++;
        if (node.Node__Left != null && node.Node__Right != null)
        {
            Dungeon_KDTree__ENDPOINTS.Remove(node);
            Dungeon_KDTree__Endpoint_Count--;
        }
    }

    private Dungeon_KDTree_Node Initial_Partition(Noise_Position root_key)
    {
        Dungeon_KDTree__Root =
            new Dungeon_KDTree_Node
            (
                root_key,
                Dungeon_KDTree__SPACE
            );

        Dungeon_KDTree__ENDPOINTS
            .Add(Dungeon_KDTree__Root);

        return Dungeon_KDTree__Root;
    }

    private Dungeon_KDTree_Node Traverse_Partition(Noise_Position position)
    {
        bool isLeft, isRight, isSelf;

        Dungeon_KDTree_Node node =
            Traverse(position, out isLeft, out isRight, out isSelf);
        if (isSelf)
        {
            throw new ArgumentException("Point already present in KDTree.");
        }

        if (isLeft)
            node.Create_Partition_Left(node.Node__PARTITIONING_KEY);

        if (isRight)
            node.Create_Partition_Right(node.Node__PARTITIONING_KEY);

        return node;
    }

    internal Dungeon_KDTree_Node Traverse
    (
        Noise_Position position,
        out bool isLeft,
        out bool isRight,
        out bool isSelf
    )
    {
        Dungeon_KDTree_Node destination =
            Dungeon_KDTree__Root;

        isLeft = false;
        isRight = false; 
        isSelf = false;
        bool isDestination = false;

        if (destination == null)
            return null;

        while(!isDestination)
        {
            destination = 
                destination
                .Traverse
                (
                    position, 
                    out isLeft,
                    out isRight,
                    out isSelf,
                    out isDestination
                );
        }

        return destination;
    }
}
