using System;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialConditionType
{
    None,           // 바로 다음 단계
    WaitSeconds,    // 몇 초 기다렸다가
    WaitForKey,     // 키 입력을 기다렸다가
    WaitForEvent    // 외부 이벤트로 완료
}

[Serializable]
public class TutorialStep
{
    public string id;     
    [TextArea] public string message;

    public TutorialConditionType conditionType = TutorialConditionType.None;
    public float waitSeconds = 1f;
    public KeyCode waitKey = KeyCode.Space;
}
