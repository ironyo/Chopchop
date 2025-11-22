using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class BuildingResourceAmountUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro textCompo;
    [SerializeField] private SpriteRenderer _sprite;
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
        yield return new WaitForSeconds(0.5f);
        textCompo.DOFade(0, 0.6f);
        _sprite.DOFade(0, 0.6f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
