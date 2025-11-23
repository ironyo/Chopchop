using UnityEngine;
using DG.Tweening;
public class SettingUI : MonoBehaviour
{
    private Sequence _sequence;
    [SerializeField] private GameObject block;
    public void Open()
    {
        gameObject.SetActive(true);
        block.SetActive(true);
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        _sequence.SetUpdate(true);
        _sequence.Append(transform.DOScale(1, 0.5f).SetEase(Ease.InOutElastic));
        Debug.Log(gameObject);
        _sequence.onComplete += () => Time.timeScale = 0;
    }
    public void Close()
    {
        Time.timeScale = 1;
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        _sequence.SetUpdate(true);
        _sequence.Append(transform.DOScale(0, 0.5f).SetEase(Ease.InOutElastic));
        _sequence.onComplete += () =>
        {
            block.SetActive(false);
            gameObject.SetActive(false);
        };
        _sequence.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
