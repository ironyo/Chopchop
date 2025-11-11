using System;
using System.Collections.Generic;
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
        NavMesh.Value.SetDestination(House.Value.gameObject.transform.position);
        return Status.Success;
    }

    protected override void OnEnd()
    {
        House.Value.Release();
        base.OnEnd();
    }
}