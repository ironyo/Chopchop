using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Init(int damage)
    {
        _text.text = damage.ToString();
        TextAnimation();
    }

    private void TextAnimation()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.2f);
        transform.DOMoveY(transform.position.y + 1.5f, 0.8f).SetEase(Ease.OutQuad);
        _text.DOFade(0, 0.8f);
        Destroy(gameObject, 1f);
    }
}
