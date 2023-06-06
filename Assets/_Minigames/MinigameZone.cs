using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigameZone : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Minigame minigame;
        [SerializeField] private float speed;
        [SerializeField] private Material boundsMat;
        [SerializeField] private MeshRenderer[] renderersUp;
        [SerializeField] private MeshRenderer[] renderersDown;
        [SerializeField] private Transform bounds;

        private Material boundsMatUp;
        private Material boundsMatDown;
        #endregion

        #region Properties
        public Transform Bounds => bounds;
        #endregion

        #region Methods
        private void Start()
        {
            boundsMatUp   = new Material(boundsMat);
            boundsMatDown = new Material(boundsMat);

            foreach (MeshRenderer renderer in renderersUp)
            {
                renderer.material = boundsMatUp;
            }
            foreach (MeshRenderer renderer in renderersDown)
            {
                renderer.material = boundsMatDown;
            }
        }
        private void Update()
        {
            Vector2 dir = Vector2.up * speed * Time.deltaTime;
            boundsMatUp.mainTextureOffset -= dir;
            boundsMatDown.mainTextureOffset += dir;
        }

        public void Expand()
        {
            minigame.ShowAndExpandBounds();
        }
        #endregion
    }
}