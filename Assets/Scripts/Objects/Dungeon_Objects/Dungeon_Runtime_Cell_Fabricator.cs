
using System;
using UnityEngine;

[Serializable]
public class Dungeon_Runtime_Cell_Fabricator
{
    [SerializeField]
    private float cell_width;
    
    [SerializeField]
    private float cell_length;
    
    [SerializeField]
    private float cell_height;

    
    [SerializeField]
    private GameObject[] wall_Prefabs;
    
    public int Wall_Prefab_Count
        => wall_Prefabs.Length;
    
    public GameObject Instantiate_Wall(int wall_ID, Noise_Position cell_position, Vector3 offset, Quaternion rotation)
        => Instanciate_Prefab(wall_Clutter_Prefabs[wall_ID], cell_position, offset, rotation);


    [SerializeField]
    private GameObject[] wall_Clutter_Prefabs;
    
    public int Wall_Clutter_Count
        => wall_Clutter_Prefabs.Length;
    
    public GameObject Instantiate_Wall_Clutter(int wall_Clutter_ID, Noise_Position cell_position, Vector3 offset, Quaternion rotation)
        => Instanciate_Prefab(wall_Clutter_Prefabs[wall_Clutter_ID], cell_position, offset, rotation);

    
    [SerializeField]
    private GameObject[] floor_Clutter_Prefabs;
    
    public int Floor_Clutter_Count
        => floor_Clutter_Prefabs.Length;
    
    public GameObject Instantiate_Floor_Clutter(int floor_Clutter_ID, Noise_Position cell_position, Vector3 offset, Quaternion rotation)
        => Instanciate_Prefab(floor_Clutter_Prefabs[floor_Clutter_ID], cell_position, offset, rotation);
    
    
    [SerializeField]
    private GameObject[] pillar_Prefabs;
    
    public int Pillar_Prefab_Count
        => pillar_Prefabs.Length;
    
    public GameObject Instantiate_Pillar(int pillar_ID, Noise_Position cell_position, Vector3 offset, Quaternion rotation)
        => Instanciate_Prefab(pillar_Prefabs[pillar_ID], cell_position, offset, rotation);
    
    
    [SerializeField]
    private GameObject floor_Ceiling_Prefab;
    
    public GameObject Instanciate_Floor_Ceiling(Noise_Position cell_position, bool isCeilingOrFloor = true)
    {
        Vector3 offset = (isCeilingOrFloor) ? Vector3.zero : new Vector3(0,cell_height,0);

        return Instanciate_Prefab(floor_Ceiling_Prefab, cell_position, offset, new Quaternion());
    }

    private GameObject Instanciate_Prefab
    (
        GameObject prefab, 
        Noise_Position cell_position, 
        Vector3 offset,
        Quaternion rotation
    )
    {
        GameObject instance = GameObject.Instantiate(prefab);
        float x = cell_position.NOISE_X * cell_width;
        float z = cell_position.NOISE_Z * cell_length;
        instance.transform.position =
            new Vector3(x,0,z)
            +
            offset;

        instance.transform.rotation =
            rotation;

        return instance;
    }
}
