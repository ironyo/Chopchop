using System;
using System.Collections;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;

namespace Member.CHJ._02.Scripts.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Patrol", story: "Patrol [NavMesh] [self]", category: "Action", id: "974f3a2bbe91bb23804f19b98d33062c")]
    public partial class PatrolAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        private Vector3 _targetPos;
        private MinionMovementManager _movement;
        private Minion _minion;
        private float _lastTime;

        protected override Status OnStart()
        {
            Debug.Log("[State] Start Patrol Action");
            _movement = new MinionMovementManager();
            _minion = Self.Value.GetComponent<Minion>();
            RandomPatrol();
            StopNavmesh();
        
            return Status.Running;
        }

        private void StopNavmesh()
        {
            NavMesh.Value.enabled = false;
            NavMesh.Value.isStopped = true;
        }
        private void ResumeNavmesh()
        {
            NavMesh.Value.enabled = true;
            NavMesh.Value.isStopped = false;
        }
        private bool CheckTime()
        {
            if (_minion.currentState != AiStates.Patrol) return false;
            else return true;
        }
        protected override Status OnUpdate()
        {
            if (!CheckTime()) return Status.Success;
        
            if (TimeManager.Instance.CurrentTime - _lastTime >= 3)
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
            while (!NavMesh.Value.pathPending &&
                   NavMesh.Value.remainingDistance <= NavMesh.Value.stoppingDistance)
            {
                RandomPatrol();
            }
            yield return new WaitForSeconds(2);
            RandomPatrol();
        }
        
        private void RandomPatrol()
        {
            _lastTime = TimeManager.Instance.CurrentTime;
            NavMesh.Value.ResetPath();
            Self.Value.transform.position = Vector3.MoveTowards(Self.Value.transform.position,_movement.RandomPatrol(), 2);
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

        protected override void OnEnd()
        {
            ResumeNavmesh();
            base.OnEnd();
        }
    }
}