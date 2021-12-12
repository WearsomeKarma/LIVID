
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_Enviroment_Scanner : MonoBehaviour
{
    private HashSet<GameObject> Entity_Enviroment_Scanner__DISCOVERED_OBJECTS { get; }
    internal IEnumerable<GameObject> Discovered_Objects
        => Entity_Enviroment_Scanner__DISCOVERED_OBJECTS.ToArray();

    [SerializeField]
    private GameObject[] blacklist;

    [SerializeField]
    private float initial_Radius;

    internal event Action<Entity_Enviroment_Scanner> Event__Objects_Detected;

    public Entity_Enviroment_Scanner()
    {
        Entity_Enviroment_Scanner__DISCOVERED_OBJECTS =
            new HashSet<GameObject>();
    }

    public void Start()
    {
        Set_Radius(initial_Radius);        
        Event__Objects_Detected += (a => Debug.Log("Detecting an entity."));
    }

    public void Assess(Vector3 position, float radius)
    {
        Set_Radius(radius);
        Assess(position);
    }

    public void Assess(Vector3 position)
    {
        Collider scan_collider = GetComponent<Collider>();
        scan_collider.transform.position = position;
    }

    private void Set_Radius(float radius)
    {
        Collider scan_collider = GetComponent<Collider>();
        if (scan_collider is SphereCollider)
            ((SphereCollider)scan_collider).radius = radius;
    }

    public void OnTriggerEnter(Collider collider)
    {
        foreach(GameObject blacklisted_object in blacklist)
            if (blacklisted_object == collider.gameObject)
                return;
        if (!Entity_Enviroment_Scanner__DISCOVERED_OBJECTS.Contains(collider.gameObject))
        {
            Entity_Enviroment_Scanner__DISCOVERED_OBJECTS.Add(collider.gameObject);        
        }
    }

    public void LateUpdate()
    {
        if (Entity_Enviroment_Scanner__DISCOVERED_OBJECTS.Count > 0)
        {
            Event__Objects_Detected?.Invoke(this);
            Entity_Enviroment_Scanner__DISCOVERED_OBJECTS.Clear();
        }
    }
}
