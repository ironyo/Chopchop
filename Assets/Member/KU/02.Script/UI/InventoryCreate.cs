using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class InventoryCreate : MonoBehaviour
{
    [SerializeField] private int _invCount = 5;
    [SerializeField] private GameObject _normalPref;
    [SerializeField] private GameObject _firstPref;
    [SerializeField] private GameObject _lastPref;
    [SerializeField] List<GameObject> _invBoxes = new List<GameObject>();

    [Header("ÂüÁ¶")]
    public List<GameObject> _needed = new List<GameObject>();

    private void Awake()
    {
        for(int i = 0; i < _invCount; i++)
        {
            if(i == 0)
                _invBoxes[i] = Instantiate(_firstPref);
            else if(i == _invCount - 1)
                _invBoxes[i] = Instantiate(_lastPref);
            else
                _invBoxes[i] = Instantiate(_normalPref);
        }
    }
}
