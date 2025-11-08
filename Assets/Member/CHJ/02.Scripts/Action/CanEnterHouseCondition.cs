using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanEnterHouse", story: "[Self] [Target]", category: "Conditions", id: "6344f805c832a9a1b72d591876557b3d")]
public partial class CanEnterHouseCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    public override bool IsTrue()
    {
        if (Self.Value == null)
            return false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(Self.Value.transform.position, 30f);
        Transform foundTrm = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Building>(out var building))
            {
                if (building.buildingSO == null) continue;
                if (building.buildingSO.name != "NormalBuildSO") continue;
                if (building.NowMinion >= building.maxMinion) continue;

                foundTrm = hit.transform;
                break;
            }
        }

        if (foundTrm != null)
        {
            Target.Value = foundTrm;
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
