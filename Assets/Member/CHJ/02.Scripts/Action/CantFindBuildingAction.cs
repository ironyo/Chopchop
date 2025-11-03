using System;
using System.Collections;
using System.Collections.Generic;
using Member.CHJ._02.Scripts.Action;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Cant Find Building Action", story: "[Nevmesh] [Self]", category: "Action", id: "64cf7f92bf09d573139059e233b664ec")]
public partial class CantFindBuildingAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Nevmesh;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private Vector3 _targetPos;
    private MinionMovementManager _movement;
    private Minion _minion;
    private float _lastTime;

    protected override Status OnStart()
    {
        _movement = new MinionMovementManager();
        _minion = Self.Value.GetComponent<Minion>();
        RandomPatrol();
        
        return Status.Running;
    }

    private bool CheckTime()
    {
        if (_minion.currentState == AiStates.Work) return true;
        else
        {
            Debug.Log("Minion does not working");
            return false;
        };
    }
    protected override Status OnUpdate()
    {
        if (!CheckTime()) return Status.Success;
        Debug.Log("[State] Start Patrol Update");
        
        // 목적지 도착 체크
        // if (!NavMesh.Value.pathPending &&
        //     NavMesh.Value.remainingDistance <= NavMesh.Value.stoppingDistance &&
        //     !MateManager.Instance.canMate)
        // {
        //     RandomPatrol();
        // }
        if (TimeManager.Instance.currentTime - _lastTime >= 3)
        {
            RandomPatrol();
            Debug.Log("[Patrol] Can Patrol Time");
        }
        
        
        if (_minion.currentState != AiStates.Patrol) return Status.Success;
        return Status.Running;
    }

    private IEnumerator CheckPatrol()
    {
        RandomPatrol();
        if (!Nevmesh.Value.pathPending &&
            Nevmesh.Value.remainingDistance <= Nevmesh.Value.stoppingDistance &&
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
        _lastTime = TimeManager.Instance.currentTime;
        Nevmesh.Value.ResetPath();
        Nevmesh.Value.SetDestination(_movement.RandomPatrol());
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

