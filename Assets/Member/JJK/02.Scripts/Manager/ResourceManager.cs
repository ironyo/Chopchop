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
        }
    }

    private void TestLog()
    {
        foreach (ResourceTypeSO key in resourceAmountDictionary.Keys)
        {
            Debug.Log(key.name + ":" + resourceAmountDictionary[key]);
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
            current.Item1 += amount;
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