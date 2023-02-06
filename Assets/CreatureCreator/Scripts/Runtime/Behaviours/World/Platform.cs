// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Platform : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool hasEntered;
        [SerializeField] private Vector3 editOffset;
        [SerializeField] private AudioSource teleportAS;
        [SerializeField] private MinimapIcon minimapIcon;
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
                player.Holder.DropAll();

                EditorManager.Instance.UpdateLoadableCreatures();
                EditorManager.Instance.SetEditing(true);

                player.Editor.Platform = this;
                player.Loader.HideFromOthers();
                player.SpeedUp.SlowDown();

                minimapIcon.enabled = false;

                hasEntered = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
            if (player != null && hasEntered)
            {
                EditorManager.Instance.SetEditing(false);
                
                player.Loader.ShowMeToOthers();

                minimapIcon.enabled = true;

                hasEntered = false;
            }
        }

        public void TeleportTo(bool sfx = true)
        {
            Player.Instance.Editor.Platform.hasEntered = false;
            Player.Instance.Editor.Platform.minimapIcon.enabled = true;
            Player.Instance.Editor.Platform = this;

            Player.Instance.Mover.Teleport(this);
            if (sfx)
            {
                teleportAS.Play();
            }
        }
        #endregion
    }
}