using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "SO/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [Header("�ִ뷹��")]
    public int maxLevel = 3;
    [Header("�ǹ��̸�")]
    public string buildName;
    [Header("ü��")]
    public int Health = 300;
    [Header("�ǹ�ũ��")]
    public int width;
    public int maxW;
    [Header("�ǹ��ο���")]
    public int maxMinion;
    [field:Header("�������� ����")]
    public List<int> countStet;
    [Header("�ʿ� �ڿ�")]
    public ResourceTypeCost[] resourceTypeCost;

}

[Serializable]
public class ResourceTypeCost
{
    public ResourceTypeSO resourceTypeSO;
    public int amount;
}