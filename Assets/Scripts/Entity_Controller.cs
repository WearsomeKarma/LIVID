
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Entity_Controller : MonoBehaviour
{
    [SerializeField]
    private Entity livid_entity;

    [SerializeField]
    private string[] enemies;

    [SerializeField]
    private float x_minimum_patrol;

    [SerializeField]
    private float x_maximum_patrol;

    [SerializeField]
    private float z_minimum_patrol;

    [SerializeField]
    private float z_maximum_patrol;

    [SerializeField]
    private float aggroRange;

    [SerializeField]
    private float attackRange;
    

    [SerializeField]
    private Text health_bar;

    private GameObject Target { get; set; }
    private Vector3 Patrol_Destination { get; set; }

    public bool Is_Patrolling { get; private set; }
    
    private Timer Attack_Timer { get; }
    private Timer Patrol_Timer { get; }
    private Timer Idle_Timer { get; }

    private Timer Despawn_Timer { get; }

    private NavMeshAgent NavMeshAgent { get; set; }

    [SerializeField]
    private Entity_Animator entity_Animator;
    [SerializeField]
    private Entity_Enviroment_Scanner environment_Scanner;

    public Entity_Controller()
    {
        Attack_Timer = new Timer(3);
        Patrol_Timer = new Timer(20);
        Idle_Timer = new Timer(1);
        Despawn_Timer = new Timer(3);
    }

    public void Start()
    {
        Patrol_Timer.Set(livid_entity.Patrol_Time);
        Attack_Timer.Set(livid_entity.Attack_Time);
        Despawn_Timer.Set(livid_entity.Despawn_Time);

        environment_Scanner
            .Event__Objects_Detected += Check_Target_Aggression;
        NavMeshAgent = GetComponent<NavMeshAgent>();

        if(Entity_Manager.Is_Not_Entity(gameObject))
            Entity_Manager.Subscribe(gameObject, livid_entity);

        livid_entity.Damaged += (e) => Damaged();
        livid_entity.Death += (e) => Death();

        Update_Health_Bar();
    }

    private void Update_Health_Bar()
    {
        health_bar.text = $"{livid_entity.Health}/{livid_entity.Health_Max}";
    }

    private void Damaged()
    {
        entity_Animator.Play_Hurt();
        Update_Health_Bar();
    }

    private void Death()
    {
        entity_Animator.Play_Death();
    }

    public void Update()
    {
        if (livid_entity.Is_Dead)
        {
            if (Despawn_Timer.Elapse(Time.deltaTime))
                GameObject.Destroy(gameObject);
            return;
        }

        AI();
    }

    private void AI()
    {
        bool should_Patrol =
            Target == null
            ||
            Entity_Manager.Is_Dead(Target);
        if (should_Patrol)
        {
            if (!Idle_Timer.Is_Elapsed)
            {
                Idle();
                return;
            }

            Patrol();
            return;
        }

        float dist = Vector3.Distance(transform.position, Target.transform.position);

        if (dist < aggroRange)
        {
            if (dist <= attackRange)
            {
                Attack();
                return;
            }

            Chase();
            return;
        }

        Target = null;
    }

    private void Idle()
    {
        if (entity_Animator.Is_Running)
            entity_Animator.Toggle_Running();

        Idle_Timer.Elapse(Time.deltaTime);
    }

    private float Get_Patrol_Distance()
    {
        Vector3 xz = new Vector3
            (
                transform.position.x,
                0,
                transform.position.z
            );

        return Vector3.Distance(xz, Patrol_Destination);
    }

    private void Patrol()
    {
        if (!entity_Animator.Is_Running)
            Toggle_Movement();

        if (Is_Patrolling)
        {
            Is_Patrolling = Get_Patrol_Distance() > 0.1f;
            Is_Patrolling = Is_Patrolling && !Patrol_Timer.Elapse(Time.deltaTime);
            
            if (!Is_Patrolling)
            {
                Idle_Timer.Set(Random.Range(livid_entity.Minimum_Idle_Time, livid_entity.Maximum_Idle_Time));
            }

            return;
        }

        Is_Patrolling = true;
        Patrol_Timer.Set();

        float x = Random.Range(x_minimum_patrol, x_maximum_patrol);
        float z = Random.Range(z_minimum_patrol, z_maximum_patrol);

        Patrol_Destination = new Vector3(x, 0, z);
        NavMeshAgent.SetDestination(Patrol_Destination);
    }

    private void Attack()
    {
        if (entity_Animator.Is_Running)
            Toggle_Movement();

        NavMeshAgent.destination = transform.position;

        if (!Attack_Timer.Elapse(Time.deltaTime))
            return;
        Attack_Timer.Set();
        Entity_Manager.Damage_Player(livid_entity.Damage_Output);

        entity_Animator.Play_Attack();
    }

    private void Chase()
    {
        if (!entity_Animator.Is_Running)
            Toggle_Movement();
        
        NavMeshAgent.SetDestination(Target.transform.position);
    }

    private void Toggle_Movement()
    {
        entity_Animator.Toggle_Running();
    }

    private void Check_Target_Aggression(Entity_Enviroment_Scanner scanner)
    {
        if (Target != null)
            return;

        foreach(GameObject scanned in scanner.Discovered_Objects)
        {
            foreach(string enemy in enemies)
            {
                if (enemy == scanned.tag)
                {
                    Target = scanned;
                    return;
                }
            }
        }
    }
}
