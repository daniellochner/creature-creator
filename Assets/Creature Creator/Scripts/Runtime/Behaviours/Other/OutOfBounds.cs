// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class OutOfBounds : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                this.InvokeAtEndOfFrame(delegate // CAN'T SET ANIMATED TO FALSE IN PHYSICS FRAME?!?!?!
                {
                    other.GetComponent<CreatureHealth>().Die();
                });

            }
        }
    }
}