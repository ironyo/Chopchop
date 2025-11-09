using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanFindMateMinion", story: "[Self]", category: "Conditions", id: "e4dc0c08f479c7422dfe3de2a97d2e8b")]
public partial class CanFindMateMinionCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    public override bool IsTrue()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(Self.Value.transform.position, 3);

        foreach (var hit in hits)
        {
            if (hit.gameObject == Self.Value) // 자기 자신 제외
                continue;

            if (hit.TryGetComponent<Minion>(out var minion))
            {
                // 짝이 없고, 현재 짝짓기 중이 아닌 경우
                if (!minion.isFoundPartner && !minion.isMating)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override void OnStart()
    {
        Debug.Log("[Condition] CanFindMateMinionCondition started");
    }
}