using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinionChat : MonoBehaviour 
{
    [Header("설정")]
    [SerializeField] private int maxChatCount = 4;       // 화면에 보일 최대 개수
    [SerializeField] private float chatDistance = 100f;  // 채팅 간격
    [SerializeField] private GameObject ChatPref;
    [SerializeField] private Transform ChatParent;
    [SerializeField] private float moveDuration = 0.25f; // 위로 올라가는 시간
    [SerializeField] private float messageLifeTime = 3f; // 메시지 유지 시간

    public List<Transform> activeChats = new List<Transform>(); // 현재 활성화된 채팅 리스트
    private Queue<Transform> pool = new Queue<Transform>();     // 대기 풀

    private bool isFirstMessage = true;

    private Coroutine clearRoutine; // 전체 삭제 코루틴

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < maxChatCount * 2; i++) // 여유롭게 풀 생성
        {
            GameObject go = Instantiate(ChatPref, ChatParent);
            Transform rt = go.GetComponent<Transform>();
            go.SetActive(false);
            pool.Enqueue(rt);
        }
    }


    public void AddMessage(string text)
    {
        if (pool.Count == 0) return;

        //  기존 채팅들은 새 메시지 추가 시점마다 목표 위치 재설정
        for (int i = 0; i < activeChats.Count; i++)
        {
            Transform rt = activeChats[i];
            rt.DOKill(); // 기존 트윈 종료
            float targetY = (activeChats.Count - i) * chatDistance; // 새 기준 위치
            rt.DOLocalMoveY(targetY, moveDuration).SetEase(Ease.OutCubic);
        }

        // 풀에서 오브젝트 꺼내기
        Transform newChat = pool.Dequeue();
        newChat.gameObject.SetActive(true);

        // 말풍선 크기 조절
        SpriteRenderer charBaseSR = newChat.transform.Find("Base").GetComponent<SpriteRenderer>();
        charBaseSR.size = new Vector2(GetBalloonWidth(text.Length), charBaseSR.size.y);

        // 텍스트 갱신
        var tmp = newChat.transform.Find("MessageTxt").GetComponent<TextMeshPro>();
        if (tmp) tmp.text = text;

        // Bottom 처리
        var bottom = newChat.transform.Find("Bottom");
        if (bottom) bottom.gameObject.SetActive(true);

        foreach (var c in activeChats)
        {
            var b = c.transform.Find("Bottom");
            if (b) b.gameObject.SetActive(false);
        }

        // 새 메시지는 항상 맨 아래(0)에서 등장
        newChat.localPosition = Vector3.zero;

        // 등장 애니메이션 트리거
        var animator = newChat.GetComponent<Animator>();
        animator?.SetTrigger("ChatTrigger");

        // 리스트에 추가
        activeChats.Add(newChat);

        // 초과된 메시지 제거
        while (activeChats.Count > maxChatCount)
        {
            Transform oldest = activeChats[0];
            activeChats.RemoveAt(0);
            oldest.gameObject.SetActive(false);
            pool.Enqueue(oldest);
        }

        // 기존 clearRoutine 리셋
        if (clearRoutine != null) StopCoroutine(clearRoutine);
        clearRoutine = StartCoroutine(ClearAfterDelay(messageLifeTime));
    }



    private IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (activeChats.Count > 0)
        {
            Transform oldest = activeChats[0];
            activeChats.RemoveAt(0);

            // oldest.GetComponent<CanvasGroup>()?.DOFade(0f, 0.3f);

            oldest.gameObject.SetActive(false);
            pool.Enqueue(oldest);

            yield return new WaitForSeconds(0.5f);
        }

        isFirstMessage = true;
        clearRoutine = null;
    }


    private float GetBalloonWidth(int textLength)
    {
        if (textLength <= 0) return 0f;
        return 0.5f * textLength + 1.5f * Mathf.Sqrt(textLength);
    }
}
