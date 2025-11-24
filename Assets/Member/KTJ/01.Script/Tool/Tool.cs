using System.Collections.Generic;
using UnityEngine;

public class Tool
{
    public ToolSO ToolSO;
    public int ToolLevel { get; private set; } = 1;

    public Tool(ToolSO toolSO)
    {
        ToolSO = toolSO;
    }

    public void UpgradeLevel()
    {
        if (ToolLevel == 3) return;
        ToolLevel++;
        Debug.Log(ToolSO.ToolName + "이(가) " + ToolLevel + "로 업그레이드 되었습니다!");
    }
    public void Use(GameObject target)
    {
        if (target.TryGetComponent<TestMinion>(out TestMinion minion))
        {
            if (ToolLevel == 1) // 레벨 1은 범위선택 없이 바로 적용
            {
                ToolSO.ToolApply(minion, ToolSO.Amount[0]);
            }
            else if (ToolLevel == 2) // 아이템 레벨이 2일경우, 범위 선택기능 적용
            {
                RangeMinionUse(minion, ToolSO.defaultRadius);
            }
            else if (ToolLevel == 3)
            {
                RangeMinionUse(minion, ToolSO.defaultRadius * 2);
            }
        }
    }

    private void RangeMinionUse(TestMinion minion, float radius)
    {
        minion.gameObject.GetComponent<SelectEffect>().RangeSelect(radius);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(minion.transform.position, radius, LayerMask.GetMask("Minion_TJ"));
        List<TestMinion> foundMinions = new List<TestMinion>();
        foreach (Collider2D minionCollider in colliders)
        {
            TestMinion comp = minionCollider.GetComponent<TestMinion>();
            if (comp != null)
            {
                foundMinions.Add(comp);
            }
        }
        for (int i = 0; i < foundMinions.Count; i++)
        {
            ToolSO.ToolApply(foundMinions[i], ToolSO.Amount[ToolLevel - 1]);
        }
    }
}
