using UnityEngine;

[CreateAssetMenu(fileName = "Apple", menuName = "Scriptable Objects/Apple")]
public class Apple : ToolSO
{
    public override void ToolApply(TestMinion minion, int amount)
    {
        minion.EatApple(amount);
        Debug.Log("aa");
    }
}
