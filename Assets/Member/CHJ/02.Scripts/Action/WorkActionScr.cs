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
            Debug.LogWarning("DoWork Failed: Target is null");
            return;
        }
        if(!(target.TryGetComponent<Building>(out Building building)))
        {
            Debug.LogWarning("DoWork Failed: Target has no Building component");
            return;
        }
        if(building.NowMinion >= building.buildingSO.maxMinion)
        {
            Debug.LogWarning($"DoWork Failed: Building is full! {building.NowMinion}/{building.buildingSO.maxMinion}");
            return;
        }

        _building = building;
        _building.MinionPlus(1);
        _target = target.GetComponent<Collider2D>();
        isWorking = true;
        Debug.Log("DoWork Succeeded: isWorking set to true"); // 성공
    }

    public bool IsCollisionWithWorkBuilding()
    {
        if (_mycollder == null || _target == null)
            return false;

        return _mycollder.IsTouching(_target);
    }
    public virtual void ExitWork()
    {
        Debug.Log("[Work] End Work");
        _building.MinionPlus(-1);
        _building = null;
        if(!transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
        isWorking = false;
        _target = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 30);
    }
}
