using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    
    private List<Unit> attackers = new List<Unit>();
    private List<Unit> defenders = new List<Unit>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnUnit(UnitData data, UnitTeam team)
    {
        GameObject obj = Instantiate(data.prefab, data.spawnPoint);
        Unit unit = obj.GetComponent<Unit>();

        unit.data = data;
        unit._team = team;
        RegisterUnit(unit);
    }

    public void RegisterUnit(Unit unit)
    {
        if (unit._team == UnitTeam.Attacker)
        {
            attackers.Add(unit);
        }
        else if (unit._team == UnitTeam.Defender)
        {
            defenders.Add(unit);
        }
    }

    public void UnregisterUnit(Unit unit)
    {
        if (unit._team == UnitTeam.Attacker)
        {
            attackers.Add(unit);
        }
        else if (unit._team == UnitTeam.Defender)
        {
            defenders.Add(unit);
        }
    }
}
