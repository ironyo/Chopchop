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
        minion.gameObject.GetComponent<SelectEffect>().RangeSelect(radius);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(minion.transform.position, radius, LayerMask.GetMask("Minion_TJ"));
        List<TestMinion> foundMinions = new List<TestMinion>();
        foreach (Collider2D minionCollider in colliders)
        {
            Debug.Log("aaaaaa");
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
