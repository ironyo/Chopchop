using System;
using Member.CHJ._02.Scripts;
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
    [SerializeReference] public BlackboardVariable<Building> Target;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Navmesh;
    private Minion _minion;
    protected override Status OnStart()
    {
        _minion = Self.Value.GetComponent<Minion>();
        Navmesh.Value.ResetPath();
        if (Target.Value == null)
        {
            Work.Value.CantWork();
        }
        if (Target.Value != null)
        {
            Debug.Log(Target.Value.enter);
            Navmesh.Value.SetDestination(Target.Value.EnterObj.transform.position);


            Work.Value.DoWork(Target);
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
        if (!Work.Value.isWorking ||
            Target.Value == null ||
            _minion.currentState != AiStates.Work)
        {
            return Status.Success;
        }

        if ((Navmesh.Value.destination - Target.Value.transform.position).sqrMagnitude >= 0.25f)
        {
            Vector2 targetPos = Target.Value.EnterObj.transform.position;
            Navmesh.Value.SetDestination(targetPos);
        }
        if(_minion.GetVisualObject().activeSelf)
            Work.Value.CheckBuilding(_minion);

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if(_minion.GetVisualObject() != null)
            _minion.GetVisualObject().SetActive(true);
        
        
        Work.Value.ExitWork();
    }
}

