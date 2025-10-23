using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] landPoints;
    
    private Transform _spawnPoint;
    private Transform _landPoint;
    private int index;

    public void SpawnShip(int enemyCount)
    {
        index = Random.Range(0, spawnPoints.Length);
        _spawnPoint = spawnPoints[index];
        _landPoint = landPoints[index];
        
        var shipObj = Instantiate(shipPrefab, _spawnPoint.position, Quaternion.identity);
        var ship = shipObj.GetComponent<Ship>();
        if (index < 2)
            ship.Initialize(_landPoint.position, enemyCount, true);
        else
            ship.Initialize(_landPoint.position, enemyCount, false);
    }
}
