using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [SerializeField] private List<TutorialStep> steps = new();
    [SerializeField] private GameObject tutorialBox;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject highlightUI;
    [SerializeField] private Transform canvas;
    [field:SerializeField] public BuildingSO MinionBuildSO { get; set; }

    private TutorialStep step;
    private int _currentIndex = -1;
    private bool _isRunning = false;
    private bool _externalCompleted = false;
    private float typingSpeed = 0.08f;
    
    private List<Button> _allButtons = new();
    private bool _buttonsCached = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ApplyUILock(steps[0]);
        ToolManager.Instance.SetToolInven();
        StartCoroutine(StartTutorial());
    }
    
    public IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(3f);
        
        tutorialBox.SetActive(true);
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
            highlightUI.SetActive(false);
            _currentIndex++;

            if (_currentIndex >= steps.Count)
                break;

            step = steps[_currentIndex];

            if (step.highlightTarget != null)
            {
                highlightUI.SetActive(true);
                int childCount = canvas.transform.childCount;
                step.highlightTarget.transform.SetSiblingIndex(childCount - 5);
            }
            
            _externalCompleted = false;
            ApplyUILock(step);
            
            StartCoroutine(Typing(step.message));
            yield return StartCoroutine(WaitForStepCondition(step));
            
            if (step.highlightTarget != null)
            {
                step.highlightTarget.transform.SetSiblingIndex(0);
            }
        }

        _isRunning = false;
        ReleaseAllUI();
        SceneManager.LoadScene(1);
        Destroy(gameObject);
        Destroy(EnemyManager.Instance.gameObject);
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
    
    private void CacheAllButtons()
    {
        if (_buttonsCached) return;

        // 씬 안의 모든 버튼 가져오기 (비활성 포함)
        _allButtons = FindObjectsOfType<Button>(true).ToList();
        _buttonsCached = true;
    }
    
    private void ApplyUILock(TutorialStep step)
    {
        CacheAllButtons();

        // 이 단계에서 UI 잠금 안 쓰면 → 전부 활성화
        if (!step.lockOtherUI)
        {
            foreach (var btn in _allButtons)
            {
                if (btn == null) continue;
                btn.interactable = true;
            }
            return;
        }

        // 잠금 사용하는 경우 → allowedButtons만 활성
        foreach (var btn in _allButtons)
        {
            if (btn == null) continue;

            bool allow = step.allowedButtons != null && step.allowedButtons.Contains(btn);
            btn.interactable = allow;
        }
    }
    
    private void ReleaseAllUI()
    {
        CacheAllButtons();

        foreach (var btn in _allButtons)
        {
            if (btn == null) continue;
            btn.interactable = true;
        }
    }
    
    private IEnumerator Typing(string sentence)
    {
        _text.text = null;

        for (int i = 0; i < sentence.Length; i++)
        {
            _text.text += sentence[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
