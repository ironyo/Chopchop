using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InvBuild : MonoBehaviour
{
    public BuildingSO buildingSO { get; set; }
    [SerializeField] TextMeshProUGUI tex;
    Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void Start()
    {
        if(buildingSO != null)
        {
            tex.text = buildingSO.buildName;
        }
        _button.onClick.AddListener(ButtonClick);
    }

    public void ButtonClick()
    {
        Debug.Log($"º±≈√ : {buildingSO.buildName}");
    }
}
