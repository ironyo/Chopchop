using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinionChat : MonoBehaviour 
{
    [Header("����")]
    [SerializeField] private int maxChatCount = 4;       // ȭ�鿡 ���� �ִ� ����
    [SerializeField] private float chatDistance = 100f;  // ä�� ����
    [SerializeField] private GameObject ChatPref;
    [SerializeField] private Transform ChatParent;
    [SerializeField] private float moveDuration = 0.25f; // ���� �ö󰡴� �ð�
    [SerializeField] private float messageLifeTime = 3f; // �޽��� ���� �ð�

    public List<Transform> activeChats = new List<Transform>(); // ���� Ȱ��ȭ�� ä�� ����Ʈ
    private Queue<Transform> pool = new Queue<Transform>();     // ��� Ǯ

    private bool isFirstMessage = true;

    private Coroutine clearRoutine; // ��ü ���� �ڷ�ƾ

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < maxChatCount * 2; i++) // �����Ӱ� Ǯ ����
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

        // Ǯ���� ������Ʈ ������
        Transform newChat = pool.Dequeue();
        newChat.gameObject.SetActive(true);

        SpriteRenderer charBaseSR = newChat.transform.Find("Base").GetComponent<SpriteRenderer>();
        charBaseSR.size = new Vector2(GetBalloonWidth(text.Length), charBaseSR.size.y);

        // �ؽ�Ʈ ����
        var tmp = newChat.gameObject.transform.Find("MessageTxt").GetComponent<TextMeshPro>();
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
            newChat.localPosition = Vector3.zero;
            var animator = newChat.GetComponent<Animator>();
            if (animator) animator?.SetTrigger("ChatTrigger");
            isFirstMessage = false;
        }
        else
        {
            // ���� ä�õ��� ���� �ø���
            for (int i = 0; i < activeChats.Count; i++)
            {
                Transform rt = activeChats[i];
                rt.DOKill();
                rt.DOLocalMoveY(rt.localPosition.y + chatDistance, moveDuration).SetEase(Ease.OutCubic);
            }

            // �� �޽����� �Ʒ�(0,0)�� ����
            newChat.localPosition = Vector3.zero;
            var animator = newChat.GetComponent<Animator>();
            if (animator) animator?.SetTrigger("ChatTrigger");
        }

        // ����Ʈ�� �߰�
        activeChats.Add(newChat);

        //  �ʰ��� �޽��� ������ ���� (���� ������ ���ʺ���)
        while (activeChats.Count > maxChatCount)
        {
            Transform oldest = activeChats[0];
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
            Transform oldest = activeChats[0];
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
        return 0.5f * textLength + 1.5f * Mathf.Sqrt(textLength);
    }
}
