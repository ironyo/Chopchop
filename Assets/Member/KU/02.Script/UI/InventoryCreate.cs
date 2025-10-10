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
    public List<InvBuild> invBoxes = new();

    public int pageNum;
    public InventoryManager manager;
    [SerializeField] Button _oneB;
    [SerializeField] Button _twoB;

    private void Awake()
    {
        for (int i = 0; i < _invCount; i++)
        {
            GameObject pref;
            if (i == 0)
            {
                pref = Instantiate(_firstPref, transform.position, Quaternion.identity, transform);
                _oneB = pref.GetComponent<Button>();
            }

            else if (i == _invCount - 1)
            {
                pref = Instantiate(_lastPref, transform.position, Quaternion.identity, transform);
                _twoB = pref.GetComponent<Button>();
            }
            else
            {
                pref = Instantiate(_normalPref, transform.position, Quaternion.identity, transform);
                invBoxes.Add(pref.GetComponent<InvBuild>());
            }
        }

    }
    private void Start()
    {
        _oneB.onClick.AddListener(() => manager.InvPageChange(false));
        _twoB.onClick.AddListener(() => manager.InvPageChange(true));
    }
}