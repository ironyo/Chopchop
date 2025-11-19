using System;
using System.Collections;
using Member.CHJ._02.Scripts;
using Member.CHJ._02.Scripts.SO;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Minion : MonoBehaviour
{
    [SerializeField] private int firstWork;
    [SerializeField] private int patrol;
    [SerializeField] private int secondWork;
    [SerializeField] private int sleep;
    [SerializeField] private bool isCanMate;

    [SerializeField] private GameObject _particleSystem;

    public BehaviorGraphAgent behaviorGraph {get; private set;}

    [field: SerializeField]public GameObject visualObj { get; private set;}

    public MinionStats Stats;
    
    public AiStates currentState;

    public bool isFoundPartner;
    
    private NavMeshAgent _navMesh;

    public bool isMating;

    public MinionTime TimeStruct;

    private bool _isStudent = false;
    
    private JobDataSO _jobData;

    private void Awake()
    {
        Stats = new MinionStats();
        TimeStruct = new MinionTime();
        behaviorGraph = GetComponent<BehaviorGraphAgent>();
        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.updateUpAxis = false;
        _navMesh.updateRotation = false;
        currentState = AiStates.None;
        SetState(currentState);
    }

    private void Start()
    {
        InitializeDay();
        MinionManager.Instance.RegisterMinion(this);
        TimeManager.Instance.OnDayStarted += InitializeDay;
    }

    private void InitializeDay()
    {
        firstWork = Random.Range(10, 16);
        patrol = Random.Range(10, 20);
        secondWork = 55 - patrol - firstWork;
        sleep = 60;

        patrol += firstWork;
        secondWork += patrol;
        Stats.Age++;
        TimeStruct.SetTime(firstWork,patrol,secondWork,sleep);
        AgeCheck();
    }

    private void AgeCheck()
    {
        //학교 확인 후 -> 학교다닌 날 증가
        SchoolCheck();
        if(_isStudent)
            Stats.SchoolDay++;
        //등교일 수 10일 이상이면 직업 얻음
        if (Stats.SchoolDay == 10)
        {
            Debug.Log("Stats.SchoolDay >= 10" + Stats.SchoolDay);
            GetJob();
        }
        //자연사
        if (Stats.Age == Stats.MaxAge)
        {
            GetComponent<TestMinion>().Die("너무 늙었어");
        }
    }
    private void SchoolCheck()
    {
        if(MinionManager.Instance.MinionsBuildingManager.IsBuilding(MinionManager.Instance.schoolSo))
            _isStudent = true;
        else
            _isStudent = false;
    }
// 학교 다닌 일수로 계산
    private void GetJob()
    {
        _isStudent = false;
        
        //직업 중복 제거
        do
        {
            _jobData = JobManager.Instance.jobDataListSo.list
                [Random.Range(0, JobManager.Instance.jobDataListSo.list.Count)];
        } 
        while (_jobData == GetComponent<WorkActionScr>().jobData);
        GetComponent<WorkActionScr>().ChangeJob(_jobData);
    }

    public void SetState(AiStates newState)
    {
        Debug.Log($"{newState} 로 Set State");
        currentState = newState;
        behaviorGraph.BlackboardReference.SetVariableValue("AiStates", newState);
    }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     if (JobButtonManager.Instance.Minion != this)
    //     {
    //         JobButtonManager.Instance.OnValueChanged?.Invoke(this);
    //     }
    // }

    public GameObject GetVisualObject() => visualObj;
    private void LateUpdate()
    {
        Vector3 p = transform.position;
        p.z = 0;
        transform.position = p;
    }

    private void OnDestroy()
    {
        MinionManager.Instance.UnRegisterMinion(this);
        TimeManager.Instance.OnDayStarted -= InitializeDay;
    } 
        

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 30f);
    }
}
public struct MinionTime
{
    public void SetTime(int firstWork, int patrol, int secondWork, int sleep)
    {
        FirstWork = firstWork;
        Patrol = patrol;
        SecondWork = secondWork;
        Sleep = sleep;
        
    }
    public int FirstWork;
    public int Patrol;
    public int SecondWork;
    public int Sleep;
}
