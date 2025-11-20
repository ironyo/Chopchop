using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    public List<Transform> enemies = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void RegisterEnemy(Transform enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(Transform enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
            BattleManager.Instance.Win();
    }

    public Transform GetNearestEnemy(Transform requester)
    {
        if (enemies.Count == 0) return null;

        Transform nearest = enemies.OrderBy(u => 
            Vector2.Distance(u.transform.position, requester.position)).First();

        return nearest;
    }
}
