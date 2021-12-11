
using System.Collections.Generic;
using UnityEngine;

public class Entity_Manager : MonoBehaviour
{
    public static Entity Entity_Manager__PLAYER_ENTITY;

    private static readonly Dictionary<GameObject, Entity> 
        Entity_Manager__RUNTIME_LOOKUP = new Dictionary<GameObject, Entity>();

    [SerializeField]
    private Entity playerEntity;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Entity_Enviroment_Scanner scanner;

    private List<Runtime_Entity> Entity_Manager__NPC_ENTITIES { get; }

    public void Start()
    {
        if (Entity_Manager__PLAYER_ENTITY == null)
            Entity_Manager__PLAYER_ENTITY = playerEntity.Clone();

        Entity_Manager__RUNTIME_LOOKUP.Add(player, Entity_Manager__PLAYER_ENTITY);
    }

    public static void Subscribe(GameObject gameObject, Entity associated_entity)
    {
        if (gameObject == null)
            return;
        if (!Is_Not_Entity(gameObject))
            return;

        Entity_Manager__RUNTIME_LOOKUP
            .Add(gameObject, associated_entity);
    }

    public static void Unsubscribe(GameObject gameObject)
    {
        if (Is_Not_Entity(gameObject))
            return;

        Entity_Manager__RUNTIME_LOOKUP.Remove(gameObject);
    }

    public static bool Is_Not_Entity(GameObject gameObject)
        => gameObject == null || !Entity_Manager__RUNTIME_LOOKUP.ContainsKey(gameObject);

    public static void Damage_Entity(GameObject gameObject, uint damage)
    {
         if(Is_Not_Entity(gameObject))
             return;

         Entity_Manager__RUNTIME_LOOKUP[gameObject].Damage(damage);
    }
}
