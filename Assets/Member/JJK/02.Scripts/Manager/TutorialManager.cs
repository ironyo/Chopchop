using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    
    private List<Button> _allButtons = new();
    private bool _buttonsCached = false;

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
        
        ToolManager.Instance.SetToolInven();
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
            
            OnStepStarted?.Invoke(step.message);
            
            yield return StartCoroutine(WaitForStepCondition(step));
        }

        _isRunning = false;
        OnTutorialCompleted?.Invoke();
        //SceneManager.LoadScene(2);
        //Destroy(gameObject);
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
