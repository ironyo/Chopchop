using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private float distance = 5f;
    
    private Vector3 _spawnPoint;
    private int _index;

    public void SpawnShip(int enemyCount)
    {
        int deg = Random.Range(0, 360);
        float x = Mathf.Cos(deg * Mathf.Deg2Rad) * distance;
        float y = Mathf.Sin(deg * Mathf.Deg2Rad) * distance;
        _spawnPoint = transform.position + new Vector3(x, y, 0);
        
        var shipObj = Instantiate(shipPrefab, _spawnPoint, Quaternion.identity);
        var ship = shipObj.GetComponent<Ship>();
        ship.Initialize(enemyCount);
    }
}
