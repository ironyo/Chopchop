using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolSiteManager : MonoBehaviour
{
    [SerializeField] private MinionData _dataSo;

    private void Awake()
    {
        _dataSo.patrolSite.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            AddList(transform.GetChild(i).transform);
        }
    }

    public void AddList(Transform site)
    {
        _dataSo.patrolSite.Add(site);
    }
}
