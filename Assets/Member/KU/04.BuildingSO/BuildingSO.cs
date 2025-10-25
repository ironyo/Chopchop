using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "SO/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [Header("레벨")]
    public int level = 1;
    [Header("건물이름")]
    public string buildName;
    [Header("체력")]
    public int Health = 300;
    [Header("건물스탯")]
    public int width;
    public int maxW;
    [Header("건물인원수")]
    public int minionCount;
    public int maxMinion;
}
