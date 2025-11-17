using System;
using UnityEngine;

public class UIBobbing : MonoBehaviour
{
    public float amplitude = 2f;
    public float frequency = 1f;

    private RectTransform rect;
    private Vector2 startPos;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * Mathf.PI * 2f * frequency) * amplitude;
        rect.anchoredPosition = new Vector2(startPos.x, startPos.y + offsetY);
    }
}
