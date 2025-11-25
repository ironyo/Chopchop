using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using DG.Tweening;

public class InvBuild : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BuildingSO BuildingSO { get; set; }
    [SerializeField] TextMeshProUGUI tex;
    [SerializeField] Image visual;
    Button _button;

    private UseToolTip _toolTip;

    private Vector3 _startPos;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
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
        _startPos = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOAnchorPosY(_startPos.y-20, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOAnchorPosY(_startPos.y - 50, 0.1f);
    }
}