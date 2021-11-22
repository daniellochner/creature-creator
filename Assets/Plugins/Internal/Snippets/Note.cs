using UnityEngine;

namespace DanielLochner.Assets
{
    public class Note : MonoBehaviour
    {
        [SerializeField, TextArea(10, 100)] private string note;
    }
}