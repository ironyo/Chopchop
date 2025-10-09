using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GetJobButton : MonoBehaviour
{
    [SerializeField] private JobType _type;

    public Minion targetMinion;
    
    public void OnClick()
    {
        JobManager.Instance.AddJob(targetMinion, _type);
    }


}
