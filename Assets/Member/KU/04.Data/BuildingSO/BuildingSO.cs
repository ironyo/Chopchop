using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "SO/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [Header("최대레벨")]
    public int maxLevel = 3;
    [Header("건물이름")]
    public string buildName;
    [Header("체력")]
    public int[] MaxHealth;
    [Header("건물크기")]
    public int width;
    public int maxW;
    [Header("건물인원수")]
    public int[] maxMinion;
    [Header("레벨별 생산력")]
    public LevelResourceTypeCost[] levelResourceType;
    public float spawnTime;
    [Header("필요 자원")]
    public ResourceTypeCost[] resourceTypeCost;

}

[Serializable]
public class ResourceTypeCost
{
    public ResourceTypeSO resourceTypeSO;
    public int amount;
}
[Serializable]
public class LevelResourceTypeCost
{
    public ResourceTypeCost[] resourceTypeSOs;
    public int level;
}