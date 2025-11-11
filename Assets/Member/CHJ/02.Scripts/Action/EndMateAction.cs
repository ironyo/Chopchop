using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EndMateAction", story: "[Minion]", category: "Action", id: "8094426a3bab89db78219631548ccb9b")]
public partial class EndMateAction : Action
{
    [SerializeReference] public BlackboardVariable<Minion> Minion;

    protected override Status OnStart()
    {
        Minion.Value.isMating = false;
        Debug.Log("NO MATE");
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

