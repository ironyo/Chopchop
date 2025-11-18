using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RealNameTxt;

    [Header("Object Setting")]
    [SerializeField] private GameObject NameSettingObj;
    [SerializeField] private GameObject FullObj;

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI NameTxt;
    [SerializeField] private Image Background;

    //[Header("Events")]
    //[SerializeField] private UnityEvent<string> SetNameEvent;
    //[SerializeField] private UnityEvent StartGameEvent;

    private void Awake()
    {
        SceneChangeManager.Instance?.OnSceneChangeLoaded.AddListener(OnSceneChangeLoaded);
    }

    public void OnSceneChangeLoaded()
    {
        Time.timeScale = 0;
    }
    public void SetBtn()
    {
        //SetNameEvent.Invoke(NameTxt.text);
        NameSettingObj.SetActive(false);
        RealNameTxt.text = NameTxt.text;
            Time.timeScale = 1;
        Background.DOFade(0f, 3f).OnComplete(() =>
        {
            //StartGameEvent.Invoke();
            FullObj.SetActive(false);
        });
    }
}
