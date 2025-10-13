using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public void Fight()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Negotiation();
        }
    }

    public void Negotiation()
    {
        int val = Random.Range(0, 10);
        ResourcesType _resourcesType = (ResourcesType)val;
        if (ResourcesManager.Instance.resources[_resourcesType] >= 50)
        {
            ResourcesManager.Instance.resources[_resourcesType] -= 50;
        }
    }
}
