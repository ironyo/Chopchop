using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public bool IsGameStarted { get; private set; } = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            IsGameStarted = true;
        }
    }

    public void StartGame()
    {
        IsGameStarted = true;
    }
}
