using System;
using System.Collections;
using System.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class DialogManager : MonoSingleton<DialogManager>
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject choiceBox;
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private GameObject spaceBar;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float typingSpeed = 0.1f;
    [SerializeField] private int waitTime = 5;

    [Header("Dialog Data")]
    [SerializeField] private DialogDataSO tutorialDialogData;
    [SerializeField] private DialogDataSO invasionDialogData;
    
    private int index = 0;
    private bool isInvasion = false;
    private bool isTutorial = false;
    private bool canFight = false;

    private enum DialogState
    {
        None,
        Tutorial,
        Invasion,
        Choosing,
        Finished
    }
    
    private DialogState state = DialogState.None;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (state == DialogState.Tutorial)
                NextTutorialLine();
            else if (state == DialogState.Invasion)
                NextInvasionLine();
        }
    }
    
    private string ProcessDialog(string rawText)
    {
        NegotiationManager.Instance.SetResource();
        string result = rawText
            .Replace("{RESOURCE}", NegotiationManager.Instance.resourceName)
            .Replace("{COUNT}", NegotiationManager.Instance.resourceAmount.ToString());
        
        return result;
    }

    private string SetName(string rawText)
    {
        string ranName = RandomName.CreateIslandName();
        enemyName.text = ranName + " 섬의 왕";
        
        string result = rawText
            .Replace("{RANDOM}", ranName)
            .Replace("{NAME}", name.text);
        
        return result;
    }

    private void NextInvasionLine()
    {
        index++;
        
        if (index == invasionDialogData.explain.Length - 3)
        {
            state = DialogState.Choosing;
            StopAllCoroutines();
            choiceBox.SetActive(true);
            spaceBar.SetActive(false);
            StartCoroutine(TimeLimit());
            return;
        }
        
        if (index < invasionDialogData.explain.Length - 2)
        {
            StopAllCoroutines();
            StartCoroutine(Typing(ProcessDialog(invasionDialogData.explain[index])));
        }
        else
        {
            EndDialog();
        }
    }

    private IEnumerator TimeLimit()
    {
        string timeLimitText = invasionDialogData.explain[index].Replace("{WAIT}", waitTime.ToString());
        StartCoroutine(Typing(timeLimitText));
        yield return new WaitForSeconds(1f);
        
        for (int i = waitTime - 1; i > 0; i--)
        {
            yield return new WaitForSeconds(1f);
            
            timeLimitText = invasionDialogData.explain[index].Replace("{WAIT}", i.ToString());
            _text.text = timeLimitText;
        }

        waitTime = 5;
    }

    private void NextTutorialLine()
    {
        index++;

        if (index < tutorialDialogData.explain.Length)
        {
            StopAllCoroutines();
            StartCoroutine(Typing(tutorialDialogData.explain[index]));
        }
        else
        {
            EndDialog();
        }
    }
    
    private void EndDialog()
    {
        dialogBox.SetActive(false);
        spaceBar.SetActive(false);

        if (canFight)
            InvasionManager.Instance.Invasion();

        state = DialogState.Finished;
    }

    public void Agree()
    {
        state = DialogState.Invasion;
        StopAllCoroutines();
        StartCoroutine(Typing(invasionDialogData.explain[invasionDialogData.explain.Length - 2]));
        choiceBox.SetActive(false);
        spaceBar.SetActive(true);
        canFight = false;
        NegotiationManager.Instance.Negotiation();
    }

    public void Disagree()
    {
        state = DialogState.Invasion;
        StopAllCoroutines();
        StartCoroutine(Typing(invasionDialogData.explain[invasionDialogData.explain.Length - 1]));
        choiceBox.SetActive(false);
        spaceBar.SetActive(true);
        canFight = true;
    }

    public void InvasionDialog()
    {
        state = DialogState.Invasion;
        index = 0;
        dialogBox.SetActive(true);
        spaceBar.SetActive(true);
        choiceBox.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(Typing(SetName(invasionDialogData.explain[index])));
    }
    
    public void TutorialDialog()
    {
        state = DialogState.Tutorial;
        index = 0;
        dialogBox.SetActive(true);
        spaceBar.SetActive(true);
        choiceBox.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(Typing(tutorialDialogData.explain[index]));
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
