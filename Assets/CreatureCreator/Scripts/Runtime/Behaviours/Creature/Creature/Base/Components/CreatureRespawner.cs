using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
    public class CreatureRespawner : MonoBehaviour
    {
        #region Properties
        public CreatureHealth Health { get; private set; }

        private bool CanRespawn
        {
            get => !Health.IsDead && !EditorManager.Instance.IsEditing;
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
        }
        private void Update()
        {
            if (InputUtility.GetKeyDown(KeybindingsManager.Data.Respawn) && CanRespawn)
            {
                Health.TakeDamage(Health.Health);
            }
        }
        #endregion
    }
}