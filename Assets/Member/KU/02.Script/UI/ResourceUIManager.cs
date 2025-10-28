using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ResourceUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _resourceUIPref;

    [SerializeField] private List<ResourceSs> resourceUIList = new();

    private BuildingSO buildData;

    public static ResourceUIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void ChooseButton()
    {
        buildData = BuildManager.Instance.GetBuildData();
        ResourceSpawn();
    }

    private void ResourceSpawn()
    {
        Debug.Log(resourceUIList.Count);

        ResourceListDestroy();
        for (int i = 0; i < buildData.resourceTypeSOList.Count; i++)
        {
            Debug.Log(buildData.resourceTypeSOList.Count);
            resourceUIList.Add(Instantiate(_resourceUIPref, transform).GetComponent<ResourceSs>());
            resourceUIList[i].ResourceSet(buildData);
            resourceUIList[i].count = i;
        }
    }

    private void ResourceListDestroy()
    {
        if (resourceUIList != null)
        {
            for (int i = 0; i < resourceUIList.Count; i++)
            {
                Destroy(resourceUIList[i].gameObject);
                resourceUIList.RemoveAt(i);
            }
        }
    }
}