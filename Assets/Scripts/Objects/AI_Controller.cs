
using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class AI_Controller
{
    public delegate void AI_State_Handler(AI_Controller entity_Controller, Entity_Environment enviroment, NavMeshAgent navMeshAgent);

    public AI_Controller_State AI_State { get; private set; }
    
    private Timer AI_State_Timer { get; }


    [SerializeField]
    private Func<Entity_Environment, AI_Controller_State> next_State_Handler;

    [SerializeField]
    private AI_State_Handler idle_State_Handler;

    [SerializeField] 
    private AI_State_Handler roam_State_Handler;

    [SerializeField]
    private AI_State_Handler attack_State_Handler;

    [SerializeField]
    private AI_State_Handler flee_State_Handler;


    public AI_Controller(){}

    public AI_Controller(AI_Controller clone)
    : 
    this
    (
        clone.next_State_Handler, 
        clone.idle_State_Handler, 
        clone.roam_State_Handler, 
        clone.attack_State_Handler, 
        clone.flee_State_Handler
    )
    {
    }

    public AI_Controller
    (
        Func<Entity_Environment, AI_Controller_State> next_State_Handler,
        AI_State_Handler idle_State_Handler,
        AI_State_Handler roam_State_Handler,
        AI_State_Handler attack_State_Handler,
        AI_State_Handler flee_State_Handler
    )
    {
        this.next_State_Handler = next_State_Handler;
        this.idle_State_Handler = idle_State_Handler;
        this.roam_State_Handler = roam_State_Handler;
        this.attack_State_Handler = attack_State_Handler;
        this.flee_State_Handler = flee_State_Handler;
    }
    
    internal void Process_Controller(Entity_Environment enviroment, NavMeshAgent navMeshAgent, float deltaTime)
    {
        if (AI_State_Timer.Elapse(deltaTime))
            AI_State = next_State_Handler?.Invoke(enviroment) ?? AI_Controller_State.Idle;

        switch(AI_State)
        {
            case AI_Controller_State.Idle:
                idle_State_Handler?.Invoke(this, enviroment, navMeshAgent);
                break;
            case AI_Controller_State.Roam:
                roam_State_Handler?.Invoke(this, enviroment, navMeshAgent);
                break;
            case AI_Controller_State.Attack:
                attack_State_Handler?.Invoke(this, enviroment, navMeshAgent);
                break;
            case AI_Controller_State.Flee:
                flee_State_Handler?.Invoke(this, enviroment, navMeshAgent);
                break;
        }
    }

    public virtual AI_Controller Clone()
    {
        return new AI_Controller(this);
    }
}
