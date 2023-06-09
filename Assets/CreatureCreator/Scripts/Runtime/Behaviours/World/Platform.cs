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
            if (other.CompareTag("Player/Local") && !hasEntered)
            {
                Enter();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player/Local") && hasEntered)
            {
                Exit();
            }
        }

        public void Enter()
        {
            Player.Instance.Holder.DropAll();
            Player.Instance.Loader.HideFromOthers();
            Player.Instance.SpeedUp.SlowDown();

            Player.Instance.Editor.Platform = this;

            EditorManager.Instance.UpdateLoadableCreatures();
            EditorManager.Instance.SetEditing(true);

            HasEntered = true;
        }
        public void Exit()
        {
            EditorManager.Instance.SetEditing(false);

            Player.Instance.Loader.ShowMeToOthers();

            HasEntered = false;
        }

        public void TeleportTo()
        {
            if (MinigameManager.Instance.CurrentMinigame == null)
            {
                TeleportTo(false, true);
            }
            else
            {
                InformationDialog.Inform(LocalizationUtility.Localize("minigame_cannot-teleport_title"), LocalizationUtility.Localize("minigame_cannot-teleport_message"));
            }
        }
        public void TeleportTo(bool align, bool playSound)
        {
            Player.Instance.Editor.Platform.HasEntered = false;
            Player.Instance.Mover.Teleport(Position, align ? Rotation : Player.Instance.transform.rotation, playSound);
            Enter();

            if (align)
            {
                Player.Instance.Camera.Root.SetPositionAndRotation(Position, Rotation);
                Player.Instance.Camera.CameraOrbit.Reset();
            }
        }
        #endregion
    }
}