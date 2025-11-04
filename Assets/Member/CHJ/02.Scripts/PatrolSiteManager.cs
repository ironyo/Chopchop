using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolSiteManager : MonoBehaviour
{
    [SerializeField] private MinionData _dataSo;
    public List<Transform> patrolSite = new List<Transform>();

    public static PatrolSiteManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != null)
            Destroy(gameObject);
        for (int i = 0; i < transform.childCount; i++)
        {
            AddList(transform.GetChild(i).transform);
        }
    }

    public void AddList(Transform site)
    {
        patrolSite.Add(site);
    }
}
