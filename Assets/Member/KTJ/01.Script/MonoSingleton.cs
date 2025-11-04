using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            // 이미 인스턴스가 있다면 그대로 반환
            if (_instance != null)
                return _instance;

            // 씬에서 찾기
            _instance = FindFirstObjectByType<T>();

            // 없으면 새 GameObject 생성
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                _instance = obj.AddComponent<T>();
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 중복 방지
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject); // 씬 이동해도 유지
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
