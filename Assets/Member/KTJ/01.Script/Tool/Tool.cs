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
    }
    public void Use(GameObject target)
    {
        if (target.TryGetComponent<TestMinion>(out TestMinion minion))
        {
            if (ToolLevel == 1) // ���� 1�� �������� ���� �ٷ� ����
            {
                ToolSO.ToolApply(minion, ToolSO.Amount[0]);
            }
            else if (ToolLevel == 2) // ������ ������ 2�ϰ��, ���� ���ñ�� ����
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
        // 선택 효과
        minion.GetComponent<SelectEffect>()?.RangeSelect(radius);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            minion.transform.position,
            radius,
            LayerMask.GetMask("Minion_TJ")
        );

        foreach (Collider2D minionCollider in colliders)
        {
            TestMinion comp = minionCollider.GetComponent<TestMinion>();

            if (comp == null)
                continue; // ❗ TestMinion 없는 Collider 제외

            ToolSO.ToolApply(comp, ToolSO.Amount[ToolLevel - 1]);
        }
    }

}
