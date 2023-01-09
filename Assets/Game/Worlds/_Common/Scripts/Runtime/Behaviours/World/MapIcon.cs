using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MapIcon : MonoBehaviour
    {
        [SerializeField] private UnityEvent onClick;
        private static int DEFAULT_SIZE = 50;

        private void Start()
        {
            transform.localScale *= MapManager.Instance.MapCamera.orthographicSize / DEFAULT_SIZE;
        }
        public void Click()
        {
            onClick.Invoke();
        }
    }
}