using UnityEngine;

public abstract class WorkActionScr : MonoBehaviour
{
    public string Name { get; protected set; }

    public bool isWorking;

    public virtual void DoWork()
    {
        isWorking = true;
    }

    public virtual void ExitWork()
    {
        isWorking = false;
    }
}
