using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameoverUI : MonoBehaviour
    {
        [Header("애니메이션 오브젝트")]
        [SerializeField] private GameObject _gameoverUI;
        [SerializeField] private Image _gameoverBackGround;
        [SerializeField] private RectTransform _mainText;
        [SerializeField] private RectTransform _subText;
        [SerializeField] private RectTransform _restart;
        [SerializeField] private RectTransform _menu;
        [SerializeField] private RectTransform _exit;
        [Header("애니메이션 이동 값")]
        [SerializeField] private float _mainTextMoveValue;
        [SerializeField] private float _subTextMoveValue;
        [SerializeField] private float _restartMoveValue;
        [SerializeField] private float _menuMoveValue;
        [SerializeField] private float _exitMoveValue;
        private Sequence _sequence;
        private bool _isCalled = false;

        private void Awake()
        {
            GameFinishManager.Instance.onGameOver.AddListener(Gameover);
        }

        public void GameOverStart()
        {
            GameFinishManager.Instance.onGameOver?.Invoke();
        }
        public void Gameover()
        {
            if (_isCalled)
                return;
            _isCalled = true;
            _gameoverUI.SetActive(true);

            _gameoverBackGround.DOFade(0.8f, 1.2f).OnComplete(ShowText); // 페이드인
        }

        private void ShowText()
        {
            // 기존 트윈 있으면 제거
            if (_sequence != null && _sequence.IsActive())
            {
                _sequence.Kill();
                _sequence = null;
            }

            // TimeScale 0에서도 동작하도록
            _sequence = DOTween.Sequence().SetUpdate(true);

            // 시작 시 위치 초기화 (중복트윈 방지)
            _mainText.anchoredPosition = new Vector2(_mainText.anchoredPosition.x, 0);
            _subText.anchoredPosition = new Vector2(_subText.anchoredPosition.x, 0);
            _restart.anchoredPosition = new Vector2(_restart.anchoredPosition.x, 0);
            _menu.anchoredPosition = new Vector2(_menu.anchoredPosition.x, 0);
            _exit.anchoredPosition = new Vector2(_exit.anchoredPosition.x, 0);

            // 애니메이션 시퀀스
            _sequence.Append(_mainText.DOAnchorPosY(_mainTextMoveValue, 0.9f));
            _sequence.Append(_subText.DOAnchorPosY(_subTextMoveValue, 0.9f));
            _sequence.Append(_restart.DOAnchorPosY(_restartMoveValue, 0.9f));
            _sequence.Append(_menu.DOAnchorPosY(_menuMoveValue, 0.9f));
            _sequence.Append(_exit.DOAnchorPosY(_exitMoveValue, 0.9f))
                     .OnComplete(() => Time.timeScale = 0);
        }


        public void Restart()
        {
            SceneChangeManager.Instance.OnSceneEnd(1);
        }
        public void Menu()
        {
            SceneChangeManager.Instance.OnSceneEnd(0);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}