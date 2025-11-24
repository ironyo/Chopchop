    using System;
    using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NegotiationManager : MonoSingleton<NegotiationManager>
{
    public ResourceTypeListSO resourceTypeList;
    public string resourceName { get; set; }
    public int resourceAmount { get; set; }

    private ResourceTypeSO resourceType;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetResource()
    {
        int nameIndex = Random.Range(0, 2);
        
        resourceName = resourceTypeList.list[nameIndex].name;
        resourceAmount = Random.Range(30, 70);
        resourceType = resourceTypeList.list[nameIndex];
    }

    public void Negotiation()
    {
        ResourceManager.Instance.UseResource(resourceType, resourceAmount);

        if (!ResourceManager.Instance.UseResource(resourceType, resourceAmount))
        {
            DialogManager.Instance.Disagree();
        }
    }
}
