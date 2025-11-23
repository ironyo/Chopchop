using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Member.CHJ._02.Scripts.Ui
{
    public class WarningUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TextMeshProUGUI text1;
        [SerializeField] private TextMeshProUGUI text2;
        public void OpenUI()
        {
            gameObject.SetActive(true);
            transform.localScale = new Vector3(0,0.2f,1);
            text.DOFade(0, 0);
            text1.DOFade(0, 0);
            text2.DOFade(0, 0);
            
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScaleX(1, 0.7f));
            seq.Append(transform.DOScaleY(1, 0.7f));
            seq.AppendCallback(()=>
            {
                text.DOFade(1, 0.6f);
                text1.DOFade(1, 0.6f);
                text2.DOFade(1, 0.6f);
            });
        }
        public void CloseUI()
        {
            Sequence seq = DOTween.Sequence();
            text.DOFade(0, 0.6f);
            text1.DOFade(0, 0.6f);
            text2.DOFade(0, 0.6f);
            seq.AppendInterval(0.6f);
            seq.Append(transform.DOScaleY(0.2f, 0.7f));
            seq.Append(transform.DOScaleX(0, 0.7f)).OnComplete(()=>gameObject.SetActive(false));
            
        }
    }
}