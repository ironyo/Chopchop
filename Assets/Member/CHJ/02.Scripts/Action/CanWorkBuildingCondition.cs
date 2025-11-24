using Member.CHJ._02.Scripts.SO;
using System;
using Member.CHJ._02.Scripts;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanWorkBuildingCondition ", story: "[Self] [Target] [WorkScript]", category: "Conditions", id: "77f4bd65fd1b66f7c36a94fd729a6b23")]
public partial class CanWorkBuildingCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Building> Target;
    [SerializeReference] public BlackboardVariable<WorkActionScr> WorkScript;
    

    public override bool IsTrue()
    {
        if (Self.Value == null)
            return false;
        if (WorkScript.Value.isWorking)
            return true;
        var buildingManager = MinionManager.Instance.MinionsBuildingManager;
        
        var building = buildingManager.GetAvailableHouseCheckOnly(Self.Value.transform.position, WorkScript.Value.jobData.buildingData, 30f);

        if (building != null)
            Target.Value = building;
        return building != null;
    }

}
