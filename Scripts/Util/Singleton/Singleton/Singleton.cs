using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            
            instance = FindFirstObjectByType<T>();
            if (instance != null) return instance;
            
            GameObject singletonObject = new GameObject(typeof(T).Name);
            instance = singletonObject.AddComponent<T>();
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}