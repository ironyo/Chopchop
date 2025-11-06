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

    public GameObject visualObj { get; private set;}

    public MinionStats Stats;
    
    public AiStates currentState;

    public bool isFoundPartner;
    
    private NavMeshAgent _navMesh;

    private bool _isCanSchedule = true;

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
        visualObj = transform.GetChild(0).gameObject;
        currentState = AiStates.None;
        SetState(currentState);
    }

    private void Start()
    {
        InitializeDay();
        try
        {
            MinionManager.Instance.AddMinion(this);
        }
        catch(NullReferenceException)
        {
            Debug.Log(MinionManager.Instance);
        }
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
    private AiStates TimeCheck(float time)
    {
        if (time < firstWork) return AiStates.Work;
        else if (time < patrol) return AiStates.Patrol;
        else if (time < secondWork) return AiStates.Work;
        else return AiStates.Sleep;
    }

    public void SetState(AiStates newState)
    {
        Debug.Log($"{newState} 로 Set State");
        currentState = newState;
        behaviorGraph.BlackboardReference.SetVariableValue("AiStates", newState);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (JobButtonManager.Instance.Minion != this)
        {
            JobButtonManager.Instance.OnValueChanged?.Invoke(this);
        }
    }

    private void OnDestroy() => TimeManager.Instance.OnDayStarted -= InitializeDay;
    
    private void LateUpdate()
    {
        Vector3 p = transform.position;
        p.z = 0;
        transform.position = p;
    }

    #region Mate

    

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
    #endregion
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
