using System;
using System.Collections;
using System.Collections.Generic;
using Member.CHJ._02.Scripts.Action;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "Patrol [NavMesh] [self]", category: "Action", id: "974f3a2bbe91bb23804f19b98d33062c")]
public partial class PatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    private Vector3 _targetPos;
    private MinionMovementManager _movement;
    private Minion _minion;

    protected override Status OnStart()
    {
        _movement = new MinionMovementManager();
        _minion = Self.Value.GetComponent<Minion>();
        RandomPatrol();

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
        
        if (MateManager.Instance.canMate) FindMatePartner();
        
        
        // 목적지 도착 체크
        if (!NavMesh.Value.pathPending &&
            NavMesh.Value.remainingDistance <= NavMesh.Value.stoppingDistance &&
            !MateManager.Instance.canMate)
        {
            RandomPatrol();
        }
        
        if (_minion.currentState != AiStates.Patrol) return Status.Success;
        return Status.Running;
    }

    private IEnumerator CheckPatrol()
    {
        RandomPatrol();
        if (!NavMesh.Value.pathPending &&
            NavMesh.Value.remainingDistance <= NavMesh.Value.stoppingDistance &&
            !MateManager.Instance.canMate)
        {
            RandomPatrol();
        }
        yield return new WaitForSeconds(2);
        RandomPatrol();
    }
    protected override void OnEnd() => _minion.ResumeState();

    private void RandomPatrol()
    {
        NavMesh.Value.ResetPath();
        Debug.Log(_movement);
        NavMesh.Value.SetDestination(_movement.RandomPatrol());
    }
    
    public void FindMatePartner()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(Self.Value.transform.position, 1);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Minion>(out var minion))
            {
                if (!minion.isFoundPartner)
                {
                    _minion.StartCoroutine(_minion.Mate(minion));
                }
            }
        }
    }

}