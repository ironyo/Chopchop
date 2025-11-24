using System;
using Unity.Behavior;
using UnityEngine;

namespace Member.CHJ._02.Scripts.Action
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CanEnterHouse", story: "[Self] [Target] [HouseSO]", category: "Conditions", id: "6344f805c832a9a1b72d591876557b3d")]
    public partial class CanEnterHouseCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<Building> Target;
        [SerializeReference] public BlackboardVariable<BuildingSO> HouseSO;
    
        public override bool IsTrue()
        {
            Target.Value = null;
            if (Self.Value == null)
                return false;
            
            var buildingManager = MinionManager.Instance.MinionsBuildingManager;
            var house = buildingManager.GetAvailableHouseCheckOnly(Self.Value.transform.position, HouseSO.Value, 30f);

            return house != null;   
        }

    }
}