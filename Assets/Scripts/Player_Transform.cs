
using UnityEngine;

public class Player_Transform : Sensitive_Transform
{
    public override void Set_Position(Vector3 position)
    {
        CharacterController cc = GetComponent<CharacterController>();

        cc.enabled = false;
        cc.transform.position = position;
        cc.enabled = true;
    }
}
