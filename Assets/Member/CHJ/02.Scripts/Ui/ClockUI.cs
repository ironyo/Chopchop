using System;
using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockTxt;

    public void UpdateClock(string time)
    {
        clockTxt.SetText(time);
    }
}
