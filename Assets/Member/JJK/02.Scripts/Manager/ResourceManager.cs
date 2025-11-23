using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    public ResourceTypeListSO _resourceTypeListSO;
    public Dictionary<ResourceTypeSO, (int, ResourcePref)> resourceAmountDictionary = new Dictionary<ResourceTypeSO, (int, ResourcePref)> ();

    [SerializeField] private GameObject resourcePref;
    [SerializeField] private RectTransform prefSpawnPos;

    private ResourcePref _resourceCompo;
    private List<ResourcePref> _resourceList = new();

    protected override void Awake()
    {
        base.Awake();
        
        foreach (ResourceTypeSO resource in _resourceTypeListSO.list)
        {
            _resourceCompo = Instantiate(resourcePref, prefSpawnPos).GetComponent<ResourcePref>();
            _resourceCompo.Set(resource.StartCount, resource.Icon, resource);
            resourceAmountDictionary.Add(resource, (100, _resourceCompo));
            _resourceList.Add(_resourceCompo);

            AddResource(resource, resource.StartCount); // ó�� �⺻�ڿ�
        }
    }

    private void TestLog()
    {
        foreach (var key in resourceAmountDictionary)
        {
            key.Value.Item2.UpdateCount(key.Value.Item1);
        }
    }

    public void AddResource(ResourceTypeSO resourceType ,int amount)
    {
        var current = resourceAmountDictionary[resourceType];
        current.Item1 += amount;
        resourceAmountDictionary[resourceType] = current;
        TestLog();
        ResourcePrefUpdateResource(resourceType);
    }
    
    public bool UseResource(ResourceTypeSO resourceType ,int amount)
    {
        if (resourceAmountDictionary[resourceType].Item1 >= amount)
        {
            var current = resourceAmountDictionary[resourceType];
            current.Item1 -= amount;
            resourceAmountDictionary[resourceType] = current; TestLog();

            ResourcePrefUpdateResource(resourceType);
            return true;
        }
        else if (resourceAmountDictionary[resourceType].Item1 < amount)
        {
            return false;
        }

        return false;
    }

    public void ResourcePrefUpdateResource(ResourceTypeSO type)
    {
        foreach (var item in _resourceList)
        {
            item.UpdateResource(type);
        }
    }

    //private void ToText()
    //{
    //    for (int i = 0; i < resourceAmountDictionary.Count; i++)
    //    {
    //        text[i].text = _resourceTypeListSO.list[i] + ": " + resourceAmountDictionary[_resourceTypeListSO.list[i]];
    //    }
    //}
}