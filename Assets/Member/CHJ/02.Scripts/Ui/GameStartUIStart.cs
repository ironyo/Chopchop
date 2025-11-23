using UnityEngine;
using UnityEngine.EventSystems;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameStartUIStart : GameStartUI
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

        public void StartGame()
        {
            SceneChangeManager.Instance.ExitStartScene();
        }
    }
}