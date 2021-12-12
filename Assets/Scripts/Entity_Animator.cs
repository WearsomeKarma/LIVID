
using UnityEngine;

public class Entity_Animator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public bool Is_Running { get; private set; }

    internal void Toggle_Running()
    {
        Is_Running = !Is_Running;
        animator.SetBool("Running", Is_Running);
    }

    internal void Play_Death()
    {
        animator.SetTrigger("Death");
    }

    internal void Play_Attack()
    {
        animator.SetTrigger("Attacking");
    }

    internal void Play_Hurt()
    {
        animator.SetTrigger("Hurt");
    }
}
