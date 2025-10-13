using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "Patrol [NavMesh] [self]", category: "Action",
    id: "974f3a2bbe91bb23804f19b98d33062c")]
public partial class PatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private float _currentTime;

    protected override Status OnStart()
    {
        return Status.Running;
        Vector2 randomPos = SetRandomPos();
    }

    protected override Status OnUpdate()
    {
        _currentTime += Time.deltaTime;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }

    private Vector2 SetRandomPos()
    {
        Vector2 randomDir = Random.insideUnitCircle * 10;
        return randomDir;
    }
}        