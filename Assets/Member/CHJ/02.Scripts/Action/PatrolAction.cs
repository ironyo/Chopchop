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
    [NodeDescription(name: "Patrol", story: "Patrol [NavMesh] [self]", category: "Action", id: "974f3a2bbe91bb23804f19b98d33062c")]
    public partial class PatrolAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMeshAgent;
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        private Vector3 _targetPos;
        private Minion _minion;
        private float _lastTime;
        private Vector3 _target;

        protected override Status OnStart()
        {
            Debug.Log("[State] Start Patrol Action!!");
            _minion = Self.Value.GetComponent<Minion>();
            RandomPatrol();
            // StopNavmesh();
        
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
        
            RandomPatrol(); // 순찰
            
            if (_minion.currentState != AiStates.Patrol) return Status.Success;
            return Status.Running;
        }
        
        private void RandomPatrol()
        {
            // _lastTime = TimeManager.Instance.CurrentTime;
            // NavMesh.Value.ResetPath();
            // Vector2 patrolPos= PatrolSiteManager.Instance.patrolSite[Random.Range(0, PatrolSiteManager.Instance.patrolSite.Count)].transform.position;
            // if( NavMesh.Value(patrolPos + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)))
            // Self.Value.transform.position = Vector3.MoveTowards(Self.Value.transform.position,_movement.RandomPatrol(), 2);
            do
            {
                Vector3 randomPos = Random.insideUnitCircle * 20;
                randomPos += Self.Value.transform.position;
                _target = randomPos;
                _target.z = 0;

            } 
            while (!NavMesh.SamplePosition(_target, out NavMeshHit hit, 20, NavMesh.AllAreas));
            Debug.Log($"Patrol Target {_target}");
        }
    
  

        protected override void OnEnd()
        {
            // ResumeNavmesh();
            base.OnEnd();
        }
    }
}