using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{
<<<<<<< Updated upstream:Assets/Member/JJK/02.Scripts/Manager/EnemyManager.cs
    public List<Unit> enemies = new List<Unit>();

    protected override void Awake()
=======
    public List<Transform> enemies = new List<Transform>();
    public List<Transform> players = new List<Transform>();
    
    public void RegisterPlayer(Transform player)
>>>>>>> Stashed changes:Assets/Member/JJK/02.Scripts/Manager/UnitManager.cs
    {
        enemies.Add(player);
    }

    public void UnregisterPlayer(Transform player)
    {
        enemies.Remove(player);

        if (enemies.Count == 0)
            BattleManager.Instance.Win();
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
    
    public Transform GetNearestPlayer(Transform requester)
    {
        if (enemies.Count == 0) return null;

        Transform nearest = players.OrderBy(u => 
            Vector2.Distance(u.transform.position, requester.position)).First();

        return nearest;
    }
}
