using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a grid of elements.
/// This is good for biome chunk heightmaps
/// as well as biome analysis.
/// </summary>
public class Spatial_Table<T>
    where T : Spatial_Element
{
    /// <summary>
    /// The NoiseMaps layed out in a local space.
    /// </summary>
    internal List<T> Element_Table { get; set; }
    /// <summary>
    /// The size of the table.
    /// </summary>
    public int Table_Size { get; private set; }

    private Noise_Position Noise_Position { get; set; } 
    private Noise_Position Noise_Offset { get; set; }

    /// <summary>
    /// The constant scale between worldspace integer
    /// positions to our NoiseMap spatial descriptions.
    /// So for example, a Noise_Table for chunks is a
    /// 1:16 correspondance, where the player's X and Y
    /// is mapped to a chunk position - that is in multiples
    /// of 16. For biomes, it could look like 1:128.
    /// </summary>
    // NOTE: Spatial_Scale should be multiples of some
    // generator number. So if you want a larger Noise_Table
    // to describe a smaller Noise_Table, the larger table's
    // Table_Size should be a MULTIPLE of the smaller table's
    // Table_Size.
    private int Spatial_Scale { get; set; }

    protected Func<Noise_Position, T> Element_Generator { get; set; }
    protected Action<Noise_Position> Element_Disposer { get; set; }

    public Spatial_Table
    (
        int tableSize,
        Noise_Position initalPosition,
        int scale,
        Func<Noise_Position, T> generator = null,
        Action<Noise_Position> disposer = null,
        Noise_Position? offset = null
    )
    {
        Table_Size = tableSize;
        Element_Table =  new List<T>();

        Noise_Position = initalPosition;

        Spatial_Scale = scale;

        Element_Generator = generator;
        Element_Disposer = disposer;

        Noise_Offset = offset ?? new Noise_Position();
    }

    internal virtual void Check_For_Updates
    (
        Vector3 worldSpaceCenter,
        out List<Noise_Position> invalidPositions,
        out List<Noise_Position> generatedPositions
    )
    {
        Noise_Position gamespace_Position =
            new Noise_Position((int)worldSpaceCenter.x, (int)worldSpaceCenter.z);
        Noise_Position updatePosition =
            Scale_To_Tablespace(gamespace_Position);

        bool isNotCriticalUpdate =
            Check_If__Is_Not_An_Update(updatePosition);

        if (isNotCriticalUpdate)
        {
            invalidPositions = null;
            generatedPositions = null;
            return;
        }

        Noise_Position = updatePosition;

        Get_Positions(out invalidPositions, out generatedPositions);

        bool isInitialGeneration = Element_Table.Count == 0;
        if (isInitialGeneration)
            generatedPositions = Get_Initial_Positions();

        Dispose_Positions(invalidPositions);
        Generate_Positions(generatedPositions);

        if (isInitialGeneration)
        {
            Handle_Initial_Generation();
        }
    }

    protected virtual void Handle_Initial_Generation(){}

    private void Generate_Positions(List<Noise_Position> generatedPositions)
    {
        foreach(Noise_Position position in generatedPositions)
        {
            T element = Element_Generator
            (
                position 
            );

            element.Spatial_Position = position;

            Element_Table.Add(element);
        }
    }

    private void Dispose_Positions(List<Noise_Position> invalidPositions)
    {
        foreach(Noise_Position position in invalidPositions)
        {
            if (Element_Disposer != null)
                Element_Disposer.Invoke(position);

            Element_Table.Remove(this[position]);
        }
    }

    private void Get_Positions
    (
        out List<Noise_Position> invalidPositions,
        out List<Noise_Position> generatedPositions
    )
    {
        List<Noise_Position> neededPositions = new List<Noise_Position>();

        for(int z=0;z<Table_Size;z++)
        {
            for(int x=0;x<Table_Size;x++)
            {
                Noise_Position position = 
                    new Noise_Position(x,z)
                    +
                    Noise_Position;

                neededPositions.Add(position);
            }
        }

        invalidPositions = new List<Noise_Position>();

        foreach(T element in Element_Table)
        {
            Noise_Position pos = new Noise_Position();
            if (neededPositions.Exists((p) => (pos=p).Equals(element.Spatial_Position)))
            {
                neededPositions.Remove(pos);
                continue;
            }
            invalidPositions.Add(element.Spatial_Position);
        }

        generatedPositions = neededPositions;
    }

    private List<Noise_Position> Get_Initial_Positions()
    {
        List<Noise_Position> positions = new List<Noise_Position>();

        if (Element_Table.Count > 0)
            return null;

        for(int z=0;z<Table_Size;z++)
        {
            for(int x=0;x<Table_Size;x++)
            {
                Noise_Position position =
                    new Noise_Position(x,z)
                    +
                    Noise_Offset;

                positions.Add(position);
            }
        }

        return positions;
    }

    private int Map_World_Space__To__Spatial_Context(float xOrZ)
    {
        return (int)(xOrZ) / Spatial_Scale;
    }

    private bool Check_If__Is_Not_An_Update(Noise_Position position)
    {
        return Noise_Position.Equals(position);
    }

    protected Noise_Position Scale_To_Gamespace(Noise_Position scaled_Position)
        => scaled_Position * Spatial_Scale;

    internal Noise_Position Scale_To_Tablespace(int x, int z)
        => Scale_To_Tablespace(new Noise_Position(x,z));

    internal Noise_Position Scale_To_Tablespace(Noise_Position position)
    {
        int x = Mathf.FloorToInt((position.NOISE_X / (float)Spatial_Scale));
        int z = Mathf.FloorToInt((position.NOISE_Z / (float)Spatial_Scale));
        
        //int x = position.NOISE_X / Spatial_Scale;
        //int z = position.NOISE_Z / Spatial_Scale;

        return new Noise_Position(x,z) + Noise_Offset;
    }

    internal T Get_With_Scale(Noise_Position gamespace_Position)
    {
        Noise_Position scaled_Position = Scale_To_Tablespace(gamespace_Position);

        return this[scaled_Position];
    }

    internal virtual T this[Noise_Position position]
    {
        get 
        {
            T element = Element_Table.Find((e) => e.Spatial_Position.Equals(position));

            return element;
        }
    }
    internal T this[int x, int z]
    {
        get 
        {
            Noise_Position position = new Noise_Position(x,z);

            return this[position];
        }
    }

    private int Get_Index(Noise_Position position)
        => position.NOISE_Z * Table_Size + position.NOISE_X;
    private int Get_Index(int x, int z)
        => z * Table_Size + x;

    public override string ToString()
    {
        string s = "";

        for(int i=0;i<Table_Size;i++)
        {
            for(int j=0;j<Table_Size;j++)
            {
                s+= $"{new Noise_Position(j,i)}:{this[j,i]} ";
            }
            s += "\n";
        }

        return s;
    }
}
