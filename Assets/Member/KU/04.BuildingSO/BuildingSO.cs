using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "SO/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [Header("최대레벨")]
    public int maxLevel = 3;
    [Header("건물이름")]
    public string buildName;
    [Header("체력")]
    public int Health = 300;
    [Header("건물크기")]
    public int width;
    public int maxW;
    [Header("건물인원수")]
    public int maxMinion;
    [field:Header("레벨마다 증가")]
    public List<int> countStet;
}