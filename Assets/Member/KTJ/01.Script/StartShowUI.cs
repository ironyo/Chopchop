using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class StartShowUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> hiddenUIs = new List<GameObject>();


    private void Start()
    {
        hiddenUIs.ForEach(x => x.SetActive(false));
    }
    public void ShowHiddenUIs()
    {
        hiddenUIs.ForEach(x => x.SetActive(true));
    }
}
