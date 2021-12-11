
using System.Collections.Generic;

public struct Dungeon_KDTree_Partition
{
    public Noise_Position Partition__Key { get; internal set; }
    public Noise_Position Partition__MIN { get; }
    public Noise_Position Partition__MAX { get; }

    public int Partition__MIN_X => Partition__MIN.NOISE_X; 
    public int Partition__MAX_X => Partition__MAX.NOISE_X;
    public int Partition__MIN_Z => Partition__MIN.NOISE_Z;
    public int Partition__MAX_Z => Partition__MAX.NOISE_Z;

    public int Partition__AREA
        => (Partition__MAX_X - Partition__MIN_X) * (Partition__MAX_Z - Partition__MIN_Z);

    /// <summary>
    /// This is to construct the root partition.
    /// </summary>
    internal Dungeon_KDTree_Partition(Noise_Position size)
    : this(new Noise_Position(), size)
    {}

    internal Dungeon_KDTree_Partition
    (
        Dungeon_KDTree_Partition parent, 
        Noise_Position max
    )
    : this(parent.Partition__MIN, max)
    {}

    internal Dungeon_KDTree_Partition
    (
        Noise_Position min,
        Dungeon_KDTree_Partition parent
    )
    : this(min, parent.Partition__MAX)
    {}

    private Dungeon_KDTree_Partition
    (
        Noise_Position min, 
        Noise_Position max
    )
    {
        Partition__Key = new Noise_Position();
        Partition__MIN = min;
        Partition__MAX = max;
    }

    public IEnumerable<Noise_Position> Get_Positions()
    {
        int row = Partition__MIN_Z, col = Partition__MIN_X;

        while(row < Partition__MAX_Z)
        {
            yield return new Noise_Position(col, row);
            col++;
            if (col >= Partition__MAX_X)
            {
                col = Partition__MIN_X;
                row++;
            }
        }
        yield break;
    }

    public bool Contains_Point(Noise_Position point)
    {
        bool boundedX = 
            point.NOISE_X < Partition__MAX_X
            &&
            point.NOISE_X > Partition__MIN_X;
        bool boundedZ =
            point.NOISE_Z < Partition__MAX_Z
            &&
            point.NOISE_Z > Partition__MIN_Z;

        return boundedX && boundedZ;
    }

    /// <summary>
    /// Partitioning on X means we will copy over
    /// the key's X for the MAX_X.
    /// </summary>
    public static Dungeon_KDTree_Partition Partition_X_Greater
    (
        Dungeon_KDTree_Partition parent,
        Noise_Position right_greater
    )
    {
        Noise_Position max =
            new Noise_Position
            (
                right_greater.NOISE_X,
                parent.Partition__MAX_Z
            );

        return new Dungeon_KDTree_Partition(parent, right_greater);
    }

    public static Dungeon_KDTree_Partition Partition_Z_Greater
    (
        Dungeon_KDTree_Partition parent,
        Noise_Position right_greater
    )
    {
        Noise_Position max =
            new Noise_Position
            (
                parent.Partition__MAX_X,
                right_greater.NOISE_Z
            );

        return new Dungeon_KDTree_Partition(parent, right_greater);
    }

    public static Dungeon_KDTree_Partition Partition_X_Lesser
    (
        Noise_Position left_lesser,
        Dungeon_KDTree_Partition parent
    )
    {
        Noise_Position max =
            new Noise_Position
            (
                left_lesser.NOISE_X,
                parent.Partition__MIN_Z
            );

        return new Dungeon_KDTree_Partition(left_lesser, parent);
    }

    public static Dungeon_KDTree_Partition Partition_Z_Lesser
    (
        Noise_Position left_lesser,
        Dungeon_KDTree_Partition parent
    )
    {
        Noise_Position max =
            new Noise_Position
            (
                parent.Partition__MIN_X,
                left_lesser.NOISE_Z
            );

        return new Dungeon_KDTree_Partition(left_lesser, parent);
    }
}
