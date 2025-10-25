using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

[System.Serializable]
public class ToolInfo
{
    public ToolSO tool;
    [Range(0, 50)]
    public int price;
    public string toolAbil; // Åø ´É·Â

    ToolManager toolManager;
}

public class ToolShop : MonoBehaviour
{
    [SerializeField] private RectTransform toolCardsParent;
    [SerializeField] private GameObject toolCardPref;
    [SerializeField] private Image WhiteBg;
    private List<ToolCard> toolCards = new List<ToolCard>();

    private bool canPurchase = true;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        List<Tool> tools = ToolManager.Instance.MainTools;
        for (int i = 0; i < tools.Count; i++)
        {
            ToolCard clonedCard = Instantiate(toolCardPref, toolCardsParent).gameObject.GetComponent<ToolCard>();
            clonedCard.Set(tools[i]);
            toolCards.Add(clonedCard);

            clonedCard.OnPurchase += (clickedCard) =>
            {
                int idx = toolCards.IndexOf(clickedCard);
                PurchaseTool(idx);
            };
        }
    }

    private void PurchaseTool(int idx)
    {
        if (canPurchase == false) return;

        List<Tool> tools = ToolManager.Instance.MainTools;
        Debug.Log(idx);
        tools[idx].UpgradeLevel();
        PuchaseEffect(idx);
    }

    private void PuchaseEffect(int idx)
    {
        List<Tool> tools = ToolManager.Instance.MainTools;
        HorizontalLayoutGroup group = toolCardsParent.gameObject.GetComponent<HorizontalLayoutGroup>();
        group.enabled = false;
        Sequence seq = DOTween.Sequence();

        canPurchase = false;

        ToolCard mainCard = null;
        List<ToolCard> otherCards = new List<ToolCard>();

        for (int i = 0; i < toolCards.Count; i++)
        {
            if (i == idx)
            {
                mainCard = toolCards[i];

                mainCard.WhiteBg.gameObject.SetActive(true);
                seq.Join(mainCard.WhiteBg.DOFade(1f, 2f));
                seq.Join(mainCard.rectransform.DOLocalMoveX(0f, 3f).SetEase(Ease.OutQuad));
                seq.Join(mainCard.rectransform.DOScale(1.1f, 3f));
                seq.Join(mainCard.rectransform.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360));
            }
            else
            {
                seq.Join(toolCards[i].canvasgroup.DOFade(0f, 1f));
                seq.Join(toolCards[i].rectransform.DOLocalMoveY(-50f, 1f));

                otherCards.Add(toolCards[i]);
            }
        }

        seq.AppendCallback(() =>
        {
            WhiteBg.gameObject.SetActive(true);
            WhiteBg.color = new Color(WhiteBg.color.r, WhiteBg.color.g, WhiteBg.color.b, 1);

            toolCards[idx].Set(tools[idx]);
            ToolManager.Instance.SetToolInven();
        });

        seq.Append(WhiteBg.DOFade(0f, 3f));
        seq.Join(mainCard.WhiteBg.DOFade(0f, 2f));


        seq.OnComplete(() =>
        {
            canPurchase = true;
            mainCard.WhiteBg.gameObject.SetActive(false);
            mainCard.rectransform.localScale = new Vector3(1, 1, 1);
            otherCards.ForEach(x =>
            {
                x.canvasgroup.alpha = 1f;
                x.rectransform.localPosition = new Vector3(0, 0, 0);
            });

            WhiteBg.gameObject.SetActive(false);
            group.enabled = true;

        });

    }
}
