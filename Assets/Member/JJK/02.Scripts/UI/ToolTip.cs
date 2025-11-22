using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ToolTip : MonoSingleton<ToolTip>
{
    private TextMeshProUGUI _text;
    private RectTransform _background;
    private RectTransform _rectTrm;
    [SerializeField] private RectTransform _canvas;
    
    private Vector2 _offset = new Vector2(8, 8);

    protected override void Awake()
    {
        base.Awake();
        
        _rectTrm = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _background = transform.Find("Background").GetComponent<RectTransform>();
        Hide();
    }

    private void SetToolTipText(string text)
    {
        _text.SetText(text);
        _text.ForceMeshUpdate();
        
        Vector2 size = _text.GetRenderedValues(false); //텍스트의 길이
        _background.sizeDelta = size + _offset;
    }

    private void Update()
    {
        _rectTrm.anchoredPosition = Mouse.current.position.ReadValue()/ _canvas.localScale.x;
        
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            SetToolTipText("qqqq");
        }
        
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            SetToolTipText("eeeeeeeeeeeeee");
        }
    }

    public void Show(string text)
    {
        gameObject.SetActive(true);
        SetToolTipText(text);
        StartCoroutine(SetTimerCoroutine());
    }

    private IEnumerator SetTimerCoroutine()
    {
        yield return new WaitForSeconds(30f);
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
