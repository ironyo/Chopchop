using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameoverUI : MonoSingleton<GameoverUI>
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

        protected override void Awake()
        {
            base.Awake();
        }

        public void GameOverStart()
        {
            GameFinishManager.Instance.onGameOver?.Invoke();
        }
        public void Gameover()
        {
            _gameoverUI.SetActive(true);
            
            _gameoverBackGround.DOFade(1, 1.2f).OnComplete(ShowText); // 페이드인
        }

        private void ShowText()
        {
            _sequence = DOTween.Sequence();   // ★ 반드시 필요

            _sequence.Append(_mainText.DOAnchorPosY(_mainTextMoveValue, 0.9f));
            _sequence.Append(_subText.DOAnchorPosY(_subTextMoveValue, 0.9f));
            _sequence.Append(_restart.DOAnchorPosY(_restartMoveValue, 0.9f));   
            _sequence.Append(_menu.DOAnchorPosY(_menuMoveValue, 0.9f));
            _sequence.Append(_exit.DOAnchorPosY(_exitMoveValue, 0.9f));

            _sequence.Play();
        }

        public void Restart()
        {
            SceneManager.LoadScene("Member/KTJ/02.Scene/TJ_Main");
        }
        public void Menu()
        {
            SceneManager.LoadScene("TJ_Start");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
