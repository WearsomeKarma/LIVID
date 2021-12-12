
using UnityEngine;
using UnityEngine.Events;

public class HUD_Blackout : MonoBehaviour
{
    [SerializeField]
    private float blackout_Duration;
    [SerializeField]
    private float blackout_Pause;
    
    [SerializeField]
    private UnityEvent post_Blackout_Handler;

    private CanvasGroup CanvasGroup { get; set; }

    private Timer Blackout_Timer { get; }
    private Timer Blackout_Pause_Timer { get; }

    public bool Is_Blacking_Out { get; private set; }

    public HUD_Blackout()
    {
        Blackout_Timer = new Timer(1);
        Blackout_Pause_Timer = new Timer(1);
    }

    public void Start()
    {
        CanvasGroup = GetComponent<CanvasGroup>();

        Blackout_Timer.Set(blackout_Duration);
        Blackout_Pause_Timer.Set(blackout_Pause);
    }

    public void Blackout()
    {
        Is_Blacking_Out = true;
        Blackout_Timer.Set();
        Blackout_Pause_Timer.Set();
    }

    public void Update()
    {
        if (!Is_Blacking_Out)
            return;

        if (Blackout_Timer.Is_Elapsed)
        {
            Blackout_Pause_Timer.Elapse(Time.deltaTime);
            if (!Blackout_Pause_Timer.Is_Elapsed)
                return;
            
            post_Blackout_Handler?.Invoke();

            return;
        }

        Blackout_Timer.Elapse(Time.deltaTime);

        CanvasGroup.alpha = Blackout_Timer.Elapsed_Percentage;
    }
}
