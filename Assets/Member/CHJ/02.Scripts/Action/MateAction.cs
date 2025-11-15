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
    [SerializeReference] public BlackboardVariable<Building> House;
    protected override Status OnStart()
    {
        Debug.Log("REAL MATE START");
        NavMesh.Value.SetDestination(House.Value.transform.position);
        Particle.Value.Play();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (NavMesh.Value.remainingDistance <= 0.01f)
        {
            Minion.Value.GetVisualObject().SetActive(false);
            return Status.Success;
        }
        return Status.Running;
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
        Minion.Value.GetVisualObject().SetActive(true);
        Particle.Value.Pause();
        //호이쨔
    }
    protected override void OnEnd()
    {
        if (House?.Value != null)
            House.Value.TryReserve();

        EndMate();
    }

}