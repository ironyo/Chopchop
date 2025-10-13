using UnityEngine;

public abstract class ToolSO : ScriptableObject
{
    public string ToolName;
    public Sprite Icon;
    public Sprite HighlitedIcon;

    public abstract void Use(GameObject target);
}
