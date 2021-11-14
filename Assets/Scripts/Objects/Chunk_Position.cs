using UnityEngine;

public struct Noise_Position
{
    public int NOISE_X { get; }
    public int NOISE_Z { get; }

    public Noise_Position(Vector3 position)
    {
        NOISE_X = (int)position.x;
        NOISE_Z = (int)position.z;
    }

    public Noise_Position(int noiseX, int noiseZ)
    {
        NOISE_X = noiseX;
        NOISE_Z = noiseZ;
    }

    public override string ToString()
        => $"Noise_Position: ({NOISE_X}, {NOISE_Z})";

    public bool Equals(Noise_Position target)
    {
        return 
            NOISE_X == target.NOISE_X
            &&
            NOISE_Z == target.NOISE_Z;
    }

    public static Noise_Position operator +(Noise_Position c1, Noise_Position c2)
        => new Noise_Position(c1.NOISE_X + c2.NOISE_X, c1.NOISE_Z + c2.NOISE_Z);
    public static Noise_Position operator -(Noise_Position c1, Noise_Position c2)
        => new Noise_Position(c1.NOISE_X - c2.NOISE_X, c1.NOISE_Z - c2.NOISE_Z);
    public static Noise_Position operator *(int scalar, Noise_Position c1)
        => new Noise_Position(c1.NOISE_X*scalar, c1.NOISE_Z*scalar);
    public static Noise_Position operator *(Noise_Position c1, int scalar)
        => new Noise_Position(c1.NOISE_X*scalar, c1.NOISE_Z*scalar);
    public static Noise_Position operator /(Noise_Position c1, int divisor)
        => new Noise_Position(c1.NOISE_X/divisor, c1.NOISE_Z/divisor);

    public static int Distance_Squared(Noise_Position point, Noise_Position target)
    {
        int deltaX = (target.NOISE_X - point.NOISE_X);
        int deltaZ = (target.NOISE_Z - point.NOISE_Z);

        return (deltaX * deltaX) + (deltaZ * deltaZ);
    }

    public static Noise_Position Offset(Noise_Position c, int deltaChunkX, int deltaChunkY)
        => new Noise_Position(c.NOISE_X + deltaChunkX, c.NOISE_Z + deltaChunkY);

    public static Noise_Position Modulo(Noise_Position c, int range)
        => new Noise_Position(c.NOISE_X % range, c.NOISE_Z % range);

    public static Noise_Position Positive_Modulo(Noise_Position c, int range)
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
