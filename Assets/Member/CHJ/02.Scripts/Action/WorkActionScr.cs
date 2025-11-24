using System;
using Member.CHJ._02.Scripts.SO;
using UnityEngine;

public class WorkActionScr : MonoBehaviour
{

    [SerializeField] public JobDataSO jobData;
    [SerializeField] private SpriteRenderer _hatRenderer;
    private Collider2D _mycollder;
    private Collider2D _target;
    private Building _building;
    public Building CurrentBuilding => _building;
    public bool isWorking { get; private set; }

    private void Awake()
    {
        _mycollder = GetComponent<Minion>().detectCollider;
    }

    public void DoWork(Building building)
    {
        if(building == null)
        {
            return;
        }
        if(!building.TryReserve())
        {
            return;
        }
        
        _building = building;
        _target = building.EnterObj.GetComponent<Collider2D>();
        isWorking = true;
    }
    public void CheckBuilding(Minion minion)
    {
        if (IsCollisionWithWorkBuilding() && minion.GetVisualObject().activeSelf)
        {
            _building.AddShowMinion();
            minion.GetVisualObject().SetActive(false);
        }
    }
    public bool IsCollisionWithWorkBuilding()
    {
        
        if (_mycollder == null || _target == null)
            return false;
        return _mycollder.IsTouching(_target);
    }
    public virtual void ExitWork()
    {
        if (!isWorking || _building == null)
            return;
        if(!transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
        _building.Release();
        isWorking = false;
        _target = null;
    }

    public virtual void CantWork()
    {
        if(!transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
        isWorking = false;
        _target = null;
    }
    public void ChangeJob(JobDataSO job)
    {
        jobData = job;
        _hatRenderer.sprite = jobData.hat;
    }
}