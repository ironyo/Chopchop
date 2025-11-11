using UnityEngine;

public enum GameEventType // 미니언 터지기, 미니언 우울증
{
    MinionBomb, MinionMope
}
public class GameEventManager : MonoSingleton<GameEventManager>
{
    public bool IsOnEvent { get; private set; } = false;
    public void RunEvent(GameEventType eventType)
    {
        if (IsOnEvent) return;

        switch (eventType)
        {
            case GameEventType.MinionBomb:
                {
                    Debug.Log("미니언 폭팔 이벤트 발동");
                    break;
                }
            case GameEventType.MinionMope:
                {
                    Debug.Log("미니언 우울증 이벤트 발동");
                    break;
                }
        }
    }
}
