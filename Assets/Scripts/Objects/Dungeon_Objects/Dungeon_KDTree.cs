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

    private List<Dungeon_KDTree_Partition> Dungeon_KDTree__ENDPOINT_PARTITIONS { get; }
    public IEnumerable<Dungeon_KDTree_Partition> Endpoints_Partitions
        => Dungeon_KDTree__ENDPOINT_PARTITIONS.ToList();

    private Dictionary<Noise_Position, Dungeon_KDTree_Node> Dungeon_KDTree__NODE_LOOKUP { get; }
    public Dungeon_KDTree_Node Get_Node(Dungeon_KDTree_Partition partition)
        => Dungeon_KDTree__NODE_LOOKUP[partition.Partition__Key];

    private List<Noise_Position> Dungeon_KDTree__KEYS { get; }
    public IEnumerable<Noise_Position> Keys
        => Dungeon_KDTree__KEYS.ToList();
    public int Key_Count
        => Dungeon_KDTree__KEYS.Count;

    internal Dungeon_KDTree
    (
        Dungeon_KDTree_Partition partitioning_space
    )
    {
        Dungeon_KDTree__SPACE =
            partitioning_space;

        Dungeon_KDTree__Endpoint_Count = 1;
        Dungeon_KDTree__Room_Count = 1;

        Dungeon_KDTree__KEYS = new List<Noise_Position>();

        Dungeon_KDTree__ENDPOINT_PARTITIONS =
            new List<Dungeon_KDTree_Partition>() {partitioning_space};

        Dungeon_KDTree__NODE_LOOKUP =
            new Dictionary<Noise_Position, Dungeon_KDTree_Node>();
    }

    internal void Partition(Noise_Position position)
    {
        Dungeon_KDTree__KEYS
            .Add(position);

        Dungeon_KDTree_Node node;
        if (Dungeon_KDTree__Root == null)
        {
            // this code is pretty awful but oh well.
            Dungeon_KDTree_Partition root_partition = new Dungeon_KDTree_Partition(Dungeon_KDTree__SPACE.Partition__MAX);
            root_partition.Partition__Key = position;
            node = Dungeon_KDTree__Root = new Dungeon_KDTree_Node(position, root_partition);
            Dungeon_KDTree__ENDPOINT_PARTITIONS[0] = root_partition;
        }
        else
            node = Traverse_Partition(position);

        Dungeon_KDTree__NODE_LOOKUP.Add(node.Node__PARTITIONING_KEY, node);
        
        Dungeon_KDTree__Room_Count+=2;
        Dungeon_KDTree__Endpoint_Count++;
        
        Dungeon_KDTree__ENDPOINT_PARTITIONS.Remove(node.Node__PARTITION);
        Dungeon_KDTree__ENDPOINT_PARTITIONS.Add(node.Node__Left_Partition);
        Dungeon_KDTree__ENDPOINT_PARTITIONS.Add(node.Node__Right_Partition);
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
            return node.Create_Partition_Left(position);

        if (isRight)
            return node.Create_Partition_Right(position);

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

        if (destination == null)
            return null;

        while(true)
        {
            int compare = destination.Compare(position);

            if (compare < 0)
            {
                if (isLeft = destination.Node__Left == null)
                    break;
                destination = destination.Node__Left;
                continue;
            }
            if (compare > 0)
            {
                if (isRight = destination.Node__Right == null)
                    break;
                destination = destination.Node__Right;
                continue;
            }
            break;
        }

        return destination;
    }
}
