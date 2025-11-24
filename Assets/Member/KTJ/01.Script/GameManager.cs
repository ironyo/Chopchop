using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public bool IsGameStarted { get; private set; } = false;

    public void StartGame()
    {
        IsGameStarted = true;
    }
}
