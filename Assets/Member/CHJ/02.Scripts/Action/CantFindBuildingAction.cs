using System;
using System.Collections;
using System.Collections.Generic;
using Member.CHJ._02.Scripts.Action;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Cant Find Building Action", story: "[Navmesh] [Self] After [State] [WaitT]", category: "Action", id: "64cf7f92bf09d573139059e233b664ec")]
public partial class CantFindBuildingAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Navmesh;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<AiStates> State;
    [SerializeReference] public BlackboardVariable<float> WaitT;
    private Vector3 _targetPos;
    private Vector3 _target;
    private Minion _minion;
    private float _lastTime;

    private const int MaxAttempt = 10;
    protected override Status OnStart()
    {
        _minion = Self.Value.GetComponent<Minion>();
        RandomPatrol(Self.Value.transform.position, 4);
        
        return Status.Running;
    }

    private bool CheckTime()
    {
        if (_minion.currentState == State.Value) return true;
        else
        {
            return false;
        };
    }
    protected override Status OnUpdate()
    {
        if (!CheckTime())
        {
            return Status.Success;
        }
        
        if (Navmesh.Value.remainingDistance <= 0.1f)
        {
            RandomPatrol(Self.Value.transform.position, 4);
        }

        if (_minion.currentState != AiStates.Patrol) return Status.Success;
        return Status.Running;
    }


    private void RandomPatrol(Vector3 currentPos, float radius)
    {
        for (int i = 0; i < MaxAttempt; i++)
        {
            WaitT.Value = Random.Range(1.5f, 4);
            Vector3 randomPos = Random.insideUnitCircle * radius;
            randomPos += (Vector3)currentPos;
            _target = randomPos;
            _target.z = 0;
            if (NavMesh.SamplePosition(_target, out NavMeshHit hit, 10, NavMesh.AllAreas))
            {
                Navmesh.Value.SetDestination(hit.position);
                return;
            }
        }
        Navmesh.Value.SetDestination(currentPos);
    }

}

