using UnityEngine;

[CreateAssetMenu(fileName = "Water", menuName = "Scriptable Objects/Water")]
public class Water : ToolSO
{
    public override void ToolApply(TestMinion minion, int amount)
    {
        minion.EatWater(amount);
    }
}
