using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "SO/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [Header("�ǹ��̸�")]
    public string buildName;
    [Header("ü��")]
    public int Health = 300;
}
