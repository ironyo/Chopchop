using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinionChat : MonoBehaviour // �ش� �ڵ�� GPT�� 80% �̻� �����Ͽ����ϴ�.
{
    [Header("����")]
    [SerializeField] private int maxChatCount = 4;       // ȭ�鿡 ���� �ִ� ����
    [SerializeField] private float chatDistance = 100f;  // ä�� ����
    [SerializeField] private GameObject ChatPref;
    [SerializeField] private RectTransform ChatParent;
    [SerializeField] private float moveDuration = 0.25f; // ���� �ö󰡴� �ð�
    [SerializeField] private float messageLifeTime = 3f; // �޽��� ���� �ð�

    public List<RectTransform> activeChats = new List<RectTransform>(); // ���� Ȱ��ȭ�� ä�� ����Ʈ
    private Queue<RectTransform> pool = new Queue<RectTransform>();     // ��� Ǯ

    private bool isFirstMessage = true;

    [SerializeField] private List<string> testMessages = new List<string>();

    private Coroutine clearRoutine; // ��ü ���� �ڷ�ƾ

    private void Start()
    {
        InitializePool();
        StartCoroutine(TestRoutine()); // �׽�Ʈ��
    }

    private void InitializePool()
    {
        for (int i = 0; i < maxChatCount * 2; i++) // �����Ӱ� Ǯ ����
        {
            GameObject go = Instantiate(ChatPref, ChatParent);
            RectTransform rt = go.GetComponent<RectTransform>();
            go.SetActive(false);
            pool.Enqueue(rt);
        }
    }

    private IEnumerator TestRoutine()
    {
        for (int i = 0; i < 10; i++)
        {
            AddMessage(testMessages[Random.Range(0, testMessages.Count)]);
            yield return new WaitForSeconds(Random.Range(0f, 4f));
        }
    }

    public void AddMessage(string text)
    {
        if (pool.Count == 0) return;

        // Ǯ���� ������Ʈ ������
        RectTransform newChat = pool.Dequeue();
        newChat.gameObject.SetActive(true);

        RectTransform chatBaseRect = newChat.transform.Find("Base").GetComponent<RectTransform>();
        chatBaseRect.sizeDelta = new Vector2(GetBalloonWidth(text.Length), chatBaseRect.sizeDelta.y);

        // �ؽ�Ʈ ����
        var tmp = newChat.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp) tmp.text = text;

        // Bottom�� �� �޽������� ǥ��
        var bottom = newChat.transform.Find("Bottom");
        if (bottom) bottom.gameObject.SetActive(true);

        // ���� Bottom���� ����
        foreach (var c in activeChats)
        {
            var b = c.transform.Find("Bottom");
            if (b) b.gameObject.SetActive(false);
        }

        if (isFirstMessage)
        {
            // ù �޽����� �׳� ���ڸ�(Vector3.zero)�� �ΰ� �ִϸ��̼Ǹ�
            newChat.anchoredPosition = Vector3.zero;
            var animator = newChat.GetComponent<Animator>();
            if (animator) animator.SetTrigger("ChatTrigger");
            isFirstMessage = false;
        }
        else
        {
            // ���� ä�õ��� ���� �ø���
            for (int i = 0; i < activeChats.Count; i++)
            {
                RectTransform rt = activeChats[i];
                rt.DOKill();
                rt.DOAnchorPosY(rt.anchoredPosition.y + chatDistance, moveDuration).SetEase(Ease.OutCubic);
            }

            // �� �޽����� �Ʒ�(0,0)�� ����
            newChat.anchoredPosition = Vector3.zero;
            var animator = newChat.GetComponent<Animator>();
            if (animator) animator.SetTrigger("ChatTrigger");
        }

        // ����Ʈ�� �߰�
        activeChats.Add(newChat);

        //  �ʰ��� �޽��� ������ ���� (���� ������ ���ʺ���)
        while (activeChats.Count > maxChatCount)
        {
            RectTransform oldest = activeChats[0];
            activeChats.RemoveAt(0);
            oldest.gameObject.SetActive(false);
            pool.Enqueue(oldest);
        }

        //  �� �޽��� ������ ���� Ÿ�̸� ����
        if (clearRoutine != null) StopCoroutine(clearRoutine);
        clearRoutine = StartCoroutine(ClearAfterDelay(messageLifeTime));
    }


    private IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���������� �ϳ��� ������� ����
        while (activeChats.Count > 0)
        {
            RectTransform oldest = activeChats[0];
            activeChats.RemoveAt(0);

            // �ε巴�� ������� �ϰ� �ʹٸ� DOFade ��� ����
            // oldest.GetComponent<CanvasGroup>()?.DOFade(0f, 0.3f);

            oldest.gameObject.SetActive(false);
            pool.Enqueue(oldest);

            yield return new WaitForSeconds(0.5f); //  1�� ����
        }

        isFirstMessage = true; // �ʱ�ȭ
        clearRoutine = null;
    }


    private float GetBalloonWidth(int textLength)
    {
        if (textLength <= 0) return 0f;
        return 2f * textLength + 100f * Mathf.Sqrt(textLength);
    }
}
