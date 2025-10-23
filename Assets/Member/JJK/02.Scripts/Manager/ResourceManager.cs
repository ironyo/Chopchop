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
    public Dictionary<ResourceTypeSO, int> resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
    
    [SerializeField] TextMeshProUGUI[] text;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        foreach (ResourceTypeSO resource in _resourceTypeListSO.list)
        {
            resourceAmountDictionary.Add(resource, 0);
        }

        TestLog();
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
        resourceAmountDictionary[resourceType] += amount;
        TestLog();
    }
    
    public void UseResource(ResourceTypeSO resourceType ,int amount)
    {
        if (resourceAmountDictionary[resourceType] >= amount)
        {
            resourceAmountDictionary[resourceType] -= amount;
            TestLog();
        }
    }

    // private void ToText()
    // {
    //     for (int i = 0; i < resourceAmountDictionary.Count; i++)
    //     {
    //         text[i].text = _resourceTypeListSO.list[i] + ": " + resourceAmountDictionary[_resourceTypeListSO.list[i]];
    //     }
    // }
}
