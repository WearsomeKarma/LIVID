
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_Manager : MonoBehaviour
{
    public static readonly Entity Entity_Manager__PLAYER_ENTITY =
        new Entity();

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
        if (!Entity_Manager__PLAYER_ENTITY.Is_A_Copy)
        {
            Entity_Manager__PLAYER_ENTITY.Copy(playerEntity);
        }

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

    public static bool Is_Dead(GameObject gameObject)
        => gameObject == null || Entity_Manager__RUNTIME_LOOKUP[gameObject].Is_Dead;

    public static void Damage_Entity(GameObject gameObject, uint damage)
    {
         if(Is_Not_Entity(gameObject))
             return;

         Entity_Manager__RUNTIME_LOOKUP[gameObject].Damage(damage);
    }

    public static void Damage_Player(uint damage)
    {
        Entity_Manager__PLAYER_ENTITY.Damage(damage);
    }

    public void Destroy_All()
        => Destroy_All_Static();

    public static void Destroy_All_Static()
    {
        foreach(GameObject gobj in Entity_Manager__RUNTIME_LOOKUP.Keys.ToArray())
        {
            if (Entity_Manager__RUNTIME_LOOKUP[gobj] == Entity_Manager__PLAYER_ENTITY)
                continue;
            Entity_Manager__RUNTIME_LOOKUP.Remove(gobj);
            GameObject.Destroy(gobj);
        }
    }
}
