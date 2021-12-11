
using UnityEngine;

public class Sword_Swing : MonoBehaviour
{
    [SerializeField]
    private GameObject forward_Reference;

    [SerializeField]
    private float swing_Duration;

    [SerializeField]
    private float reset_Duration;

    [SerializeField]
    private float pitch;

    public bool Is_Swinging { get; private set; }
    public bool Is_Resetting { get; private set; }

    private Timer Swing_Timer { get; }
    private Timer Reset_Timer { get; }

    private float Attack_Duration
        => Swing_Timer.Timer_Limit + Reset_Timer.Timer_Limit;

    private Quaternion Swing_End { get; set; }

    public Sword_Swing()
    {
        Swing_Timer = new Timer(1);
        Reset_Timer = new Timer(1);
    }

    public void Start()
    {
        Swing_Timer.Set(swing_Duration);
        Reset_Timer.Set(reset_Duration);
    }

    public void Update()
    {
        if (!Is_Swinging && !Is_Resetting)
            return;
        Update_Swing_End();

        if (Is_Swinging)
        {
            Is_Swinging = !Swing_Timer.Elapse(Time.deltaTime);
            Is_Resetting = !Is_Swinging;
            if (Is_Swinging)
                Swing();
        }
        
        if (Is_Resetting)
        {
            Is_Resetting = !Reset_Timer.Elapse(Time.deltaTime);
            if (Is_Resetting)
                Reset();
        }
    }

    public bool Attack()
    {
        if (Is_Swinging || Is_Resetting)
            return false;

        Swing_Timer.Set();
        Reset_Timer.Set();

        Is_Swinging = true;

        return true;
    }

    internal virtual void Update_Swing_End()
    {
        Vector3 endRot = 
            forward_Reference.transform.eulerAngles 
            + 
            new Vector3(pitch, 0, 0);

        Swing_End = Quaternion.Euler(endRot);
    }

    internal virtual void Swing()
    {
        Quaternion reference = forward_Reference.transform.rotation;

        Quaternion swing = 
            Quaternion.Lerp(reference, Swing_End, Swing_Timer.Elapsed_Percentage);

        transform.rotation = swing;
    }

    internal virtual void Reset()
    {
        Quaternion reference = forward_Reference.transform.rotation;

        Quaternion swing = 
            Quaternion.Lerp(Swing_End, reference, Reset_Timer.Elapsed_Percentage);

        transform.rotation = swing;
    }
}
