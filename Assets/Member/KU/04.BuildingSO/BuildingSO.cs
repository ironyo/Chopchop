using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "SO/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [Header("����")]
    public int level = 1;
    [Header("�ǹ��̸�")]
    public string buildName;
    [Header("ü��")]
    public int Health = 300;
    [Header("�ǹ�����")]
    public int width;
    public int maxW;
    [Header("�ǹ��ο���")]
    public int minionCount;
    public int maxMinion;
}
