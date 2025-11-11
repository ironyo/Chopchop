using System;
using System.Collections;
using Member.CHJ._02.Scripts;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Minion : MonoBehaviour, IPointerClickHandler
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

    private void Awake()
    {
        Stats = new MinionStats();
        TimeStruct = new MinionTime();
        behaviorGraph = GetComponent<BehaviorGraphAgent>();
        _navMesh = GetComponent<NavMeshAgent>();
        behaviorGraph.BlackboardReference.SetVariableValue("Self", gameObject);
        _navMesh.updateUpAxis = false;
        _navMesh.updateRotation = false;
        currentState = AiStates.None;
        SetState(currentState);
    }

    private void Start()
    {
        InitializeDay();
        MinionManager.Instance.AddMinion(this);
        TimeManager.Instance.OnDayStarted += InitializeDay;
    }

    private void InitializeDay()
    {
        firstWork = Random.Range(10, 16);
        patrol = Random.Range(10, 20);
        secondWork = 55 - patrol - firstWork;
        sleep = 55;
        
        patrol += firstWork;
        secondWork += patrol;
        Stats.Age++;
        
        TimeStruct.SetTime(firstWork,patrol,secondWork,sleep);
    }
    
    public void SetState(AiStates newState)
    {
        Debug.Log($"{newState} 로 Set State");
        currentState = newState;
        behaviorGraph.BlackboardReference.SetVariableValue("AiStates", newState);
    }

    public void StartMate()
    {
        SetState(AiStates.Mate);
        isMating = true;
    }

    public void EndMate() => isMating = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (JobButtonManager.Instance.Minion != this)
        {
            JobButtonManager.Instance.OnValueChanged?.Invoke(this);
        }
    }

    public GameObject GetVisualObject()
    {
        Debug.Log(visualObj);
        return visualObj;
    }
    private void LateUpdate()
    {
        Vector3 p = transform.position;
        p.z = 0;
        transform.position = p;
    }
    private void OnDestroy() => TimeManager.Instance.OnDayStarted -= InitializeDay;
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
