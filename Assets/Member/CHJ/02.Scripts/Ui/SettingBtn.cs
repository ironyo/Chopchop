using UnityEngine;

namespace Member.CHJ._02.Scripts.Ui
{
    public class SettingBtn : MonoBehaviour
    {
        public void Restart()
        {
            SceneChangeManager.Instance.OnSceneEnd(1);
        }
        public void Exit()
        {
            Application.Quit();
        }

        public void Menu()
        {
            SceneChangeManager.Instance.OnSceneEnd(0);
        }
    }
}