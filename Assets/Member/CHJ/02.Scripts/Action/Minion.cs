using System;
using System.Collections;
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

    public GameObject visualObj { get; private set;}

    public MinionStats Stats;
    
    public AiStates currentState;

    public bool isFoundPartner;
    
    private NavMeshAgent _navMesh;

    private void Awake()
    {
        Stats = new MinionStats();
        behaviorGraph = GetComponent<BehaviorGraphAgent>();
        behaviorGraph.BlackboardReference.SetVariableValue("Self", gameObject);
        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.updateUpAxis = false;
        _navMesh.updateRotation = false;
        visualObj = transform.GetChild(0).gameObject;
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
            firstWork = Random.Range(10, 16);
            patrol = Random.Range(10, 20);
            secondWork = Random.Range(25,31);
            sleep = 55;

            if (firstWork + patrol + secondWork == 55)
            {
                break;
            }        
        }
        patrol += firstWork;
        secondWork += patrol;
        Stats.Age++;
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
    
    public IEnumerator Mate(Minion minion)
    {
        isFoundPartner = true;
        behaviorGraph.End();
        _particleSystem.SetActive(true);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 6);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("House"))
                _navMesh.SetDestination(hit.transform.position);
        }
        if (_navMesh.remainingDistance <= 0.01f)
        {
            Debug.Log(_navMesh.destination);
            visualObj.SetActive(false);
            yield return new WaitForSeconds(3);
            Debug.Log("뿌직응가호이짜");
        }
        
        EndMate();
    }
    
    private void EndMate()
    {
        visualObj.SetActive(true);
        _particleSystem.SetActive(false);
        behaviorGraph.Start();
        isFoundPartner = false;
    }
}
