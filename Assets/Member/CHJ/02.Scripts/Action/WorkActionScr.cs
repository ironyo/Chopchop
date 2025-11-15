using Member.CHJ._02.Scripts.SO;
using UnityEngine;

public class WorkActionScr : MonoBehaviour
{

    [SerializeField] public JobDataSO jobData;
    private Collider2D _mycollder;
    private Collider2D _target;
    private Building _building;
    public bool isWorking { get; private set; }

    private void Awake()
    {
        _mycollder = GetComponent<Collider2D>();
    }

    public void DoWork(Transform target)
    {
        if(target == null)
        {
            return;
        }
        if(!(target.TryGetComponent<Building>(out Building building)))
        {
            return;
        }
        if(!building.TryReserve())
        {
            return;
        }
        
        _building = building;
        _target = target.GetComponent<Collider2D>();
        isWorking = true;
    }

    public bool IsCollisionWithWorkBuilding()
    {
        if (_mycollder == null || _target == null)
            return false;

        return _mycollder.IsTouching(_target);
    }
    public virtual void ExitWork()
    {
        if(!transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
        _building.Release();
        isWorking = false;
        _target = null;
    }
}