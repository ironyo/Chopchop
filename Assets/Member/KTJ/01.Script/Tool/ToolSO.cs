using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Apple, Water, Clean
}

public abstract class ToolSO : ScriptableObject
{
    [Header("도구 레벨별 정보")]
    public string[] ToolName = new string[3];
    public Sprite[] Icon = new Sprite[3];
    public Sprite[] HighlitedIcon = new Sprite[3];
    public int[] Amount = new int[3];
    public int[] Price = new int[3];
    public string[] ToolDesc = new string[3];

    [Header("도구 정보")]
    public ItemType ItemType;
    public float defaultRadius;

    public abstract void ToolApply(TestMinion minion, int amount); // minion.EatApple(Amount) 여따가 실제로 어떤 행동을 할지 적으면 됨.
}
