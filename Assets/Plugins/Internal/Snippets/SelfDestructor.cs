using UnityEngine;

namespace DanielLochner.Assets
{
    public class SelfDestructor : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float lifetime = 0.1f;

        [Header("Debug")]
        [SerializeField, ReadOnly] private float elapsedTime = 0f;
        #endregion

        #region Methods
        private void Update()
        {
            if (elapsedTime < lifetime)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                SelfDestruct();
            }
        }
        public void SelfDestruct()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}