using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private Sword_Swing sword_Swing;
    [SerializeField]
    private Attack_Trace attack_Trace;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private bool Is_Able_To_Attack { get; set; }

    private Entity Player { get; set; }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Player = Entity_Manager.Entity_Manager__PLAYER_ENTITY;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float x = Input.GetAxis("Horizontal") * playerSpeed;
        float z = Input.GetAxis("Vertical") * playerSpeed;

        Vector3 move =
            (transform.right * x)
            +
            (transform.forward * z);
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        Is_Able_To_Attack = !sword_Swing.Is_Swinging && !sword_Swing.Is_Resetting;
        if (Input.GetButtonDown("Fire1"))
        {
            sword_Swing.Attack();
            GameObject hitEntity = attack_Trace.Trace();

            Entity_Manager.Damage_Entity(hitEntity, Player?.Damage_Output ?? 0);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
