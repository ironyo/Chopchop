using DG.Tweening;
using UnityEngine;

public class SelectEffect : MonoBehaviour
{
    [SerializeField] private GameObject RangeSelectCircle;
    public void RangeSelect(float range)
    {
        RangeSelectCircle.SetActive(true);
        RangeSelectCircle.gameObject.transform.localScale = new Vector3(range * 2, range * 2, 0);
        SpriteRenderer sr = RangeSelectCircle.GetComponent<SpriteRenderer>();
        sr.DOFade(0f, 0.5f).OnComplete(() =>
        {
            RangeSelectCircle.SetActive(false);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        });

        
    }
}
