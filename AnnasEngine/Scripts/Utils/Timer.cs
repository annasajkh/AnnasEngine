namespace AnnasEngine.Scripts.Utils;

public class Timer
{
    public float Time { get; private set; } // in seconds
    public float WaitTime { get; } // in seconds

    private Action timeout;

    private bool isRunning;

    public Timer(float waitTime, Action timeout)
    {
        WaitTime = waitTime;
        this.timeout = timeout;
    }

    public void Start()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Step(float delta)
    {
        if (isRunning)
        {
            Time += delta;

            if (Time >= WaitTime)
            {
                Time = 0;
                timeout();
            }
        }
    }
}
