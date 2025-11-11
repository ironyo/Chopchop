using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public ResourceTypeListSO _resourceTypeListSO;
    public static ResourceManager Instance;
    public Dictionary<ResourceTypeSO, (int, ResourcePref)> resourceAmountDictionary = new Dictionary<ResourceTypeSO, (int, ResourcePref)> ();

    [SerializeField] private GameObject resourcePref;
    [SerializeField] private RectTransform prefSpawnPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        foreach (ResourceTypeSO resource in _resourceTypeListSO.list)
        {
            ResourcePref clonedPref = Instantiate(resourcePref, prefSpawnPos).GetComponent<ResourcePref>();
            clonedPref.Set(resource.StartCount, resource.Icon);
            resourceAmountDictionary.Add(resource, (0, clonedPref));

            AddResource(resource, resource.StartCount); // 처음 기본자원
        }

        AddResource(_resourceTypeListSO.list[0], 10);
        UseResource(_resourceTypeListSO.list[1], 10);
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
    }
    
    public void UseResource(ResourceTypeSO resourceType ,int amount)
    {
        if (resourceAmountDictionary[resourceType].Item1 >= amount)
        {
            var current = resourceAmountDictionary[resourceType];
            current.Item1 -= amount;
            resourceAmountDictionary[resourceType] = current; TestLog();
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