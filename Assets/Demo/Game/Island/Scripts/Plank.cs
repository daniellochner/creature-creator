// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Plank : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private GameObject breakFX;
        [SerializeField] private float weightThreshold;
        [SerializeField] private float fixTime;
        #endregion

        #region Methods
        private void OnCollisionEnter(Collision other)
        {
            CreatureBase creature = other.gameObject.GetComponent<CreatureBase>();
            if (creature != null && creature.Constructor.Statistics.weight > weightThreshold)
            {
                Break();
            }
        }
        
        public void Break()
        {
            BreakServerRpc();
        }
        [ServerRpc]
        private void BreakServerRpc()
        {
            StartCoroutine(BreakRoutine());
        }
        [ClientRpc]
        private void BreakClientRpc()
        {
            gameObject.SetActive(false);
            Instantiate(breakFX, transform.position, transform.rotation);
        }
        [ClientRpc]
        private void FixClientRpc()
        {
            gameObject.SetActive(true);
        }

        private IEnumerator BreakRoutine()
        {
            BreakClientRpc();
            yield return new WaitForSeconds(fixTime);
            FixClientRpc();
        }
        #endregion
    }
}