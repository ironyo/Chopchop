using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    [SerializeField] private float _durTime;
    [SerializeField] private float _fallPos;
    [SerializeField] private float _closPos;
    [SerializeField] private GameObject _pagePrefab;
    [SerializeField] private GameObject _texPref;
    [SerializeField] private GameObject _startTextPref;

    public GameObject startText;

    [SerializeField] RectTransform _rectTransform;

    public int _nowPage = 1;
    [SerializeField] private int _maxPage = 2;

    [SerializeField] List<InventoryCreate> _invPrefObj = new();
    public List<BuildingSO> _buildSO = new();

    public bool IsNowClose { get; private set; } = true;
    bool _isMoveInv = false;
    protected override void Awake()
    {
        base.Awake();
        for(int i = 0; i < _maxPage; i++)
        {
            GameObject obj = Instantiate(_pagePrefab, transform);
            _invPrefObj.Add(obj.GetComponent<InventoryCreate>());
        }
        for (int i = 0; i < _invPrefObj.Count; i++)
        {
            _invPrefObj[i].pageNum = i;
            _invPrefObj[i].manager = this;
        }

        _rectTransform.anchoredPosition = new Vector2(
    _rectTransform.anchoredPosition.x,
    0                 // ´ÝÈû »óÅÂ
);
        IsNowClose = true;
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
                    _invPrefObj[i].invBoxes[j].BuildingSO = _buildSO[count];
                    count++;
                }
            }
        }
    }



    private void Update()
    {
        if (!IsNowClose && !_isMoveInv)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame && _nowPage > 1)
                InvPageChange(false);
            if (Keyboard.current.digit2Key.wasPressedThisFrame && _nowPage < _maxPage)
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

                _rectTransform.DOAnchorPosX(-_fallPos * _nowPage, _durTime).OnComplete(() =>
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

                _rectTransform.DOAnchorPosX(-_fallPos * (_nowPage - 2), _durTime).OnComplete(() =>
                {
                    _isMoveInv = false;
                });
                _nowPage--;
            }

        }
    }

    public void CloseInv()
    {
        BuildManager.Instance.isMoveInv = true;

        if (!IsNowClose)
        {
            IsNowClose = true;
            BuildManager.Instance.isMoveInv = false;
        }
        else if (IsNowClose)
        {
            IsNowClose = false;
            BuildManager.Instance.isMoveInv = false;
        }
    }

    public void StartBuildHQ()
    {
        startText = Instantiate(_startTextPref, GameObject.Find("Canvas").transform);
        List<InvBuild> build = _invPrefObj[_invPrefObj.Count - 1].invBoxes;
        build[build.Count - 1].Building();

        Destroy(_invPrefObj[_invPrefObj.Count - 1].invBoxes[_invPrefObj[_invPrefObj.Count - 1].invBoxes.Count - 1].gameObject);
        _invPrefObj[_invPrefObj.Count - 1].invBoxes.RemoveAt(_invPrefObj[_invPrefObj.Count - 1].invBoxes.Count - 1);
    }
}
