
using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Just like all other objects in LIVID with "Runtime"
/// in it's name - this is where the LIVID business logic
/// meets the UnityEngine logic. In otherwords, this object
/// corresponds (a) GameObject(s) to a LIVID logical entity.
/// </summary>
[Serializable]
public class Runtime_Entity
{
    [SerializeField]
    private GameObject entity_GameObject;
    public Vector3 GameObject_Position => entity_GameObject.transform.position;

    [SerializeField]
    private Entity livid_Entity;
    internal Entity Livid_Entity
        => livid_Entity;

    [SerializeField]
    private AI_Controller livid_Entity_Controller;
    internal void Update_Entity(Entity_Environment enviroment, float deltaTime)
        => livid_Entity_Controller?.Process_Controller(enviroment, navMeshAgent, deltaTime);

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    public Runtime_Entity(GameObject entity_GameObject, Entity livid_Entity)
    {
        this.entity_GameObject = entity_GameObject;
        this.livid_Entity = livid_Entity;
    }

    public Runtime_Entity(Runtime_Entity clone)
    {
        entity_GameObject = GameObject.Instantiate(clone.entity_GameObject);
        livid_Entity = clone.livid_Entity.Clone();
        livid_Entity_Controller = clone.livid_Entity_Controller.Clone();
        navMeshAgent = NavMeshAgent.Instantiate(clone.navMeshAgent);
    }

    internal Runtime_Entity Instanciate()
    {
        return new Runtime_Entity(this);
    }
}
