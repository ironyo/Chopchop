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
    [Header("���� ������ ����")]
    public string[] ToolName = new string[3];
    public Sprite[] HighlitedIcon = new Sprite[3];
    public int[] Amount = new int[3];
    public int[] Price = new int[3];
    public string[] ToolDesc = new string[3];

    [Header("���� ����")]
    public ItemType ItemType;
    public float defaultRadius;

    public abstract void ToolApply(TestMinion minion, int amount); // minion.EatApple(Amount) ������ ������ � �ൿ�� ���� ������ ��.
}
