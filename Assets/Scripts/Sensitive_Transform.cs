
using UnityEngine;

//This is to work around CharacterController and RigidBody's
//restrictive control over transform.position.
public class Sensitive_Transform : MonoBehaviour
{
    public virtual void Set_Position(Vector3 position)
    {
        transform.position = position;
    }
}
