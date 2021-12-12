
/// <summary>
/// Represents a 3x3 boolean table
/// that describes various criteria of 
/// Dungeon_Runtime_Cell generation.
///
/// For example, if two hallways are running
/// side by side, we only generate one wall
/// along their shared edges instead of two.
/// This is also true for adjacent hallway
/// corners - we don't want to double up on
/// pillars.
/// </summary>
public sealed class Dungeon_Cell_Adjacency_Table
{
    private bool[,] Adjaceny__TABLE { get; }

    public Dungeon_Cell_Adjacency_Table
    (
        Noise_Position cell_position,
        Dungeon_Hallway_Linker hallway_Linker
    )
    {}
}
