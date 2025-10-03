using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsFighting", story: "if [Fighting]", category: "Conditions", id: "29bf3ea62f45efd3be12a8e9e42704b2")]
public partial class IsFightingCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> Fighting;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
