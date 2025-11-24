using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Member.CHJ._02.Scripts.Ui
{
    public class EscUI : MonoBehaviour
    {
        [SerializeField] private SettingUI _setting;
        [SerializeField] private GameObject background;
        private bool _isOpened;
        private void Update()
        {
            if(_isOpened) return;
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                _isOpened = true;
                background.SetActive(true);
            }
        }

        public void Continue()
        {
            _isOpened = false;
            background.SetActive(false);
        }

        public void Exit()
        {
            _isOpened = false;
            Application.Quit();
        }

        public void MainMenu()
        {
            _isOpened = false;
            SceneManager.LoadScene("TJ_Start");
        }

        public void Setting()
        {
            _isOpened = false;
            background.SetActive(false);
            _setting.Open();
        }
    }
}