using System;
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
    
    public BehaviorGraphAgent behaviorGraph {get; private set;}

    
    public MinionStats stat;
    
    public AiStates currentState;
    private NavMeshAgent _navMesh;

    private void Awake()
    {
        stat = new MinionStats();
        behaviorGraph = GetComponent<BehaviorGraphAgent>();
        behaviorGraph.BlackboardReference.SetVariableValue("Self", gameObject);
        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.updateUpAxis = false;
        _navMesh.updateRotation = false;

        currentState = AiStates.None;
        SetState(currentState);
    }

    private void Start()
    {
        InitializeDay();
        TimeManager.Instance.OnDayStarted += InitializeDay;
    }

    private void InitializeDay()
    {
        while(true)
        {
            firstWork = Random.Range(10, 21);
            patrol = Random.Range(10, 16);
            secondWork = Random.Range(25,31);
            sleep = 55;

            if (firstWork + patrol + secondWork == 55)
            {
                break;
            }        
        }
        patrol += firstWork;
        secondWork += patrol;
        stat.Age++;
    }

    private void Update()
    {
        AiStates newState = Check(TimeManager.Instance.currentTime);
        if (currentState != newState)
        {
            SetState(newState);
        }
    }

    private AiStates Check(float time)
    {
        if (time < firstWork) return AiStates.Work;
        else if (time < patrol) return AiStates.Patrol;
        else if (time < secondWork) return AiStates.Work;
        else return AiStates.Sleep;
    }

    private void SetState(AiStates newState)
    {
        currentState = newState;
        Debug.Log(newState);
        behaviorGraph.BlackboardReference.SetVariableValue("AiStates", newState);
    }

    private void OnAttack(InputValue value)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        
        _navMesh.SetDestination(worldPos);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        if (JobButtonManager.Instance.Minion != this)
        {
            JobButtonManager.Instance.OnValueChanged?.Invoke(this);
        }
    }

    private void OnDestroy() => TimeManager.Instance.OnDayStarted -= InitializeDay;
    

    private void LateUpdate()
    {
        var p = transform.position;
        p.z = 0;
        transform.position = p;
    }
}
