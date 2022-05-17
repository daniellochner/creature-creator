using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MouthDamager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            CreatureBase creature = other.GetComponent<CreatureBase>();
            if (creature != null)
            {
                NetworkCreature net = creature.GetComponentInParent<NetworkCreature>();
                net.TakeDamage(10);
            }
        }
    }
}