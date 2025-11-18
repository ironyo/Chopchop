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
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(Self.Value.transform.position, 10);
        Debug.Log($"[CanFindMate] hits: {hits.Length} at {Self.Value.transform.position}");
        foreach (var hit in hits)
        {
            if (hit.gameObject == Self.Value.gameObject) // 자기 자신 제외
                continue;
            Debug.Log($"MINION DETECT {hit}");
            if (hit.TryGetComponent<Minion>(out var minion))
            {
                // 짝이 없고, 현재 짝짓기 중이 아닌 경우
                if (!minion.isFoundPartner && minion.isMating)
                {
                    Debug.Log("FindMinion");
                    return true;
                }
                Debug.Log($"Cant Found Minion Because Mating : {minion.isMating} or findingPartner : {minion.isFoundPartner}");
            }
        }

        return false;
    }
}