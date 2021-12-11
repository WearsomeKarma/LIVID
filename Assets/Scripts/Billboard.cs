
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private Transform billboard_Target;

    public void Update()
    {
        Vector3 look_At_Location =
            new Vector3(billboard_Target.position.x, transform.position.y, billboard_Target.position.z);

        transform.LookAt(look_At_Location);
    }
}
