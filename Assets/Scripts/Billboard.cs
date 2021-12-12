
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform Billboard_Target { get; set; }

    public void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Billboard_Target = player.transform;
    }

    public void Update()
    {
        Vector3 look_At_Location =
            new Vector3(Billboard_Target.position.x, transform.position.y, Billboard_Target.position.z);

        transform.LookAt(look_At_Location);
    }
}
