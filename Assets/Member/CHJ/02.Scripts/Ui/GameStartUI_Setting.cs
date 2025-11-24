using UnityEngine;
using UnityEngine.EventSystems;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameStartUISetting : GameStartUI
    {
        [SerializeField]private SettingUI _setting;

        public void OpenSetting()
        {
            _setting.Open();
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