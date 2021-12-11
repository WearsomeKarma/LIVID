
using System;
using UnityEngine;

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
    [SerializeField]
    private int current_health;
    public int Health
    {
        get => current_health;
        private set
        {
            if (value < current_health)
                Damaged?.Invoke(this);
            if (value > current_health)
                Healed?.Invoke(this);

            current_health = value;

            if (current_health <= HEALTH_DEATH)
                Death?.Invoke(this);
        }
    }

    public bool Is_Dead
        => Health <= HEALTH_DEATH;

    [SerializeField]
    private float movement_Speed;
    public float Movement_Speed 
        => movement_Speed;

    [SerializeField]
    private float jump_Speed;
    public float Jump_Speed
        => jump_Speed;

    [SerializeField]
    private float attack_Speed;
    public float Attack_Speed 
        => attack_Speed;

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

    public Entity()
    {
    }

    public Entity(Entity clone)
    : 
    this
    (
        clone.health_max, 
        clone.current_health,
        clone.movement_Speed, 
        clone.jump_Speed, 
        clone.attack_Speed, 
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
        this.current_health = current_health;
        this.movement_Speed = movement_Speed;
        this.jump_Speed = jump_Speed;
        this.attack_Speed = attack_Speed;
        this.damage_Output = damage_Output;
        this.field_Of_Aggression = field_Of_Aggression;
    }

    /// <summary>
    /// Subtracts from the health a value equivalent to
    /// damage. Health is clamped between 0 and it's maximum.
    /// </summary>
    internal void Damage(uint damage)
    {
        int udamage = (int)damage;
        Health = Mathf.Clamp(Health - udamage, 0, health_max);
    }

    /// <summary>
    /// Adds to the health a value equivalent to
    /// heal. Health is clamped between 0 and it's maximum.
    /// </summary>
    internal void Heal(uint heal)
    {
        Health = Mathf.Clamp(Health + (int)heal, 0, health_max);
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
