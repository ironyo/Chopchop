using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Work", story: "[self] Do [work] at [Target] [Navmesh]", category: "Action", id: "485f71707e8a79af83a64c0970755cfd")]
public partial class WorkAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<WorkActionScr> Work;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Navmesh;
    private Minion _minion;
    protected override Status OnStart()
    {
        Navmesh.Value.ResetPath();
        _minion = Self.Value.GetComponent<Minion>();
        Work.Value.DoWork();
        if (Target.Value != null)
        {
            Vector3 targetPos = Target.Value.position;
            targetPos.z = 0;
            Navmesh.Value.SetDestination(targetPos);
        }
        return Status.Running;
    }
    private bool CheckTime()
    {
        if (_minion.currentState != AiStates.Work) return false;
        else return true;
    }
    protected override Status OnUpdate()
    {
        if (!CheckTime() || !Work.Value.isWorking || _minion.currentState != AiStates.Work)
            return Status.Success;
        if (Work.Value.IsCollisionWithWorkBuilding() && _minion.visualObj.activeSelf)
        {
            _minion.visualObj.SetActive(false);
        }
        
        Debug.Log($"ONHIT{Work.Value.IsCollisionWithWorkBuilding()} / {Target}");
        
        if (Target.Value != null)
        {
            // 목적지와 NavMeshAgent의 현재 목적지 비교
            if (Vector3.Distance(Navmesh.Value.destination, Target.Value.position) > 0.1f)
            {
                // 목적지가 바뀌었으면 갱신
                Vector3 targetPos = Target.Value.position;
                targetPos.z = 0;
                Navmesh.Value.SetDestination(targetPos);
            }
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        _minion.visualObj.SetActive(true);
        Work.Value.ExitWork();
    }
}

