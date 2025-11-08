using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MateAction", story: "[NavMesh] [Particle] [minion] [House]", category: "Action", id: "56599743a53276c842f1e4443cec563a")]
public partial class MateAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
    [SerializeReference] public BlackboardVariable<ParticleSystem> Particle;
    [SerializeReference] public BlackboardVariable<Minion> Minion;
    [SerializeReference] public BlackboardVariable<Transform> House;
    protected override Status OnStart()
    {
        NavMesh.Value.SetDestination(House.Value.position);
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
    }

    private void StartMate()
    {
        Minion.Value.isFoundPartner = false;
        
    }

    private bool FindMateMinion()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(Minion.Value.transform.position, 3);
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

