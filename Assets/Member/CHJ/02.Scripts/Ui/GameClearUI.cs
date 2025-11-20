using DG.Tweening;
using Member.CHJ._02.Scripts;
using Member.CHJ._02.Scripts.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUI : MonoSingleton<GameClearUI>
{
    [SerializeField] private GameObject _gameoverUI;
    [SerializeField] private Image _gameoverBackGround;
    [SerializeField] private TextMeshProUGUI _mainText;
    [SerializeField] private TextMeshProUGUI _resourceText;
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private TextMeshProUGUI _maxText;
    private Sequence _sequence;

    public void GameClearStart()
    {
        GameFinishManager.Instance.onGameClear?.Invoke();
    }
    public void GameClear()
    {
        _gameoverUI.SetActive(true);
        _gameoverBackGround.DOFade(1, 1.2f).OnComplete(ShowText); // 페이드인
        Debug.Log("GAMECLEAR" + _gameoverUI.activeSelf);
            
    }

    private void ShowText()
    {
        _mainText.DOFade(1, 1f);
        _resourceText.DOFade(1, 1f);
        _dayText.DOFade(1, 1f);
        _maxText.DOFade(1, 1f);
            
        _dayText.SetText($"생존 일수 : {TimeManager.Instance.Day}");
        _maxText.SetText($"최대 미니언 : {MinionManager.Instance.MinionMaxCount}");
    }
}
