using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class ResourceUIManager : MonoSingleton<ResourceUIManager>
{
    public Dictionary<ResourceTypeSO, int> resourceTypeDic = new();

    [SerializeField] private GameObject _resourceUIPref;

    [SerializeField] private List<ResourceSs> resourceUIList = new();

    private BuildingSO buildData;


    public void ChooseButton()
    {
        buildData = BuildManager.Instance.GetBuildData();
        ResourceSpawn();
    }

    private void ResourceSpawn()
    {
        ResourceListDestroy();
        for (int i = 0; i < buildData.resourceTypeCost.Length; i++)
        {
            resourceUIList.Add(Instantiate(_resourceUIPref, transform).GetComponent<ResourceSs>());
            resourceUIList[i].ResourceSet(buildData, i);
        }
    }

    private void ResourceListDestroy()
    {
        if (resourceUIList != null)
        {
            for (int i = 0; i < resourceUIList.Count; i++)
            {
                Destroy(resourceUIList[i].gameObject);
            }
            resourceUIList.Clear();
        }
    }
}