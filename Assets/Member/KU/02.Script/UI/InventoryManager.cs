using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private float _durTime;
    [SerializeField] private float _fallPo;
    [SerializeField] private Button _oneButton;
    [SerializeField] private Button _twoButton;
    [SerializeField] private TextMeshProUGUI _pageTex;
    [SerializeField] private GameObject _pagePrefab;
    [SerializeField] private RectTransform _subTransform;
    private RectTransform _rectTransform;

    [SerializeField] private int _nowPage = 1;
    [SerializeField] private int _maxPage = 2;

    List<InventoryCreate> _invPrefObj = new List<InventoryCreate>();

    bool isNowClose = false;
    bool _isMoveInv = false;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        for(int i = 0; i < _maxPage; i++)
        {
            GameObject obj = Instantiate(_pagePrefab, gameObject.transform);
            _invPrefObj.Add(obj.GetComponent<InventoryCreate>());
        }
    }

    private void Start()
    {
        _oneButton = _invPrefObj[0]._needed[0].GetComponent<Button>();
        _twoButton = _invPrefObj[_maxPage - 1]._needed[1].GetComponent<Button>();
        _pageTex = _invPrefObj[0]._needed[2].GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {

        if(!_isMoveInv && Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            InvPageChange(false);
        }
        if (!_isMoveInv && Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            InvPageChange(true);
        }
    }
    
    public void InvPageChange(bool isNowOne)
    {
        _isMoveInv = true;
        if (isNowOne)
        {
            if (_nowPage < _maxPage)
                _nowPage++;
            _pageTex.text = $"{_nowPage} / {_maxPage} Page";
            _twoButton.gameObject.SetActive(false);
            _subTransform.DOAnchorPosX(-_fallPo, _durTime).OnComplete(() =>
            {
                _isMoveInv = false;
                _oneButton.gameObject.SetActive(true);
            });
        }
        else if (!isNowOne)
        {
            if (_nowPage > 1)
                _nowPage--;
            _pageTex.text = $"{_nowPage} / {_maxPage} Page";
            _oneButton.gameObject.SetActive(false);
            _subTransform.DOAnchorPosX(0, _durTime).OnComplete(() =>
            {
                _isMoveInv = false;
                _twoButton.gameObject.SetActive(true);
            });
        }
    }

    public void CloseInv()
    {
        if (!isNowClose)
        {
            isNowClose = true;
            _rectTransform.DOAnchorPosY(-210, _durTime).OnComplete(() =>
            {

            });
        }
        else if (isNowClose)
        {
            isNowClose = false;

            _rectTransform.DOAnchorPosY(0, _durTime).OnComplete(() =>
            {

            });
        }
    }
}
