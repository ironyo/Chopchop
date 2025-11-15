using System;
using System.Collections.Generic;
using Member.CHJ._02.Scripts;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Sleep", story: "[Self] Sleep [House] [NavMesh]", category: "Action", id: "f3fece142aa3a1f7c90885af7376da67")]
public partial class SleepAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Building> House;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
    protected override Status OnStart()
    {
        var buildManager = MinionManager.Instance.MinionsBuildingManager;
        if (buildManager.TryEnterBuilding(House.Value))
        {
            NavMesh.Value.SetDestination(House.Value.transform.position);
            return Status.Success;
        }

        return Status.Failure;
    }
}