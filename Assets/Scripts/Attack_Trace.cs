
using UnityEngine;

public class Attack_Trace : MonoBehaviour
{
    [SerializeField]
    private float attack_Distance = 1;

    internal GameObject Trace()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, attack_Distance);

        return hit.collider?.gameObject;
    }
}
