using System;
using System.Collections.Generic;
using Member.CHJ._02.Scripts;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using NavMeshSurface = NavMeshPlus.Components.NavMeshSurface;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Sleep", story: "[Self] Sleep [House] [NavMesh] [WorkAction] [HpSystem]", category: "Action", id: "f3fece142aa3a1f7c90885af7376da67")]
public partial class SleepAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Building> House;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
    [SerializeReference] public BlackboardVariable<WorkActionScr> WorkAction;
    [SerializeReference] public BlackboardVariable<HealthSystem> HpSystem;
    private Minion _minion;
    protected override Status OnStart()
    {
        Debug.Log("Sleep Start");
        _minion = Self.Value.GetComponent<Minion>();
        var buildManager = MinionManager.Instance.MinionsBuildingManager;
        var building = buildManager.GetAvailableHouseCheckOnly(Self.Value.transform.position,MinionManager.Instance.houseSo, 30);
        if (building != null)
        {
            Debug.Log("FIND SLEEP BUILDING");
            NavMesh.Value.SetDestination(building.transform.position);
            WorkAction.Value.DoWork(building);
            return Status.Running;
        }
        else
        {
            return Status.Failure;
            
        }
    }

    protected override Status OnUpdate()
    {
        if (_minion.currentState != AiStates.Sleep)
        {
            return Status.Success;
        }
        
        WorkAction.Value.CheckBuilding(_minion);
        if (_minion.GetVisualObject().activeSelf)
        {
            HpSystem.Value.GetDamage(HpSystem.Value.maxHealth - HpSystem.Value.HP);
            WorkAction.Value.CheckBuilding(_minion);
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Debug.Log("Sleep End");
        WorkAction.Value.ExitWork();
        base.OnEnd();
    }
}