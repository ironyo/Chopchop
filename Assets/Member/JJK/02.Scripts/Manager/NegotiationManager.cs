    using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NegotiationOption
{
    public string text;
    public int cost;
    public ResourceTypeSO type;
}

public class NegotiationManager : MonoBehaviour
{
    public List<NegotiationOption> options;

    public void AcceptNegotiation(int index)
    {
        var option = options[index];
        ResourceManager.Instance.UseResource(option.type ,option.cost);
        Debug.Log("협상 성공");
    }
    
    public void CancelNegotiation()
    {
        Debug.Log("전투 시작");
        BattleManager.Instance.BattleStart();
    }
}
