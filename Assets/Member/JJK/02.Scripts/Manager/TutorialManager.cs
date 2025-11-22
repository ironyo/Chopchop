using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [SerializeField] private List<TutorialStep> steps = new();
    [SerializeField] private bool autoStartOnAwake = true;

    private int _currentIndex = -1;
    private bool _isRunning = false;
    private bool _externalCompleted = false;

    // UI가 구독해서 텍스트 띄워주도록 하는 이벤트
    public UnityEvent<string> OnStepStarted;
    public UnityEvent OnTutorialCompleted;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (autoStartOnAwake)
        {
            StartTutorial();
        }
    }
    
    public void StartTutorial()
    {
        if (_isRunning || steps.Count == 0) return;

        _isRunning = true;
        _currentIndex = -1;
        StartCoroutine(TutorialRoutine());
    }
    
    public void CompleteCurrentStepExternally()
    {
        _externalCompleted = true;
    }

    private IEnumerator TutorialRoutine()
    {
        while (true)
        {
            _currentIndex++;

            if (_currentIndex >= steps.Count)
                break;

            TutorialStep step = steps[_currentIndex];
            _externalCompleted = false;

            // UI에 텍스트 전달 (예: 툴팁, 튜토리얼 패널 등)
            OnStepStarted?.Invoke(step.message);

            // 조건 기다리기
            yield return StartCoroutine(WaitForStepCondition(step));
        }

        _isRunning = false;
        OnTutorialCompleted?.Invoke();
        Debug.Log("[Tutorial] 튜토리얼 종료");
    }

    private IEnumerator WaitForStepCondition(TutorialStep step)
    {
        switch (step.conditionType)
        {
            case TutorialConditionType.None:
                yield break;
            case TutorialConditionType.WaitSeconds:
                yield return new WaitForSeconds(step.waitSeconds);
                break;
            case TutorialConditionType.WaitForKey:
                while (!Input.GetKeyDown(step.waitKey))
                {
                    yield return null;
                }
                break;
            case TutorialConditionType.WaitForEvent:
                yield return new WaitUntil(() => _externalCompleted);
                break;
        }
    }
    
    public string GetCurrentStepId()
    {
        if (_currentIndex < 0 || _currentIndex >= steps.Count)
            return null;

        return steps[_currentIndex].id;
    }
}
