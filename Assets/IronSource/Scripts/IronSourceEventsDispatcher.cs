using System;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceEventsDispatcher : MonoBehaviour
{
    private static IronSourceEventsDispatcher instance = null;

    // Queue For Events
    private static readonly Queue<Action> ironSourceExecuteOnMainThreadQueue = new Queue<Action>();

    public static void executeAction(Action action)
    {
        lock (ironSourceExecuteOnMainThreadQueue)
        {
            ironSourceExecuteOnMainThreadQueue.Enqueue(action);
        }
    }

    void Update()
    {
        // dispatch events on the main thread when the queue is bigger than 0
        while (ironSourceExecuteOnMainThreadQueue.Count > 0)
        {
            Action IronSourceDequeuedAction = null;
            lock (ironSourceExecuteOnMainThreadQueue)
            {
                try
                {
                    IronSourceDequeuedAction = ironSourceExecuteOnMainThreadQueue.Dequeue();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            if (IronSourceDequeuedAction != null)
            {
                IronSourceDequeuedAction.Invoke();
            }
        }
    }

    public void removeFromParent()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
        {
            Destroy(this);
        }
    }

    public static void initialize()
    {
        if (isCreated())
        {
            return;
        }

        // Add an invisible game object to the scene
        GameObject obj = new GameObject("IronSourceEventsDispatcher");
        obj.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(obj);
        instance = obj.AddComponent<IronSourceEventsDispatcher>();
    }

    public static bool isCreated()
    {
        return instance != null;
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnDisable()
    {
        instance = null;
    }

}
