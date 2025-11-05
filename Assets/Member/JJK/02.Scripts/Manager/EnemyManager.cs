using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public List<Unit> enemies = new List<Unit>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemy(Unit unit)
    {
        enemies.Add(unit);
    }

    public void UnregisterEnemy(Unit unit)
    {
        enemies.Remove(unit);

        if (enemies.Count == 0)
            BattleManager.Instance.Win();
    }

    public Unit GetNearestEnemy(Unit requester)
    {
        if (enemies.Count == 0) return null;

        Unit nearest = enemies.OrderBy(u => 
            Vector2.Distance(u.transform.position, requester.transform.position)).First();

        return nearest;
    }
}
