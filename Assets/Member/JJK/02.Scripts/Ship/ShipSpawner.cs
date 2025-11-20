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
    private int _index;

    public void SpawnShip(int enemyCount)
    {
        _index = Random.Range(0, spawnPoints.Length);
        _spawnPoint = spawnPoints[_index];
        _landPoint = landPoints[_index];
        
        var shipObj = Instantiate(shipPrefab, _spawnPoint.position, Quaternion.identity);
        var ship = shipObj.GetComponent<Ship>();
        ship.Initialize(_landPoint.position, enemyCount);
    }
}
