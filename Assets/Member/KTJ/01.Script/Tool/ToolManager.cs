using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.NetworkInformation;

public class ToolManager : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] private List<ToolSO> toolInventory = new List<ToolSO>();
    private ToolSO currentTool;

    [Header("오브젝트 설정")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject invenSlotPref;
    [SerializeField] private Image handToolImage;

    private List<GameObject> toolSlotList = new List<GameObject>(); 

    private void Awake()
    {
        Initlize();
    }

    private void Initlize()
    {
        for (int i = 0; i < toolInventory.Count; i++)
        {
            int index = i;

            GameObject clonedSlot = Instantiate(invenSlotPref, slotParent.transform);
            Button slotBtn = clonedSlot.GetComponent<Button>();
            slotBtn.onClick.AddListener(() => ToolGive(index));


            if (clonedSlot.transform.Find("ItemImage").TryGetComponent<Image>(out Image toolImage))
                toolImage.sprite = toolInventory[i].Icon;

            toolSlotList.Add(clonedSlot);
        }
    }

    private void ToolGive(int index)
    {
        Debug.Log(index);
        currentTool = toolInventory[index];
        handToolImage.sprite = currentTool.HighlitedIcon;
    }
}
