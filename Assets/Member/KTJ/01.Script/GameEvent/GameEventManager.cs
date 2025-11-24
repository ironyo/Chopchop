using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventType // �̴Ͼ� ������, �̴Ͼ� �����
{
    MinionBomb, MinionMope
}

public class GameEventManager : MonoSingleton<GameEventManager>
{
    [SerializeField] private List<GameEventType> events = new List<GameEventType>();








    public IGameEvent currentRunEvent = null;
    public void RunEvent(GameEventType eventType)
    {
        if (currentRunEvent != null) return;

        switch (eventType)
        {
            case GameEventType.MinionBomb:
                {
                    GameObject parentEvent = CreateEventObject(eventType);
                    currentRunEvent = parentEvent.AddComponent<BombEvent>();
                    currentRunEvent.Run();

                    break;
                }
            case GameEventType.MinionMope:
                {
                    GameObject parentEvent = CreateEventObject(eventType);
                    currentRunEvent = parentEvent.AddComponent<MopeEvent>();
                    currentRunEvent.Run();

                    break;
                }
        }

        currentRunEvent.Run();
    }

    public GameObject CreateEventObject(GameEventType gameEvent)
    {
        if (gameObject.transform.childCount != 0)
        {
            Destroy(gameObject.transform.GetChild(0));
        }

        GameObject a = new GameObject(gameEvent.ToString());
        a.transform.SetParent(gameObject.transform);

        return a;
    }

    public void StopEvent()
    {
        if (currentRunEvent  == null) return;

        currentRunEvent.Stop();
        currentRunEvent=null;

        Destroy(gameObject.transform.GetChild(0).gameObject);
    }
}
