using UnityEngine;

public interface ISingleton<T> where T : MonoBehaviour
{
    void SetSingleton(T instance);
}