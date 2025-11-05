using UnityEngine;

[CreateAssetMenu(fileName = "NegotiationData", menuName = "SO/NegotiationData")]
public class NegotiationData : ScriptableObject
{
    public string text;
    public int cost;
    public ResourceTypeSO type;
}
