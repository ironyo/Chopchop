using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoint;

    private Vector3 landPoint;
    private List<GameObject> loadedEnemies = new List<GameObject>();
    
    public void Initialize(Vector3 position, int count, bool canFlip)
    {
        landPoint = position;
        LoadEnemyOnShip(count, canFlip);
        StartCoroutine(MoveToLandPoint());
    }

    private void LoadEnemyOnShip(int count, bool canFlip)
    {
        for (int i = 0; i < count; i++)
        {
            var enemy = Instantiate(enemyPrefab, spawnPoint[i].position, Quaternion.identity);
            enemy.transform.SetParent(transform);
            loadedEnemies.Add(enemy);
            
            if (canFlip) FlipY(enemy);
        }
    }

    private void FlipY(GameObject obj)
    {
        obj.transform.Rotate(0, 0, 180);
    }

    private IEnumerator MoveToLandPoint()
    {
        while (Vector3.Distance(transform.position, landPoint) > 0.1f)
        {
            Vector2 dir = landPoint - transform.position;
            float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            transform.position = Vector3.MoveTowards(transform.position, landPoint, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Land();
    }

    private void Land()
    {
        foreach (var enemy in loadedEnemies)
        {
            enemy.transform.SetParent(null);
            enemy.transform.rotation = Quaternion.identity;
            Destroy(gameObject);
        }
    }
}
