using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InvBuild : MonoBehaviour
{
    public BuildingSO BuildingSO { get; set; }
    [SerializeField] TextMeshProUGUI tex;
    [SerializeField] Image visual;
    Button _button;

    private UseToolTip _toolTip;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _toolTip = GetComponent<UseToolTip>();
    }
    private void Start()
    {
        if(BuildingSO != null)
        {
            if(BuildingSO == InventoryManager.Instance._buildSO[InventoryManager.Instance._buildSO.Count - 1])
            {

            }
            _button.onClick.AddListener(() => { BuildManager.Instance.Buildings(true, BuildingSO); });
            _button.onClick.AddListener(() => { ResourceUIManager.Instance.ChooseButton(); });
            tex.text = BuildingSO.buildName;
        }
        _toolTip.tip = BuildingSO.explaneStr;
    }

    public void Building()
    {
        BuildManager.Instance.Buildings(true, BuildingSO);
        ResourceUIManager.Instance.ChooseButton();
    }
}