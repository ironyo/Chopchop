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
    [SerializeReference] public BlackboardVariable<List<GameObject>> House;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMesh;
    private List<GameObject> HouseList = new List<GameObject>();
    protected override Status OnStart()
    {
        Collider2D[] houses = Physics2D.OverlapCircleAll(Self.Value.transform.position, 100);
        foreach (var house in houses)
        {
            if (house.CompareTag("House"))
            {
                HouseList.Add(house.gameObject);
            }
        }
        if (HouseList.Count != 0)
        {
            NavMesh.Value.SetDestination(HouseList[0].transform.position);
        }
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

