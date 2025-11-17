using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;

    private float lifeTime = 3f;
    private float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyCoroutine());
    }

    public void Initialize(float dmg)
    {
        damage = dmg;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //임시
        {
            other.GetComponent<Unit>().HealthCompo.GetDamage(10);
            //PoolManager.Instance.ReturnToPool("BulletPool", gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        //PoolManager.Instance.ReturnToPool("BulletPool", gameObject);
        Destroy(gameObject);
    }
}
