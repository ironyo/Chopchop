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
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }
    public void Mate()
    {
        Particle.Value.Play();
        Collider2D[] hits = Physics2D.OverlapCircleAll(Minion.Value.transform.position, 6);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("House"))
                NavMesh.Value.SetDestination(hit.transform.position);
        }
        if (NavMesh.Value.remainingDistance <= 0.01f)
        {
            Minion.Value.visualObj.SetActive(false);
        }
    }
    
    private void EndMate()
    {
        Minion.Value.visualObj.SetActive(true);
        Particle.Value.Pause();
    }
    protected override void OnEnd()
    {
    }
}

