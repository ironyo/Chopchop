using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLoginChecker : MonoBehaviour
{
    private const string FIRST_PLAY_KEY = "FirstPlay";

    void Start()
    {
        // 처음 접속인지 체크
        if (!PlayerPrefs.HasKey(FIRST_PLAY_KEY))
        {
            // PlayerPrefs 저장 (처음 접속 기준 기록)
            PlayerPrefs.SetInt(FIRST_PLAY_KEY, 1);
            PlayerPrefs.Save();

            // SceneChangeManager가 준비돼 있는지 체크 후 호출
            if (SceneChangeManager.Instance != null)
            {
                MoveToTutorial();
            }
            else
            {
                Debug.LogError("SceneChangeManager.Instance가 null입니다! 초기화 순서를 확인하세요.");
            }
        }
    }

    public void MoveToTutorial()
    {
        SceneChangeManager.Instance.OnSceneEnd(2);
    }
}
