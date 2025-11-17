using System;
using System.Collections;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Member.CHJ._02.Scripts.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MinionPatrol", story: "Patrol [Navmesh] [Self]", category: "Action", id: "974f3a2bbe91bb23804f19b98d33062c")]
    public partial class PatrolAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<NavMeshAgent> Navmesh;
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        private const int MaxAttempt = 10;
        private Vector3 _targetPos;
        private Minion _minion;
        private float _lastT;
        private float _currentT;
        private Vector3 _target;
        private float _waitT = 3;

        protected override Status OnStart()
        {
            Debug.Log("[State] Start Patrol Action!!");
            _minion = Self.Value.GetComponent<Minion>();
            RandomPatrol(Self.Value.transform.position, 3);
            
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

              
            if (Navmesh.Value.remainingDistance <= 0.1f && Time.time - _lastT >= _waitT)
            {
                RandomPatrol(Self.Value.transform.position, 3);
            }
            
            if (_minion.currentState != AiStates.Patrol) return Status.Success;
            return Status.Running;
        }
        
        private void RandomPatrol(Vector2 currentPos, float radius)
        {
            _waitT = Random.Range(1.5f, 4);
            _lastT = Time.time;
            for (int i = 0; i < MaxAttempt; i++)
            {
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
    
  

        protected override void OnEnd()
        {
            base.OnEnd();
        }
    }
}