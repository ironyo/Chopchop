using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSs : MonoBehaviour
{
    public int count = 0;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;

    public void ResourceSet(BuildingSO buildData)
    {
        _text.text = $"{0} / {buildData.resourceTypeCost[count].amount}";
        //image.sprite = buildData.resourceTypeCost. <- 해당 타입 스프라이트
    }
}