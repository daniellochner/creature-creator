// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Plank : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool isGap;
        [SerializeField, DrawIf("isGap", true)] private float gapWidth;

        private Collider plank;
        #endregion

        #region Methods
        private void Awake()
        {
            plank = GetComponent<Collider>();
        }
        private void OnCollisionEnter(Collision other)
        {
            if (isGap && other.collider.CompareTag("Player") && (other.collider as CapsuleCollider).radius * 2f < gapWidth)
            {
                StartCoroutine(DisableRoutine());
            }
        }

        private IEnumerator DisableRoutine()
        {
            plank.enabled = false;
            yield return new WaitForSeconds(1f);
            plank.enabled = true;
        }
        #endregion
    }
}