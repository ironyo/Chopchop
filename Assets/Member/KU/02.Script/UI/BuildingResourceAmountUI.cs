using System.Collections;
using UnityEngine;

public class BuildingResourceAmountUI : MonoBehaviour
{
    private Vector2 _target;
    void Start()
    {
        _target = new Vector2(transform.position.x, transform.position.y + 1.5f);
        StartCoroutine(DestroyObj());
    }
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _target, 0.02f);
    }
    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
