using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "Patrol [NavMesh] [self] at [WorkPoints]", category: "Action", id: "974f3a2bbe91bb23804f19b98d33062c")]
public partial class PatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> WorkPoints;
    private Vector3 _targetPos;
    private Minion _minion;

    protected override Status OnStart()
    {
        RandomPatrol();
        _minion = Self.Value.GetComponent<Minion>();

        return Status.Running;
    }

    private bool CheckTime()
    {
        if (_minion.currentState != AiStates.Patrol) return false;
        else return true;
    }
    protected override Status OnUpdate()
    {
        if (!CheckTime()) return Status.Success;
        // 목적지 도착 체크
        if (!NavMesh.Value.pathPending &&
            NavMesh.Value.remainingDistance <= NavMesh.Value.stoppingDistance)
        {
            RandomPatrol();
        }

        if (_minion.currentState != AiStates.Patrol) return Status.Success;
        return Status.Running;
    }

    private void RandomPatrol()
    {
        Vector2 pos= WorkPoints.Value[Random.Range(0, WorkPoints.Value.Count)].transform.position;
        Vector2 target = pos + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        
        NavMesh.Value.ResetPath();
        
        NavMesh.Value.SetDestination(target);
    }
}