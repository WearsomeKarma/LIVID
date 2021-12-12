
using UnityEngine;

public class Player_Interaction_Ray : MonoBehaviour
{
    [SerializeField]
    private float range;

    [SerializeField]
    private Interactable_Prompt prompt;

    private Interactable Current_Interaction { get; set; }

    /// <summary>
    /// Returns true if currently looking at an interactable.
    /// </summary>
    public bool Is_Engaged 
        => Current_Interaction != null;

    public void Update()
    {
        RaycastHit hit;
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        Physics.Raycast(transform.position, direction, out hit, range);

        Current_Interaction = hit.collider?.gameObject?.GetComponent<Interactable>();

        if (!Is_Engaged)
            prompt.Hide();
        else
            prompt.Show(Current_Interaction.Prompt);
    }

    internal void Interact()
        => Current_Interaction?.Interact();
}
