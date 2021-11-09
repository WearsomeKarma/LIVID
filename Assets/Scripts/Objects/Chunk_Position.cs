
public struct Chunk_Position
{
    public int CHUNK_X { get; }
    public int CHUNK_Z { get; }

    public Chunk_Position(int chunkX, int chunkZ)
    {
        CHUNK_X = chunkX;
        CHUNK_Z = chunkZ;
    }

    public override string ToString()
        => $"Chunk_Position: ({CHUNK_X}, {CHUNK_Z})";

    public bool Equals(Chunk_Position target)
    {
        return 
            CHUNK_X == target.CHUNK_X
            &&
            CHUNK_Z == target.CHUNK_Z;
    }

    public static Chunk_Position operator +(Chunk_Position c1, Chunk_Position c2)
        => new Chunk_Position(c1.CHUNK_X + c2.CHUNK_X, c1.CHUNK_Z + c2.CHUNK_Z);

    public static int Distance_Squared(Chunk_Position point, Chunk_Position target)
    {
        int deltaX = (target.CHUNK_X - point.CHUNK_X);
        int deltaZ = (target.CHUNK_Z - point.CHUNK_Z);

        return (deltaX * deltaX) + (deltaZ * deltaZ);
    }

    public static Chunk_Position Offset(Chunk_Position c, int deltaChunkX, int deltaChunkY)
        => new Chunk_Position(c.CHUNK_X + deltaChunkX, c.CHUNK_Z + deltaChunkY);

    public static Chunk_Position Modulo(Chunk_Position c, int range)
        => new Chunk_Position(c.CHUNK_X % range, c.CHUNK_Z % range);

    public static Chunk_Position Positive_Modulo(Chunk_Position c, int range)
        => 
        Modulo
        (
            Offset
            (
                Modulo
                (
                    c,
                    range
                ),
                range,
                range
            ),
            range
        );
}
