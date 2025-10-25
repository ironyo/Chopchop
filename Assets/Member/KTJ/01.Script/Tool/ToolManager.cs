using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System;
using TMPro;
using System.Linq;

[System.Serializable]
public class ToolSlot // ���������
{
    public ToolSO tool;
    public int count;
}

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }
    public List<Tool> MainTools { get; private set; } = new List<Tool>(); // ��¥ ���� ��Ƶδ� ��

    #region �ν����� ����
    [Header("�⺻ ����")]
    [SerializeField] private List<ToolSlot> toolInventory = new List<ToolSlot>();
    public IReadOnlyList<ToolSlot> ToolInventory => toolInventory;

    [Header("������Ʈ ����")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject invenSlotPref;
    [SerializeField] private GameObject toolRemoveBtnPref;
    [SerializeField] private Image handToolImage;
    #endregion

    private class UISlot
    {
        public GameObject slotObj;
        public TextMeshProUGUI countText;
    }
    private List<UISlot> toolSlotList = new List<UISlot>();
    private Tool currentTool = null;

    private void Awake()
    {
        Init();

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

    private void Init()
    {
        for (int i = 0; i < toolInventory.Count; i++)
        {
            MainTools.Add(new Tool(toolInventory[i].tool));
        }

        SetToolInven();
    }

    public void SetToolInven()
    {
        toolSlotList.ForEach((x) => Destroy(x.slotObj.gameObject));
        toolSlotList.Clear();
        for (int i = 0; i < toolInventory.Count; i++) // i�� -1�� �ٲٸ� ������ ��ư ����
        {
            //if (i == -1)
            //{
            //    GameObject clonedRemoveBtn = Instantiate(toolRemoveBtnPref, slotParent.transform);
            //    Button removeBtn = clonedRemoveBtn.GetComponent<Button>();
            //    removeBtn.onClick.AddListener(() => RemoveTool());

            //    continue;
            //}

            int index = i;
            GameObject clonedSlot = Instantiate(invenSlotPref, slotParent.transform);

            if (clonedSlot.transform.Find("CountTxt").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI countTxt))
                countTxt.text = toolInventory[i].count.ToString();

            if (clonedSlot.transform.Find("ItemImage").TryGetComponent<Image>(out Image toolImage))
                toolImage.sprite = toolInventory[i].tool.Icon[MainTools[i].ToolLevel - 1];

            if (clonedSlot.TryGetComponent<Button>(out Button slotBtn))
                slotBtn.onClick.AddListener(() => ToolGive(index));


            toolSlotList.Add(new UISlot
            {
                slotObj = clonedSlot,
                countText = countTxt,
            });
        }
    }

    private bool CanGiveTool(int index)
    {
        if (toolInventory[index].count <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ToolCountSpent(int index)
    {
        ToolSlot toolSlot = toolInventory[index];
        toolSlot.count--;
        toolInventory[index] = toolSlot;
        toolSlotList[index].countText.text = toolSlot.count.ToString();

        Debug.Log("����" + toolSlot.tool.ToolName + " �� ���: " + toolSlot.count);
    }

    private void ToolGive(int index)
    {
        if (CanGiveTool(index))
        {
            currentTool = MainTools[index];
            handToolImage.sprite = currentTool.ToolSO.HighlitedIcon[MainTools[index].ToolLevel - 1];
            handToolImage.gameObject.SetActive(true);

            ToolCountSpent(index);
            Debug.Log(index);
        }
        else
            currentTool = null;
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
