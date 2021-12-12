
using UnityEngine;

public class Main_Menu_Camera_Pan : MonoBehaviour
{
    public void Update()
    {
        transform.Rotate(new Vector3(0,0.5f,0) * Time.deltaTime);
    }
}
