using UnityEngine;

[CreateAssetMenu(fileName = "ResourceTypeSO", menuName = "SO/ResourceTypeSO")]
public class ResourceTypeSO : ScriptableObject
{
    public string name;
    public Sprite Icon;
    public int StartCount; // 처음 제공하는 자원
}
