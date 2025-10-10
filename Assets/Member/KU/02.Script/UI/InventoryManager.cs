using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private float _durTime;
    [SerializeField] private float _fallPo;
    [SerializeField] private GameObject _pagePrefab;
    [SerializeField] private GameObject _texPref;

    [SerializeField] RectTransform _rectTransform;

    public int _nowPage = 1;
    [SerializeField] private int _maxPage = 2;

    [SerializeField] List<InventoryCreate> _invPrefObj = new();
    [SerializeField] List<BuildingSO> _buildSO = new();

    bool isNowClose = false;
    bool _isMoveInv = false;
    private void Awake()
    {
        for(int i = 0; i < _maxPage; i++)
        {
            GameObject obj = Instantiate(_pagePrefab, gameObject.transform, transform);
            _invPrefObj.Add(obj.GetComponent<InventoryCreate>());

            GameObject tex = Instantiate(_texPref, transform.position, Quaternion.identity);

            tex.transform.SetParent(_rectTransform, false);

            RectTransform rect = tex.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(i * _fallPo - 550f, -220f);

            tex.GetComponent<TextMeshProUGUI>().text = $"{i + 1} / {_maxPage} Page";
        }
        for (int i = 0; i < _invPrefObj.Count; i++)
        {
            _invPrefObj[i].pageNum = i;
            _invPrefObj[i].manager = this;
        }

    }
    private void Start()
    {
        int count = 0;

        for (int i = 0; i < _invPrefObj.Count; i++)
        {
            for (int j = 0; j < _invPrefObj[i].invBoxes.Count; j++)
            {
                if (count < _buildSO.Count)
                {
                    _invPrefObj[i].invBoxes[j].buildingSO = _buildSO[count];
                    count++;
                }
            }
        }
    }

    private void Update()
    {

        if(!_isMoveInv && Keyboard.current.digit1Key.wasPressedThisFrame && _nowPage > 1)
        {
            InvPageChange(false);
        }
        if (!_isMoveInv && Keyboard.current.digit2Key.wasPressedThisFrame && _nowPage < _maxPage)
        {
            InvPageChange(true);
        }
    }

    public void InvPageChange(bool isNowOne)
    {
        if (isNowOne)
        {
            if (_nowPage < _maxPage)
            {
                _isMoveInv = true;

                _rectTransform.DOAnchorPosX(-_fallPo * _nowPage, _durTime).OnComplete(() =>
                {
                    _isMoveInv = false;
                });
                _nowPage++;
            }
        }
        else if (!isNowOne)
        {
            if (_nowPage > 1)
            {
                _isMoveInv = true;

                _rectTransform.DOAnchorPosX(-_fallPo * (_nowPage - 2), _durTime).OnComplete(() =>
                {
                    _isMoveInv = false;
                });
                _nowPage--;
            }

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
