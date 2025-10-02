using System;
using UnityEngine;

public class Miner : WorkActionScr
{
    private Transform _minerTrs;
    private void Awake()
    {
        Name = "Miner";
    }

    public override void DoWork()
    {
        Debug.Log("MinerWork");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 100);
        foreach (var hit in hits)
        {
            _minerTrs = hit.transform;
            break;
        }
        
        base.DoWork();
    }

    public override void ExitWork()
    {
        base.ExitWork();
    }
}
