using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    [Header("�⺻ ����")]
    [SerializeField] private List<ToolSO> toolInventory = new List<ToolSO>();
    private ToolSO currentTool = null;

    [Header("������Ʈ ����")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject invenSlotPref;
    [SerializeField] private GameObject toolRemoveBtnPref;
    [SerializeField] private Image handToolImage;

    private List<GameObject> toolSlotList = new List<GameObject>(); 

    private void Awake()
    {
        Initlize();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� �̵� �ÿ��� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
        }
    }

    private void Initlize()
    {
        for (int i = -1; i < toolInventory.Count; i++)
        {
            if (i == -1)
            {
                GameObject clonedRemoveBtn = Instantiate(toolRemoveBtnPref, slotParent.transform);
                Button removeBtn = clonedRemoveBtn.GetComponent<Button>();
                removeBtn.onClick.AddListener(() => RemoveTool());

                continue;
            }

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
        currentTool = toolInventory[index];
        handToolImage.sprite = currentTool.HighlitedIcon;
        handToolImage.gameObject.SetActive(true);
    }

    private void RemoveTool()
    {
        currentTool = null;
        handToolImage.gameObject.SetActive(false);
    }

    public void UseTool(GameObject target)
    {
        if (currentTool == null)
        {
            Debug.Log("������ ������ �����ϴ�");
            return;
        }
        else
        {
            currentTool.Use(target);
            RemoveTool();
        }
    }
}
