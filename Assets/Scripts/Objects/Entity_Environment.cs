
using System.Collections.Generic;

public class Entity_Environment
{
    public Entity Percieving_Entity { get; }

    public IEnumerable<Entity> Visible_Entities { get; }

    public Entity_Environment
    (
        Entity percieving_Entity, 
        IEnumerable<Entity> visible_Entities
    )
    {
        Percieving_Entity = percieving_Entity;
        Visible_Entities = visible_Entities;
    }
}
