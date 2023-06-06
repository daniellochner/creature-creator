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

        public bool HasEntered
        {
            get => hasEntered;
            set
            {
                hasEntered = value;
                minimapIcon.enabled = !hasEntered;
            }
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

                HasEntered = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
            if (player != null && hasEntered)
            {
                EditorManager.Instance.SetEditing(false);
                
                player.Loader.ShowMeToOthers();

                HasEntered = false;
            }
        }

        public void TeleportTo()
        {
            TeleportTo(false, true);
        }
        public void TeleportTo(bool align, bool playSound)
        {
            Player.Instance.Editor.Platform.HasEntered = false;
            Player.Instance.Editor.Platform = this;
            HasEntered = true;

            Player.Instance.Mover.Teleport(Position, align ? Rotation : Player.Instance.transform.rotation, playSound);

            if (align)
            {
                Player.Instance.Camera.Root.SetPositionAndRotation(Position, Rotation);
            }
        }
        #endregion
    }
}