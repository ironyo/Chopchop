using Unity.Behavior;
using UnityEngine;

public class Soldier : WorkActionScr
{
    private void Awake()
    {
        Type = JobType.Soldier;
    }

    public override void DoWork()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 6);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Army"))
            {
                GetComponent<BehaviorGraphAgent>().SetVariableValue("Target", hit.transform);
                break;
            }
        }
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
