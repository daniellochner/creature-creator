// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Platform : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool hasEntered;
        [SerializeField] private Vector3 editOffset;
        #endregion

        #region Properties
        public Vector3 Position
        {
            get => transform.position + editOffset;
        }
        public Quaternion Rotation
        {
            get => transform.rotation;
        }
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
            if (player != null && !hasEntered)
            {
                EditorManager.Instance.IsEditing = true;

                player.Editor.Platform = this;
                player.Hider.Hide();

                hasEntered = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
            if (player != null && hasEntered)
            {
                EditorManager.Instance.IsEditing = false;

                player.Hider.Show();

                hasEntered = false;
            }
        }
        #endregion
    }
}