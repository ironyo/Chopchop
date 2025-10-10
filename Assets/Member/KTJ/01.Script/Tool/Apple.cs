using UnityEngine;

[CreateAssetMenu(fileName = "Apple", menuName = "Scriptable Objects/Apple")]
public class Apple : ToolSO
{
    public int Amount;
    public override void Use(GameObject target)
    {
        if (target.TryGetComponent<TestMinion>(out TestMinion minion))
        {
            minion.EatFood(Amount);
        }
    }
}
