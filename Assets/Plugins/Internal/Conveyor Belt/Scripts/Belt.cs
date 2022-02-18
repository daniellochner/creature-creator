// Conveyor Belt
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public class Belt : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Material belt;
        [SerializeField] private float speed;
        #endregion

        #region Properties
        public float Speed => speed;
        #endregion

        #region Methods
        private void Update()
        {
            belt.mainTextureOffset += new Vector2(0, speed / transform.localScale.z * belt.mainTextureScale.y * Time.deltaTime);
        }
        private void OnDestroy()
        {
            belt.mainTextureOffset = Vector2.zero;
        }
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Conveyor Belt"))
            {
                collision.transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
        #endregion
    }
}