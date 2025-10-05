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
    protected override Status OnStart()
    {
        while (Target == null)
        {
            Work.Value.DoWork();
            //순찰 상태 지속
        }
        Navmesh.Value.SetDestination(Target.Value.position);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Debug.Log("OnUpdate");
        if (!Work.Value.isWorking)
        {
            return Status.Success;
        }
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Work.Value.ExitWork();
    }
}

