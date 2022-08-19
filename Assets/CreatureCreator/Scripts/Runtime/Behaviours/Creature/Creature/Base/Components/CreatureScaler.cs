using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureScaler : MonoBehaviour
    {
        public CreatureConstructor Constructor { get; set; }

        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }
        public void Scale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }
    }
}