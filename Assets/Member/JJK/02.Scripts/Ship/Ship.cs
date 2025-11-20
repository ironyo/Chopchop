using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private float Interval = 0.5f;

    private Vector3 landPoint;
    
    private int enemyCount;
    private bool canFlip;

    public void Initialize(Vector3 position, int count, bool canFlip)
    {
        landPoint = position;
        enemyCount = count;
        this.canFlip = canFlip;
        
        StartCoroutine(MoveToLandPoint());
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
        StartCoroutine(DisembarkRoutine());
    }

    private IEnumerator DisembarkRoutine()
    {
        InvasionManager.Instance.isLanding = true;

        for (int i = 0; i < enemyCount; i++)
        {
            Transform point = spawnPoint[i % spawnPoint.Length];
            GameObject enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);
            
            if (canFlip)
                FlipY(enemy);
            
            var agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = true;
            
            UnitManager.Instance.RegisterEnemy(enemy.transform);
            
            yield return new WaitForSeconds(Interval);
        }

        // 모두 내린 뒤 배 제거
        Destroy(gameObject);
    }
}
