using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InvBuild : MonoBehaviour
{
    public BuildingSO buildingSO { get; set; }
    [SerializeField] TextMeshProUGUI tex;
    [SerializeField] Image visual;
    Button _button;
    private BuildManager _buildManager;
    
    private void Awake()
    {
        _buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        _button = GetComponent<Button>();
    }
    private void Start()
    {
        if(buildingSO != null)
        {
            _button.onClick.AddListener(ButtonClick);
            _button.onClick.AddListener(() => { _buildManager.Buildings(true, buildingSO); });
            tex.text = buildingSO.buildName;
        }
    }
    public void ButtonClick()
    {
        Debug.Log($"º±≈√ : {buildingSO.buildName}");
    }
}