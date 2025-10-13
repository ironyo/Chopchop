using System;
using System.Resources;
using UnityEngine;

public class TakeResources : MonoBehaviour
{
    [SerializeField] private ResourcesType resourcesType;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //자원 획득시
        {
            ResourcesManager.Instance.GetResources(resourcesType, 10);
        }
        
        if (Input.GetKeyDown(KeyCode.F)) //자원 소비시
        {
            ResourcesManager.Instance.UseResources(resourcesType, 10);
        }
    }
}
