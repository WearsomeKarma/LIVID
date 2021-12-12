
using UnityEngine;
using UnityEngine.UI;

public class Interactable_UI_Prompt : Interactable_Prompt
{
    [SerializeField]
    Text text;

    internal override void Show(string prompt)
    {
        text.text = prompt;
        base.Show(prompt);
    }
}
