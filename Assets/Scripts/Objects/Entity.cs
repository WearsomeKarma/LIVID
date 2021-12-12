
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Entity 
{
    /// <summary>
    /// The constant value which represents the
    /// death of an entity.
    /// </summary>
    public const int HEALTH_DEATH = 0;

    [SerializeField]
    private int health_max;
    public int Health_Max
        => health_max;
    [SerializeField]
    private int current_health;
    public int Health
    {
        get => current_health;
        private set
        {
            if (current_health == value)
                return;

            int old_health = current_health;
            current_health = value;

            if (value < old_health)
            {
                Damaged?.Invoke(this);
            }
            if (value > old_health)
                Healed?.Invoke(this);

            if (current_health <= HEALTH_DEATH)
                Death?.Invoke(this);
        }
    }

    public bool Is_Dead
        => Health <= HEALTH_DEATH;
    public bool Is_A_Copy { get; private set; }

    [SerializeField]
    private float movement_Speed;
    public float Movement_Speed 
        => movement_Speed;

    [SerializeField]
    private float jump_Speed;
    public float Jump_Speed
        => jump_Speed;

    [SerializeField]
    private float attack_Time = 3;
    public float Attack_Time 
        => attack_Time;
    [SerializeField]
    private float minimum_Idle_Time = 3;
    public float Minimum_Idle_Time 
        => minimum_Idle_Time;
    [SerializeField]
    private float maximum_Idle_Time = 10;
    public float Maximum_Idle_Time 
        => maximum_Idle_Time;
    [SerializeField]
    private float patrol_Time = 20;
    public float Patrol_Time 
        => patrol_Time;
    [SerializeField]
    private float despawn_Time= 1;
    public float Despawn_Time 
        => patrol_Time;

    [SerializeField]
    private uint damage_Output;
    public uint Damage_Output
        => damage_Output;

    [SerializeField]
    private float field_Of_Aggression;
    public float Field_Of_Aggression
        => field_Of_Aggression;

    internal event Action<Entity> Damaged;
    internal event Action<Entity> Healed;
    internal event Action<Entity> Death;

    [SerializeField]
    private UnityEvent death_Hook;

    public Entity()
    {
        Death += (e) => death_Hook?.Invoke();
    }

    public Entity(Entity clone)
    : 
    this
    (
        clone.health_max, 
        clone.current_health,
        clone.movement_Speed, 
        clone.jump_Speed, 
        clone.attack_Time, 
        clone.damage_Output,
        clone.field_Of_Aggression
    )
    {}

    public Entity
    (
        int health_max,
        int current_health,
        float movement_Speed,
        float jump_Speed,
        float attack_Speed,
        uint damage_Output,
        float field_Of_Aggression
    )
    {
        this.health_max = health_max;
        this.current_health = current_health;
        this.movement_Speed = movement_Speed;
        this.jump_Speed = jump_Speed;
        this.attack_Time = attack_Speed;
        this.damage_Output = damage_Output;
        this.field_Of_Aggression = field_Of_Aggression;

        Death += (e) => death_Hook?.Invoke();
    }

    internal void Copy(Entity entity)
    {
        Is_A_Copy = true;
        Set_Max_Health((uint)entity.health_max);
        Set_Health((uint)entity.current_health);
        movement_Speed = entity.movement_Speed;
        jump_Speed = entity.jump_Speed;
        attack_Time = entity.attack_Time;
        damage_Output = entity.damage_Output;
        field_Of_Aggression = entity.field_Of_Aggression;
    }

    /// <summary>
    /// Subtracts from the health a value equivalent to
    /// damage. Health is clamped between 0 and it's maximum.
    /// </summary>
    internal void Damage(uint damage)
    {
        int udamage = (int)damage;
        Health = Mathf.Clamp(current_health - udamage, 0, health_max);
    }

    /// <summary>
    /// Adds to the health a value equivalent to
    /// heal. Health is clamped between 0 and it's maximum.
    /// </summary>
    internal void Heal(uint heal)
    {
        Health = Mathf.Clamp(current_health + (int)heal, 0, health_max);
    }

    /// <summary>
    /// Sets the health to a value equivalent to
    /// the new health. 
    /// Health is clamped between 0 and it's maximum.
    /// This does not trigger damage/heal events.
    /// </summary>
    internal void Set_Health(uint health)
    {
        int uhealth = (int)health;
        Health = Mathf.Clamp(uhealth, 0, health_max);
    }

    /// <summary>
    /// Sets the health_max to a value equivalent to
    /// the new health. 
    /// This does not trigger damage/heal events.
    /// </summary>
    internal void Set_Max_Health(uint health)
    {
        int uhealth = (int)health;
        health_max = uhealth;
        Set_Health(health);
    }

    public virtual Entity Clone()
    {
        return new Entity(this);
    }
}
