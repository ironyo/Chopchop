using UnityEngine;
using UnityEngine.EventSystems;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameStartUIQuit : GameStartUI
    {
        public void Quit()
        {
            Application.Quit();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }
    }
}