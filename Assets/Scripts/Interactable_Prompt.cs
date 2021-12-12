
using UnityEngine;

public class Interactable_Prompt : MonoBehaviour
{
    internal virtual void Hide()
        => gameObject.SetActive(false);

    internal virtual void Show(string prompt)
        => gameObject.SetActive(true);
}
