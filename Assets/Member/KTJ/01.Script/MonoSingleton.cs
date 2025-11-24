using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            // 현재 씬에서 찾기
            _instance = FindFirstObjectByType<T>();

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 중복 방지 (씬 안에서만)
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        // 씬 이동 or 오브젝트 파괴 시 Instance 초기화
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
