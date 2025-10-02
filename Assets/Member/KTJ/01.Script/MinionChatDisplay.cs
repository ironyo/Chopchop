using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MinionChatDisplay : MonoBehaviour
{
    [SerializeField] private List<RectTransform> minions = new List<RectTransform>(); // ����ê ǥ���� �̴Ͼ��
    [SerializeField] private RectTransform bubbleChatPref; // ����ê ������

    private List<RectTransform> chatRects = new List<RectTransform>(); // ������ ����ê���� �����ص�

    private void Start()
    {
        InitializeBubbleChats();
    }

    private void InitializeBubbleChats()
    {
        for (int i = 0; i < minions.Count; i++)
        {
            RectTransform clonedChat = Instantiate(bubbleChatPref, minions[i]);
            clonedChat.gameObject.SetActive(false);
            chatRects.Add(clonedChat);
        }
    }
}
