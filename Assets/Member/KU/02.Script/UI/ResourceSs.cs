using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResourceSs : MonoBehaviour
{
    int _count = 0;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;

    private BuildingSO buildSOData;

    public void ResourceSet(BuildingSO buildData, int count)
    {
        buildSOData = buildData;
        _count = count;
    }
    private void Start()
    {
        _image.sprite = buildSOData.resourceTypeCost[_count].resourceTypeSO.Icon;
    }
    private void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            ResourceManager.Instance.AddResource(buildSOData.resourceTypeCost[_count].resourceTypeSO, 1);
        }
        _text.text = $"{ResourceManager.Instance.resourceAmountDictionary[buildSOData.resourceTypeCost[_count].resourceTypeSO].Item1} / {buildSOData.resourceTypeCost[_count].amount}";
    }
}