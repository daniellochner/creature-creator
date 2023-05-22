using MTAssets.SkinnedMeshCombiner;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(SkinnedMeshCombiner))]
    public class CreatureOptimizer : MonoBehaviour
    {
        #region Fields
        [SerializeField, Button("Optimize")] private bool optimize;
        #endregion

        #region Properties
        private SkinnedMeshCombiner Combiner { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Combiner = GetComponent<SkinnedMeshCombiner>();
        }
        public void Optimize()
        {
            if (SettingsManager.Data.OptimizeOtherCreatures && !gameObject.CompareTag("Player/Local"))
            {
                Combiner.CombineMeshes();
            }
        }
        #endregion
    }
}