using System;
using System.Collections.Generic;

public struct Dungeon_Hallway_Graph_Edge
{
    public int Edge__TO { get; }
    public int Edge__FROM { get; }

    public Noise_Position Position__TO { get; }
    public Noise_Position Position__FROM { get; }

    public int Edge__WEIGHT { get; }

    public Dungeon_Hallway_Graph_Edge
    (
        //V - From
        int v,
        Noise_Position pos_v,
        //W - To
        int w,
        Noise_Position pos_w
    )
    {
        int deltaX = System.Math.Abs(pos_w.NOISE_X - pos_v.NOISE_X);
        int deltaZ = System.Math.Abs(pos_w.NOISE_Z - pos_v.NOISE_Z);

        if (deltaX != 0 && deltaZ != 0)
        {
            throw new ArgumentException($"Edges V:[{v} - {pos_v}] and W:[{w} - {pos_w}] do not form a straight line.");
        }

        Edge__TO = w;
        Edge__FROM = v;

        Position__TO = pos_w;
        Position__FROM = pos_v;

        Edge__WEIGHT = deltaX + deltaZ;
    }

    public override int GetHashCode()
    {
        // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
        // Jon Skeet for the prime number and hashing.
        unchecked
        {
            int hash = (int) 2166136261;

            hash = (hash * 16777619) ^ Position__TO.GetHashCode();
            hash = (hash * 16777619) ^ Position__FROM.GetHashCode();

            return hash;
        }
    }

    public IEnumerable<Noise_Position> Get_Edge_Points()
    {
        Noise_Position delta = Position__TO - Position__FROM;
        Noise_Position current = Position__FROM;

        while(!current.Equals(Position__TO))
            yield return (current = Position__FROM + delta);

        yield break;
    }
}
