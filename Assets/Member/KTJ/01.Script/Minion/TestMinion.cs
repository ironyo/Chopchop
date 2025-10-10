using UnityEngine;

public class TestMinion : MonoBehaviour
{
    public int Hungry; // 0~100
    public int Thirsty; // 0~100
    public int Dirty; // 0~100

    public void EatFood(int amount)
    {
        Hungry = Mathf.Clamp(amount + Hungry, 0, 100);
    }

    public void EatWater(int amount)
    {
        Thirsty = Mathf.Clamp(amount + Thirsty, 0, 100);
    }

    public void Clean(int amount)
    {
        Dirty = Mathf.Clamp(amount + Dirty, 0, 100);
    }
}
