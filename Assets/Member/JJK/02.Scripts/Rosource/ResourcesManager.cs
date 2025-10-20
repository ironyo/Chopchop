using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ResourcesType
{
    Wood = 0,
    Stone = 1,
    Iron = 2,
    Gold = 3,
    Diamond = 4,
    Leather = 5,
    Wheat = 6,
    Rice = 7,
    Pork = 8,
    Beef = 9,
    Chicken = 10
}

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance;
    public Dictionary<ResourcesType, int> resources = new Dictionary<ResourcesType, int>();
    
    [SerializeField] TextMeshProUGUI[] text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (ResourcesType type in Enum.GetValues(typeof(ResourcesType)))
        {
            resources[type] = 0;
        }
    }

    private void Update()
    {
        ToText();
    }

    public void GetResources(ResourcesType resource ,int amount)
    {
        resources[resource] += amount;
    }
    
    public void UseResources(ResourcesType resource ,int amount)
    {
        if (resources[resource] >= amount)
            resources[resource] -= amount;
    }

    private void ToText()
    {
        for (int i = 0; i < resources.Count; i++)
        {
            text[i].text = (ResourcesType)i + ": " + resources[(ResourcesType)i];
        }
    }
}
