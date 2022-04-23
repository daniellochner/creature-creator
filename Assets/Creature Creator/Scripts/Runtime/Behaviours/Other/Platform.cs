// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Platform : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool hasEntered;
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !hasEntered)
            {
                EditorManager.Instance.IsEditing = true;
                other.GetComponent<CreatureMover>().Platform = this;

                NetworkCreature networkCreature = other.GetComponentInParent<NetworkCreature>();
                if (networkCreature != null)
                {
                    networkCreature.Hide();
                }

                hasEntered = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && hasEntered)
            {
                EditorManager.Instance.IsEditing = false;

                NetworkCreature networkCreature = other.GetComponentInParent<NetworkCreature>();
                if (networkCreature != null)
                {
                    networkCreature.ReconstructAndShow();
                }

                hasEntered = false;
            }
        }
        #endregion
    }
}