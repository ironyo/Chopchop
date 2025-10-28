using System;
using Member.CHJ._02.Scripts.SO;
using Unity.Behavior;
using UnityEngine;

public class WorkActionScr : MonoBehaviour
{

    [SerializeField] public JobDataSO jobData;
    private Collider2D _mycollder;
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
                if(building.buildingSO == jobData.BuildingData)
                {
                    if (building.NowMinion == building.buildingSO.maxMinion)
                        GetComponent<BehaviorGraphAgent>().SetVariableValue("IsCanWorkBuilding", false);
                    
                    building.NowMinion++;
                    _target = hit;
                    Debug.Log("IAMWWORKING");
                    GetComponent<BehaviorGraphAgent>().SetVariableValue("IsCanWorkBuilding", true);
                    GetComponent<BehaviorGraphAgent>().SetVariableValue("Target", hit.transform);
                    break;
                }
            }
        }
    }

    public bool IsCollisionWithWorkBuilding()
    {
        _mycollder = GetComponent<Collider2D>();
        Debug.Log($"나 충동 {_target && _mycollder.IsTouching(_target)}");
        return _target && _mycollder.IsTouching(_target);
    }
    public virtual void ExitWork()
    {
        if(!transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log(transform.GetChild(0).gameObject.activeSelf);
        isWorking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 30);
    }
}
