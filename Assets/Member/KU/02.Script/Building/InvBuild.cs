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
    private BuildManager _buildManager;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void Start()
    {
        if(BuildingSO != null)
        {
            _button.onClick.AddListener(() => { BuildManager.Instance.Buildings(true, BuildingSO); });
            _button.onClick.AddListener(() => { ResourceUIManager.Instance.ChooseButton(); });
            tex.text = BuildingSO.buildName;
        }
    }
}