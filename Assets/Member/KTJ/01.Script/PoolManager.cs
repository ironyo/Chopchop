using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject poolObject;
        public int poolCount;
    }
    
    [SerializeField] private List<Pool> pools = new List<Pool>();
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(var pool in pools)
        {
            Queue<GameObject> objectpool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolCount; i++)
            {
                GameObject obj = Instantiate(pool.poolObject);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                objectpool.Enqueue(pool.poolObject);
            }

            poolDictionary.Add(pool.name, objectpool);
        }
    }

    public GameObject GetMinionFromPool(string poolName, Vector2 spawnPos)
    {
        if (poolDictionary.ContainsKey(poolName))
        {
            GameObject obj = poolDictionary[poolName].Dequeue();
            obj.SetActive(true);

            return obj;
        }
        else
        {
            Debug.Log(poolName + "(는)은 존재하지 않는 풀 이름입니다");
            return null;
        }
    }

    public void ReturnToPool(string poolName, GameObject obj)
    {
        if (poolDictionary.ContainsKey(poolName))
        {
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
    }
}
