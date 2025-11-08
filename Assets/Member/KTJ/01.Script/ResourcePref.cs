using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePref : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countTxt;
    [SerializeField] private TextMeshProUGUI countTxt_s;
    [SerializeField] private Image icon;

    public void Set(int count, Sprite icon)
    {
        countTxt.text = count.ToString() + " :";
        countTxt_s.text = count.ToString() + " :";
        this.icon.sprite = icon;
    }

    public void UpdateCount(int count)
    {
        countTxt.text = count.ToString() + " :";
        countTxt_s.text = count.ToString() + " :";
    }
}
