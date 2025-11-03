using Member.CHJ._02.Scripts.SO;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanWorkBuildingCondition ", story: "[Self] [Job] [Target]", category: "Conditions", id: "77f4bd65fd1b66f7c36a94fd729a6b23")]
public partial class CanWorkBuildingCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<JobDataSO> Job;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    public override bool IsTrue()
    {
        
        if (Self.Value == null || Job.Value == null)
            return false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(Self.Value.transform.position, 30f);
        Transform foundTrm = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Building>(out var building))
            {
                if (building.buildingSO == null) continue;
                if (building.buildingSO != Job.Value.BuildingData) continue;
                if (building.NowMinion >= building.buildingSO.maxMinion) continue;

                foundTrm = hit.transform;
                break;
            }
        }

        if (foundTrm != null)
        {
            Target.Value = foundTrm;
            Debug.Log($"[Condition] Found valid building for {Job.Value.name}.");
            return true;
        }
        else
        {
            Target.Value = null;
            Debug.Log("[Condition] No available building found.");
            return false;
        }
    }

}
