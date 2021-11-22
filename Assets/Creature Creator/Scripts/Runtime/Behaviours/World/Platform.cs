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
                other.GetComponent<CreatureMover>().Platform = transform;
                other.GetComponentInParent<NetworkCreature>().Hide();

                hasEntered = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && hasEntered)
            {
                EditorManager.Instance.IsEditing = false;
                other.GetComponentInParent<NetworkCreature>().ReconstructAndShow();

                hasEntered = false;
            }
        }
        #endregion
    }
}