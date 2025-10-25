using Unity.Behavior;
using UnityEngine;

public class Soldier : WorkActionScr
{
    public override void DoWork()
    {
        base.DoWork();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 6);
    }

    public override void ExitWork()
    {
        base.ExitWork();
    }
}
