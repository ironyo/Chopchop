using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToolCard : MonoBehaviour
{
    public TextMeshProUGUI NameTxt;
    public TextMeshProUGUI LevelTxt;
    public TextMeshProUGUI DescTxt;
    public TextMeshProUGUI PriceTxt;

    public Image IconImage;
    public Image ShadowIconImage;
    public Image WhiteBg;

    public Button PurchaseButton;

    public UnityAction<ToolCard> OnPurchase;
    public RectTransform rectransform;
    public CanvasGroup canvasgroup;

    private void Awake()
    {
        rectransform = GetComponent<RectTransform>();
        canvasgroup = GetComponent<CanvasGroup>();
    }

    public void Set(Tool tool)
    {
        NameTxt.text = tool.ToolSO.ToolName[tool.ToolLevel - 1].ToString();
        LevelTxt.text = "도구레벨 " + tool.ToolLevel.ToString();
        DescTxt.text = tool.ToolSO.ToolDesc[tool.ToolLevel - 1].ToString();

        IconImage.sprite = tool.ToolSO.Icon[tool.ToolLevel - 1];
        ShadowIconImage.sprite = tool.ToolSO.Icon[tool.ToolLevel - 1];

        PurchaseButton.onClick.AddListener(() =>
        {
            OnPurchase?.Invoke(this);
        });

        if (tool.ToolLevel == 3)
        {
            PriceTxt.text = "최대레벨";
        }
        else
        {
            PriceTxt.text = tool.ToolSO.Price[tool.ToolLevel - 1].ToString() + " 짱돌";
        }
    }
}
