
public class Timer
{
    public float Elapsed_Time { get; private set; }
    public float Timer_Limit { get; private set; }

    public bool Is_Elapsed
        => Elapsed_Time >= Timer_Limit;

    public float Elapsed_Percentage 
        => (Timer_Limit == 0) ? 0 : (Elapsed_Time / Timer_Limit);

    public Timer(float timer_Limit = 1)
    {
        Elapsed_Time = 0;
        Timer_Limit = timer_Limit;
    }

    public void Set(float? newLimit = null)
    {
        Elapsed_Time = 0;
        if (newLimit != null)
            Timer_Limit = (float)newLimit;
    }

    public bool Elapse(float deltaTime)
    {
        Elapsed_Time += deltaTime;
        return Is_Elapsed;
    }
}
