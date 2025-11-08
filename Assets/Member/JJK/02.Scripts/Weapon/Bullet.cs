using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;

    private float lifeTime = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyCoroutine());
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        PoolManager.Instance.ReturnToPool("BulletPool", gameObject);
    }
}
