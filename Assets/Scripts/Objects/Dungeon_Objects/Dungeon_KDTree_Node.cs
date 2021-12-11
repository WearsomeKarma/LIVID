
using System;

public sealed class Dungeon_KDTree_Node
{
    public Noise_Position Node__PARTITIONING_KEY { get; }
    public Dungeon_KDTree_Partition Node__PARTITION { get; }
    private int Node__DEPTH { get; }
    private bool Node__IS_X_OR_Y
        => Node__DEPTH % 2 == 0;

    public Dungeon_KDTree_Node Node__Left { get; private set; }
    public Dungeon_KDTree_Node Node__Right { get; private set; }
    public bool Node__Is_Endpoint
        =>Node__Left == null || Node__Right == null;

    public Dungeon_KDTree_Partition Node__Left_Partition { get; set; }
    public Dungeon_KDTree_Partition Node__Right_Partition { get; set; }

    private bool Node_Is_Leaf
        => Node__Left == null && Node__Right == null;

    internal Dungeon_KDTree_Node
    (
        Noise_Position partitioning_Key,
        Dungeon_KDTree_Partition initalSpace
    )
    : this(partitioning_Key, initalSpace, 0)
    {
    }

    private Dungeon_KDTree_Node
    (
        Noise_Position partitioning_Key,
        Dungeon_KDTree_Partition partition,
        int depth
    )
    {
        Node__PARTITIONING_KEY = 
            partitioning_Key;

        Node__PARTITION = partition;
        Node__DEPTH = depth;

        int key_x = Node__PARTITIONING_KEY.NOISE_X;
        int key_z = Node__PARTITIONING_KEY.NOISE_Z;

        Noise_Position left_max, right_min;

        if (Node__IS_X_OR_Y)
        {
            //is biased on X
            left_max =
                new Noise_Position(key_x, Node__PARTITION.Partition__MAX_Z);
            right_min =
                new Noise_Position(key_x, Node__PARTITION.Partition__MIN_Z);

            Set_Partitions(left_max, right_min);

            return;
        }

        //is biased on Z
        left_max =
            new Noise_Position(Node__PARTITION.Partition__MAX_X, key_z);
        right_min =
            new Noise_Position(Node__PARTITION.Partition__MIN_X, key_z);

        Set_Partitions(left_max, right_min);
    }

    internal int Compare(Noise_Position position)
    {
        if(Node__IS_X_OR_Y)
        {
            int deltaX = position.NOISE_X - Node__PARTITIONING_KEY.NOISE_X;

            return deltaX;
        }

        int deltaZ = position.NOISE_Z - Node__PARTITIONING_KEY.NOISE_Z;

        return deltaZ;
    }

    private void Set_Partitions
    (
        Noise_Position left_max,
        Noise_Position right_min
    )
    {
        Node__Left_Partition =
            new Dungeon_KDTree_Partition
            (
                Node__PARTITION,
                left_max
            ) {Partition__Key = Node__PARTITIONING_KEY};
        Node__Right_Partition =
            new Dungeon_KDTree_Partition
            (
                right_min,
                Node__PARTITION
            ) {Partition__Key = Node__PARTITIONING_KEY};
    }

    internal Dungeon_KDTree_Node Create_Partition_Left(Noise_Position key)
    {
        if (Node__Left != null)
            throw new InvalidOperationException("Left Node was already partitioned.");
        return Node__Left =
            new Dungeon_KDTree_Node 
            (
                key,
                Node__Left_Partition,
                Node__DEPTH+1
            );
    }

    internal Dungeon_KDTree_Node Create_Partition_Right(Noise_Position key)
    {
        if (Node__Right != null)
            throw new InvalidOperationException("Right Node was already partitioned.");
        return Node__Right =
            new Dungeon_KDTree_Node
            (
                key,
                Node__Right_Partition,
                Node__DEPTH+1
            );
    }
}
