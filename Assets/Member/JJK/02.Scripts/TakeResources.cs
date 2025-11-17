using System;
using System.Resources;
using UnityEngine;

public class TakeResources : MonoBehaviour
{
    private ResourceTypeSO resourceTypeSO;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //자원 획득시
        {
            //ResourceManager.Instance.AddResources(resourcesType, 10);
        }
        
        if (Input.GetKeyDown(KeyCode.F)) //자원 소비시
        {
            //ResourceManager.Instance.UseResources(resourcesType, 10);
        }
    }
}
