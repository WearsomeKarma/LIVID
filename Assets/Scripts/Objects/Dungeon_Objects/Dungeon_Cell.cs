
public struct Dungeon_Cell
{
    public Noise_Position Dungeon_Cell__POSITION { get; }
    public Dungeon_Cell_Type Dungeon_Cell__TYPE { get; }

    public Dungeon_Cell
    (
        Noise_Position cell_Position,
        Dungeon_Cell_Type cell_Type
    )
    {
        Dungeon_Cell__POSITION = cell_Position;
        Dungeon_Cell__TYPE = cell_Type;
    }   
}
