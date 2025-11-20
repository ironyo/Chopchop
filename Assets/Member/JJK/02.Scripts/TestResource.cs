using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestResource : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            ResourceManager.Instance.AddResource(ResourceManager.Instance._resourceTypeListSO.list[0], 10);
            ResourceManager.Instance.AddResource(ResourceManager.Instance._resourceTypeListSO.list[1], 10);
            ResourceManager.Instance.AddResource(ResourceManager.Instance._resourceTypeListSO.list[2], 10);
        }
    }
}
