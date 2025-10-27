using Member.CHJ._02.Scripts.SO;
using Unity.Behavior;
using UnityEngine;

public class WorkActionScr : MonoBehaviour
{

    [SerializeField] public JobDataSO jobData;
    private Collider2D _target;

    public bool isWorking;


    public virtual void DoWork()
    {
        isWorking = true;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 30);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Building>(out var building))
            {
                if(building.buildingSO == jobData.BuildingData && building)
                {
                    _target = hit;
                    GetComponent<BehaviorGraphAgent>().SetVariableValue("Target", hit.transform);
                    break;
                }
            }
        }
    }

    public bool IsCollisionWithWorkBuilding()
    {
        return GetComponent<Collider2D>().IsTouching(_target);
    }
    public virtual void ExitWork()
    {
        if(!transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log(transform.GetChild(0).gameObject.activeSelf);
        isWorking = false;
    }
}
