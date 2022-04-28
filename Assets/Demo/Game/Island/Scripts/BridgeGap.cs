// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BridgeGap : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject bridge;
        [SerializeField] private float gapWidth;
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && (other as CapsuleCollider).radius * 2f < gapWidth)
            {
                StartCoroutine(DisableBridgeRoutine());
            }
        }
        private IEnumerator DisableBridgeRoutine()
        {
            bridge.SetActive(false);
            yield return new WaitForSeconds(1f);
            bridge.SetActive(true);
        }
        #endregion
    }
}