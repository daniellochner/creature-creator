using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Farm
{
    public class ExpandInSandboxCinematic : TeleportCinematic
    {
        #region Fields
        [SerializeField] private Transform target;
        #endregion

        #region Methods
        public override void Show()
        {
            base.Show();
            target.position = SpawnCreature(spawnPoint, TeleportManager.dataBuffer).Body.position;
        }
        #endregion
    }
}