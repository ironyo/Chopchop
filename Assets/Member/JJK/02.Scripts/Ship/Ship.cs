    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float Interval = 0.5f;

    private Transform target;
    private int enemyCount;
    private bool hasLanded = false;

    private void Awake()
    {
        target =  GameObject.FindGameObjectWithTag("HQ").transform;
    }

    public void Initialize(int count)
    {
        enemyCount = count;
        
        StartCoroutine(MoveToLandPoint());
    }

    private IEnumerator MoveToLandPoint()
    {
        Vector2 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        while (!hasLanded)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;
            StartCoroutine(DisembarkRoutine());
            Debug.Log("collision");
        }
    }

    private IEnumerator DisembarkRoutine()
    {
        InvasionManager.Instance.isLanding = true;

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position + transform.right * 5, Quaternion.identity);
            
            var agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = true;
            
            yield return new WaitForSeconds(Interval);
        }
        
        Destroy(gameObject);
    }
}
