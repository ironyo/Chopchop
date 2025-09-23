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
    private Dictionary<ResourcesType, int> resources = new Dictionary<ResourcesType, int>();
    
    [SerializeField] TextMeshProUGUI[] resourceText;

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
        foreach (ResourcesType type in System.Enum.GetValues(typeof(ResourcesType)))
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
        
        //resourceText.text = resource.ToString();
        //amountText.text = resources[resource].ToString();
    }
    
    public void UseResources(ResourcesType resource ,int amount)
    {
        if (resources[resource] >= amount)
            resources[resource] -= amount;
        
        //resourceText.text = resource.ToString();
        //amountText.text = resources[resource].ToString();
    }

    private void ToText()
    {
        // resourceText[0].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[1].text = "Wood :  " + resources[ResourcesType.];
        // resourceText[2].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[3].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[4].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[5].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[6].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[7].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[8].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[9].text = "Wood :  " + resources[ResourcesType.Wood];
        // resourceText[10].text = "Wood :  " + resources[ResourcesType.Wood];

        for (int i = 0; i < resources.Count; i++)
        {
            resourceText[i].text = (ResourcesType)i + ": " + resources[(ResourcesType)i];
        }
    }
}
