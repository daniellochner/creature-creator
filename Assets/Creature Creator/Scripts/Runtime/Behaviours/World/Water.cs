// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Water : MonoBehaviour
    {
        [SerializeField] private bool allowSwimming;

        private void OnTriggerEnter(Collider other)
        {
            CreatureHealth health = other.GetComponent<CreatureHealth>();
            if (health != null && !allowSwimming)
            {
                this.InvokeAtEndOfFrame(health.Die); // Can't set IsAnimated to false in physics frame?
            }
        }
    }
}