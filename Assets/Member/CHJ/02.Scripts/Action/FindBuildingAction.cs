using System;
using Member.CHJ._02.Scripts.SO;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindBuilding", story: "[self] find [job] buildings for [bool] to [Target]", category: "Action", id: "129e9807d7e7a918d6c004f97d309d40")]
public partial class FindBuildingAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<JobDataSO> Job;
    [SerializeReference] public BlackboardVariable<bool> Bool;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    protected override Status OnStart()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(Self.Value.transform.position, 30);
        Transform foundTrm = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Building>(out var building))
            {
                if (building.buildingSO == null ||
                    building.buildingSO != Job.Value.BuildingData ||
                    building.NowMinion >= building.buildingSO.maxMinion) continue;

                foundTrm = hit.transform;
                break;
            }
        }

        var agent = Self.Value.GetComponent<BehaviorGraphAgent>();
        if (agent != null)
        {
            agent.BlackboardReference.SetVariableValue("IsCanWorkBuilding", foundTrm != null);
            agent.BlackboardReference.GetVariableValue("IsCanWorkBuilding", out bool value);
            Debug.Log($"[FindBuildingAction] (Runtime Blackboard) IsCanWorkBuilding = {value}");
        }

        Target.Value = foundTrm;
        return Status.Success;
    }


}

