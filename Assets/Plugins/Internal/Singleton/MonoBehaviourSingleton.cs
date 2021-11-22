using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour, ISingleton<T> where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public void SetSingleton(T instance)
    {
        if (Instance != null) Destroy(this);
        Instance = instance;
    }

    protected virtual void Awake()
    {
        SetSingleton(this as T);
    }
}