using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MinionSetting : MonoBehaviour
{
    [SerializeField] private int firstWork;
    [SerializeField] private int patrol;
    [SerializeField] private int secondWork;
    [SerializeField] private int sleep;
    [SerializeField] private float _currentTime;

    public BehaviorGraphAgent behaviorGraph {get; private set;}
    private AiStates _currentState;
    
    private MinionStats _stats;
    private NavMeshAgent _navMesh;
    private float _maxTime;
    private void Awake()
    {
        _stats = new MinionStats();
        behaviorGraph = GetComponent<BehaviorGraphAgent>();
        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.updateUpAxis = false;
        _navMesh.updateRotation = false;
        SetDayTime();
        
        behaviorGraph.BlackboardReference.SetVariableValue("AiStates", AiStates.Patrol);
    }

    private void SetDayTime()
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
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > 60)
        {
            _currentTime = 0;
            _stats.Age++;
            SetDayTime();
        }

        AiStates newState = Check(_currentTime);
        
        if (_currentState != newState)
        {
            SetState(newState);
        }
        Debug.Log( $"{Check(_currentTime)}, {(int)_currentTime}");
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
        _currentState = newState;
        behaviorGraph.BlackboardReference.SetVariableValue("AiStates", Check(_currentTime));
        Debug.Log("상태 변경");
    }

    public void GetJob(WorkActionScr scr )
    {
    }

    private void OnAttack(InputValue value)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;

        _navMesh.SetDestination(worldPos);
    }
    
}
