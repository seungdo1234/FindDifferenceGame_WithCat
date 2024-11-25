using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum E_PoolObjectType { Default = -1, Success, Fail, SuccessEffect, CatchMatchedEffect, CatchStartEffect, CurrencyEffect }

public class PoolManager : Singleton<PoolManager>
{
    [Serializable]
    public class PoolInfo
    {
        public E_PoolObjectType type;
        public GameObject prefab;
        public int initialSize;
        public int spawnSize;
        public int maxSize;
        public Transform parentTransform;
    }

    [SerializeField] private List<PoolInfo> pools;
    private Dictionary<int, Queue<PoolObject>> poolDictionary;
    private Dictionary<int, PoolInfo> poolInfoDictionary;

    protected override void Awake()
    {
        base.Awake();
        poolDictionary = new Dictionary<int, Queue<PoolObject>>();
        poolInfoDictionary = new Dictionary<int, PoolInfo>();
    }

    private async void Start()
    {
        await InitPoolDataAsync();
    }

    private async UniTask InitPoolDataAsync()
    {

        foreach (var pool in pools)
        {
            int typeKey = (int)pool.type;
            poolInfoDictionary[typeKey] = pool;
            poolDictionary[typeKey] = new Queue<PoolObject>(pool.initialSize);
            await AddPoolObjectsAsync(typeKey, pool.initialSize);
        }
    }

    private async UniTask AddPoolObjectsAsync(int typeKey, int count)
    {
        PoolInfo poolInfo = poolInfoDictionary[typeKey];
        for (int i = 0; i < count; i++)
        {
            if (i % 10 == 0) await UniTask.Yield(); // 매 10개 생성마다 프레임 양보

            PoolObject poolObj = Instantiate(poolInfo.prefab, poolInfo.parentTransform).GetComponent<PoolObject>();
            poolObj.gameObject.SetActive(false);
            poolDictionary[typeKey].Enqueue(poolObj);
        }
    }

    public T SpawnFromPool<T>(E_PoolObjectType type) where T : PoolObject
    {
        int typeKey = (int)type;
        if (!poolDictionary.TryGetValue(typeKey, out Queue<PoolObject> objectQueue))
        {
            Debug.LogError($"Pool of type {type} doesn't exist.");
            return null;
        }

        T poolObject;
        if (objectQueue.Count == 0)
        {
            PoolInfo info = poolInfoDictionary[typeKey];
            if (objectQueue.Count < info.maxSize)
            {
                AddPoolObjectsAsync(typeKey, Math.Min(info.spawnSize, info.maxSize - objectQueue.Count)).Forget();
            }
            poolObject = Instantiate(info.prefab, info.parentTransform).GetComponent<T>();
        }
        else
        {
            poolObject = objectQueue.Dequeue() as T;
        }

        if (poolObject != null)
            poolObject.gameObject.SetActive(true);
        else
            Debug.LogError($"{type} is Null !");

        return poolObject;
    }

    public void ReturnToPool(E_PoolObjectType type, PoolObject obj)
    {
        int typeKey = (int)type;
        obj.gameObject.SetActive(false);
        poolDictionary[typeKey].Enqueue(obj);
    }

}