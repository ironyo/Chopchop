using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;

    private float lifeTime = 3f;
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyCoroutine());
    }

    public void Initialize(int dmg)
    {
        damage = dmg;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.TryGetComponent<Unit>(out Unit unit) && unit._unitType == UnitType.Enemy)
    //     {
    //         unit.HealthCompo.GetDamage(damage);
    //         //PoolManager.Instance.ReturnToPool("BulletPool", gameObject);
    //         Destroy(gameObject);
    //     }
    // }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        //PoolManager.Instance.ReturnToPool("BulletPool", gameObject);
        Destroy(gameObject);
    }
}
