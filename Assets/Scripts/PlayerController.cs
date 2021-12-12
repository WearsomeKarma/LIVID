
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private uint regeneration_amount = 1;
    [SerializeField]
    private float regeneration_tick = 3.0f;

    [SerializeField]
    private Sword_Swing sword_Swing;
    [SerializeField]
    private Attack_Trace attack_Trace;
    [SerializeField]
    private Player_Interaction_Ray interaction_Ray;
    [SerializeField]
    private float death_animation_time;
    [SerializeField]
    private Vector3 death_resting_orientation;

    [SerializeField]
    private UnityEvent post_Death_Hook;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private bool Is_Able_To_Attack { get; set; }

    private Entity Player { get; set; }

    private Timer Regeneration_Timer { get; }
    
    private Timer Death_Timer { get; }
    private Quaternion Death_Orientation { get; set; }
    private Quaternion Death_Resting_Orientation { get; set; }
    private bool Death_Observed { get; set; }

    public PlayerController()
    {
        Death_Timer = new Timer(1);
        Regeneration_Timer = new Timer(1);
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Player = Entity_Manager.Entity_Manager__PLAYER_ENTITY;

        Death_Timer.Set(death_animation_time);
        Regeneration_Timer.Set(regeneration_tick);

        Player.Death += (e) => post_Death_Hook?.Invoke();
    }

    void Update()
    {
        if (Player.Is_Dead)
        {
            if (!Death_Observed)
            {
                Death_Orientation = transform.rotation;
                Death_Resting_Orientation =
                    Death_Orientation
                    *
                    Quaternion.Euler(death_resting_orientation);
            }
            Death_Observed = true;

            controller.enabled = false;
            Die();
            return;
        }

        if (Regeneration_Timer.Elapse(Time.deltaTime))
        {
            Regeneration_Timer.Set();
            Player.Heal(regeneration_amount);
        }

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
        if (Input.GetButtonDown("Fire1") && sword_Swing.Is_Not_In_Motion)
        {
            sword_Swing.Attack();
            GameObject hitEntity = attack_Trace.Trace();

            Entity_Manager.Damage_Entity(hitEntity, Player?.Damage_Output ?? 0);
        }

        if (Input.GetButtonDown("Interact"))
        {
            interaction_Ray.Interact();
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Die()
    {
        if (Death_Timer.Is_Elapsed)
            return;
        
        Death_Timer.Elapse(Time.deltaTime);

        transform.rotation = Quaternion.Lerp
        (
            Death_Orientation,
            Death_Resting_Orientation,
            Death_Timer.Elapsed_Percentage
        );

        if (Death_Timer.Is_Elapsed)
            post_Death_Hook?.Invoke();
    }

    internal void Set_Y(float y)
    {
        controller.enabled = false;
        Vector3 pos = controller.transform.position;
        pos.y = y;
        controller.transform.position = pos;
        controller.enabled = true;
    }
}
