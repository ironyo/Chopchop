using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;

[System.Serializable]
public class ToolSlot // ���������
{
    public ToolSO tool;
    public int count;
}

public class ToolManager : MonoSingleton<ToolManager>
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

    [SerializeField] private PlayerInput playerInput;
    #endregion

    private class UISlot
    {
        public GameObject slotObj;
        public TextMeshProUGUI countText;
    }
    private List<UISlot> toolSlotList = new List<UISlot>();
    private Tool currentTool = null;

    protected override void Awake()
    {
        base.Awake();
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

        playerInput.OnItemInvenKeyReleased += ToolGive;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentTool != null)
            {
                RemoveTool();
            }
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
                toolImage.sprite = toolInventory[i].tool.HighlitedIcon[MainTools[i].ToolLevel - 1];

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
        //if (toolInventory[index].count <= 0)
        //{
        //    return false;
        //}
        //else
        //{
        //    return true;
        //}

        return true;
    }

    private void ToolCountSpent(int index)
    {
        //ToolSlot toolSlot = toolInventory[index];
        //toolSlot.count--;
        //toolInventory[index] = toolSlot;
        //toolSlotList[index].countText.text = toolSlot.count.ToString();

    }

    private void ToolGive(int index)
    {
        if (currentTool != null) return;

        if (GameManager.Instance.IsGameStarted == false) return;

        if (CanGiveTool(index))
        {
            currentTool = MainTools[index];
            handToolImage.sprite = currentTool.ToolSO.HighlitedIcon[MainTools[index].ToolLevel - 1];
            handToolImage.gameObject.SetActive(true);

            ToolCountSpent(index);
        }
        else
            currentTool = null;
    }

    private void RemoveTool()
    {
        currentTool = null;
        handToolImage.gameObject.SetActive(false);
    }

    public bool UseTool(GameObject target)
    {
        if (currentTool == null)
        {
            RemoveTool();
            return false;
        }
        else
        {
            currentTool.Use(target);
            RemoveTool();
            return true;
        }
    }
}
