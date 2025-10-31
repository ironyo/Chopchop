using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSs : MonoBehaviour
{
    public int count = 0;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;

    public void ResourceSet(BuildingSO buildData)
    {
        for (int i = 0; i < buildData.resourceTypeSOList.Count; i++)
        {
            Debug.Log(buildData.resourceTypeSOList[i]);
        }
        //text.text = $"{ResourceManager.Instance.resourceAmountDictionary[buildData.resourceTypeSOList[count]]} / {buildData.resourceTypeDic[buildData.resourceTypeSOList[count]]}";
        //image.sprite = buildData.resourceTypeSOList. <- 해당 타입 스프라이트
    }
}
