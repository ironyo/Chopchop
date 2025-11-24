using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLoginChecker : MonoBehaviour
{
    private const string FIRST_PLAY_KEY = "FirstPlay";

    void Start()
    {
        // ó�� �������� üũ
        if (!PlayerPrefs.HasKey(FIRST_PLAY_KEY))
        {
            // PlayerPrefs ���� (ó�� ���� ���� ���)
            PlayerPrefs.SetInt(FIRST_PLAY_KEY, 1);
            PlayerPrefs.Save();

            // SceneChangeManager�� �غ�� �ִ��� üũ �� ȣ��
            if (SceneChangeManager.Instance != null)
            {
                MoveToTutorial();
            }
        }
    }

    public void MoveToTutorial()
    {
        SceneChangeManager.Instance.OnSceneEnd(2);
    }
}
