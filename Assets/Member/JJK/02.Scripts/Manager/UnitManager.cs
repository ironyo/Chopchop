using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{
    public List<Transform> enemies = new List<Transform>();
    public List<Transform> players = new List<Transform>();
    
    public void RegisterPlayer(Transform player)
    {
        enemies.Add(player);
    }

    public void UnregisterPlayer(Transform player)
    {
        enemies.Remove(player);

        if (enemies.Count == 0)
            BattleManager.Instance.Win();
    }

    public void RegisterEnemy(Transform unit)
    {
        enemies.Add(unit);
    }

    public void UnregisterEnemy(Transform unit)
    {
        enemies.Remove(unit);

        if (enemies.Count == 0)
            BattleManager.Instance.Win();
    }

    public Transform GetNearestEnemy(Transform requester)
    {
        if (enemies.Count == 0) return null;

        Transform nearest = enemies.OrderBy(u => 
            Vector2.Distance(u.transform.position, requester.transform.position)).First();

        return nearest;
    }
    
    public Transform GetNearestPlayer(Transform requester)
    {
        if (enemies.Count == 0) return null;

        Transform nearest = players.OrderBy(u => 
            Vector2.Distance(u.transform.position, requester.position)).First();

        return nearest;
    }
}
