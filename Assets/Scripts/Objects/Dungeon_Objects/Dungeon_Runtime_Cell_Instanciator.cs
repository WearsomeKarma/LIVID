
public abstract class Dungeon_Runtime_Cell_Instanciator
{
    protected Dungeon_Schematic Instanciator__SCHEMATIC { get; }

    public Dungeon_Runtime_Cell_Instanciator
    (
        Dungeon_Schematic schematic
    )
    {
        Instanciator__SCHEMATIC = schematic;
    }

    internal abstract Dungeon_Runtime_Cell Create_Runtime_Cell
    (
        Dungeon_Cell logic_cell
    );
}
