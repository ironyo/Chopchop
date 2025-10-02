using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MinionChatDisplay : MonoBehaviour
{
    [SerializeField] private List<RectTransform> minions = new List<RectTransform>(); // 버블챗 표시할 미니언들
    [SerializeField] private RectTransform bubbleChatPref; // 버블챗 프리팹

    private List<RectTransform> chatRects = new List<RectTransform>(); // 생성된 버블챗들을 저장해둠

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
