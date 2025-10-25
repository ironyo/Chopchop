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

        // 풀에서 오브젝트 꺼내기
        Transform newChat = pool.Dequeue();
        newChat.gameObject.SetActive(true);

        SpriteRenderer charBaseSR = newChat.transform.Find("Base").GetComponent<SpriteRenderer>();
        charBaseSR.size = new Vector2(GetBalloonWidth(text.Length), charBaseSR.size.y);

        // 텍스트 갱신
        var tmp = newChat.gameObject.transform.Find("MessageTxt").GetComponent<TextMeshPro>();
        if (tmp) tmp.text = text;

        // Bottom은 새 메시지에만 표시
        var bottom = newChat.transform.Find("Bottom");
        if (bottom) bottom.gameObject.SetActive(true);

        // 기존 Bottom들은 끄기
        foreach (var c in activeChats)
        {
            var b = c.transform.Find("Bottom");
            if (b) b.gameObject.SetActive(false);
        }

        if (isFirstMessage)
        {
            // 첫 메시지는 그냥 제자리(Vector3.zero)에 두고 애니메이션만
            newChat.localPosition = Vector3.zero;
            var animator = newChat.GetComponent<Animator>();
            if (animator) animator?.SetTrigger("ChatTrigger");
            isFirstMessage = false;
        }
        else
        {
            // 기존 채팅들은 위로 올리기
            for (int i = 0; i < activeChats.Count; i++)
            {
                Transform rt = activeChats[i];
                rt.DOKill();
                rt.DOLocalMoveY(rt.localPosition.y + chatDistance, moveDuration).SetEase(Ease.OutCubic);
            }

            // 새 메시지는 아래(0,0)에 등장
            newChat.localPosition = Vector3.zero;
            var animator = newChat.GetComponent<Animator>();
            if (animator) animator?.SetTrigger("ChatTrigger");
        }

        // 리스트에 추가
        activeChats.Add(newChat);

        //  초과된 메시지 있으면 제거 (제일 오래된 위쪽부터)
        while (activeChats.Count > maxChatCount)
        {
            Transform oldest = activeChats[0];
            activeChats.RemoveAt(0);
            oldest.gameObject.SetActive(false);
            pool.Enqueue(oldest);
        }

        //  새 메시지 들어오면 삭제 타이머 리셋
        if (clearRoutine != null) StopCoroutine(clearRoutine);
        clearRoutine = StartCoroutine(ClearAfterDelay(messageLifeTime));
    }


    private IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 위에서부터 하나씩 순서대로 삭제
        while (activeChats.Count > 0)
        {
            Transform oldest = activeChats[0];
            activeChats.RemoveAt(0);

            // 부드럽게 사라지게 하고 싶다면 DOFade 사용 가능
            // oldest.GetComponent<CanvasGroup>()?.DOFade(0f, 0.3f);

            oldest.gameObject.SetActive(false);
            pool.Enqueue(oldest);

            yield return new WaitForSeconds(0.5f); //  1초 간격
        }

        isFirstMessage = true; // 초기화
        clearRoutine = null;
    }


    private float GetBalloonWidth(int textLength)
    {
        if (textLength <= 0) return 0f;
        return 0.5f * textLength + 1.5f * Mathf.Sqrt(textLength);
    }
}
