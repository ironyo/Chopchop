using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResourceSs : MonoBehaviour
{
    public int count = 0;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;

    private BuildingSO buildSOData;

    public void ResourceSet(BuildingSO buildData)
    {
        buildSOData = buildData;
    }
    private void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            ResourceManager.Instance.AddResource(buildSOData.resourceTypeCost[count].resourceTypeSO, 1);
        }
        _text.text = $"{ResourceManager.Instance.resourceAmountDictionary[buildSOData.resourceTypeCost[count].resourceTypeSO]} / {buildSOData.resourceTypeCost[count].amount}";
    }
}