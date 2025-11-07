using System.Collections;
using UnityEngine;
using DG.Tweening;

public abstract class UIBase : MonoBehaviour
{
    [Header("Toggle UI Setting")]
    public GameObject toggleObject;

    private bool isOpen = false;

    public IEnumerator Open()
    {
        if (isOpen) yield break;
        isOpen = true;

        toggleObject.SetActive(true);
        yield return StartCoroutine(OpenEffect());
    }

    public IEnumerator Close()
    {
        if (!isOpen) { toggleObject.SetActive(false); yield break; }
        isOpen = false;

        yield return StartCoroutine(CloseEffect());
        toggleObject.SetActive(false);
    }

    public void SetActiveImmediate(bool active)
    {
        isOpen = active;
        toggleObject.SetActive(active);
    }

    public abstract IEnumerator OpenEffect();
    public abstract IEnumerator CloseEffect();

    public void ToggleBtn()
    {
        UIManager.Instance?.Toggle(this);
        SoundManager.instance.ClickSound_01();
    }

    protected static Tween DoY(RectTransform rt, float y, float duration)
    {
        rt.DOKill();
        return rt.DOAnchorPosY(y, duration);
    }
}
