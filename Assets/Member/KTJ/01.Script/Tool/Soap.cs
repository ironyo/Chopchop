using UnityEngine;

[CreateAssetMenu(fileName = "Soap", menuName = "Scriptable Objects/Soap")]
public class Soap : ToolSO
{
    public override void ToolApply(TestMinion minion, int amount)
    {
        minion.Clean(amount);
    }
}
