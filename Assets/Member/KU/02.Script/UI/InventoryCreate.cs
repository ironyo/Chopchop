using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class InventoryCreate : MonoBehaviour
{
    [SerializeField] private int _invCount = 7;
    [SerializeField] private GameObject _normalPref;
    [SerializeField] private GameObject _firstPref;
    [SerializeField] private GameObject _lastPref;
    [SerializeField] private GameObject _texPref;
    [SerializeField] List<GameObject> _invBoxes = new List<GameObject>();

    public int pageNum;
    public InventoryManager manager;

    [Header("ÂüÁ¶")]
    [SerializeField] Button _oneB;
    [SerializeField] Button _twoB;
    [SerializeField] TextMeshProUGUI _tex;

    private void Awake()
    {
        for (int i = 0; i < _invCount; i++)
        {
            GameObject pref;
            if (i == 0)
            {
                pref = Instantiate(_firstPref, transform.position, Quaternion.identity, transform);
                GameObject tex = Instantiate(_texPref, transform.position, Quaternion.identity, transform);
                _oneB = pref.GetComponent<Button>();
                _tex = tex.GetComponent<TextMeshProUGUI>();
            }

            else if (i == _invCount - 1)
            {
                pref = Instantiate(_lastPref, transform.position, Quaternion.identity, transform);
                _twoB = pref.GetComponent<Button>();
            }
            else
                pref = Instantiate(_normalPref, transform.position, Quaternion.identity, transform);
            _invBoxes.Add(pref);
        }

    }
    private void Start()
    {
        _oneB.onClick.AddListener(() => manager.InvPageChange(false));
        _twoB.onClick.AddListener(() => manager.InvPageChange(true));
        if (pageNum == 0)
            manager.InvChange(_oneB, _twoB, _tex);
    }
}