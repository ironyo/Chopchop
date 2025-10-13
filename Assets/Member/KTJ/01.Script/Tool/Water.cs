using UnityEngine;

[CreateAssetMenu(fileName = "Water", menuName = "Scriptable Objects/Water")]
public class Water : ToolSO
{
    public int Amount;
    public override void Use(GameObject target)
    {
        if (target.TryGetComponent<TestMinion>(out TestMinion minion))
        {
            minion.EatWater(Amount);
        }
    }
}
