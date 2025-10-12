using UnityEngine;

[CreateAssetMenu(fileName = "Soap", menuName = "Scriptable Objects/Soap")]
public class Soap : ToolSO
{
    public int Amount;
    public override void Use(GameObject target)
    {
        if (target.TryGetComponent<TestMinion>(out TestMinion minion))
        {
            minion.Clean(Amount);
        }
    }
}
