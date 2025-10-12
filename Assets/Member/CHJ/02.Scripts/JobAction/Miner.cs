using System;
using Unity.Behavior;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Miner : WorkActionScr
{
    private void Awake()
    {
        Type = JobType.Miner;
    }

    public override void DoWork()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 6);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Mine"))
            {
                GetComponent<BehaviorGraphAgent>().SetVariableValue("Target", hit.transform);
                Debug.Log("Find");
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
