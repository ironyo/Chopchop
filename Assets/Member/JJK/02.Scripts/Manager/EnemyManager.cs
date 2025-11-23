using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    private readonly List<EnemyUnit> enemies = new();

    public void RegisterEnemy(EnemyUnit enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyUnit enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0 && TutorialManager.Instance.GetCurrentStepId() == "Invasion")
        {
            TutorialManager.Instance.CompleteCurrentStepExternally();
        }
    }

    public int GetEnemyCount() => enemies.Count;
}
