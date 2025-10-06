using System;
using Unity.Behavior;
using UnityEngine;

public class Baby : WorkActionScr
{
    private void Awake()
    {
        Type = JobType.Baby;
    }

    public override void DoWork()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 6);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("School"))
            {
                GetComponent<BehaviorGraphAgent>().SetVariableValue("Target", hit.transform);
                break;
            }
        }
        base.DoWork();
    }

    public override void ExitWork()
    {
        base.ExitWork();
    }
}
