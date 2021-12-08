using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public static class UnityEventUtility
    {
        public static void AddListenerOnce(this UnityEvent unityEvent, UnityAction call)
        {
            unityEvent.RemoveListener(call);
            unityEvent.AddListener(call);
        }
        public static void AddListenerOnce<T>(this UnityEvent<T> unityEvent, UnityAction<T> call)
        {
            unityEvent.RemoveListener(call);
            unityEvent.AddListener(call);
        }
    }
}