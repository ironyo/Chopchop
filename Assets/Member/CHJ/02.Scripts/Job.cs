using UnityEngine;

public abstract class Job
{
    public bool IsUnlocked;

    public WorkActionScr WorkScr;

    public Job(WorkActionScr scr)
    {
        WorkScr = scr;
    }
}
