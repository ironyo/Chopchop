using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MateAction", story: "[NavMesh] [Particle] [minion]", category: "Action", id: "56599743a53276c842f1e4443cec563a")]
public partial class MateAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
    [SerializeReference] public BlackboardVariable<ParticleSystem> Particle;
    [SerializeReference] public BlackboardVariable<Minion> Minion;

    protected override Status OnStart()
    {
        NavMesh.Value.ResetPath();
        TimeManager.Instance.OnOneSecond += MateCheck;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (NavMesh.Value.remainingDistance <= 0.01f)
        {
            Minion.Value.visualObj.SetActive(false);
            return Status.Success;
        }

        return Status.Running;
    }
    public void MateCheck(int t)
    {
        if (FindMateMinion() && FindHouse() && Minion.Value.isFoundPartner)
            StartMate();
    }

    private void StartMate()
    {
        Minion.Value.isFoundPartner = false;
        
    }

    private bool FindMateMinion()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(Minion.Value.transform.position, 1);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Minion>(out var minion))
            {
                if (!minion.isFoundPartner)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool FindHouse()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(Minion.Value.transform.position, 6);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("House"))
                return true;
        }

        return false;
    }
    private void EndMate()
    {
        Minion.Value.visualObj.SetActive(true);
        Particle.Value.Pause();
        //호이쨔
    }
    protected override void OnEnd()
    {
        EndMate();
    }
}

