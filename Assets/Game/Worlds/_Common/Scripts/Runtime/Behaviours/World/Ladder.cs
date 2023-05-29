// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Ladder : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float liftSpeed;
        #endregion

        #region Methods
        private void OnTriggerStay(Collider other)
        {
            CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
            if (player != null && InputUtility.GetKey(KeybindingsManager.Data.WalkForwards))
            {
                player.Rigidbody.velocity = transform.up * liftSpeed;
            }
        }
        #endregion
    }
}