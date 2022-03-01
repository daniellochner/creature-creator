using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Earthquake : MonoBehaviour
    {
        #region Fields
        [SerializeField] private StressReceiver receiver;
        [SerializeField] private Transform creatures;
        [Space]
        [SerializeField] private float stress;
        #endregion

        #region Methods
        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && !CanvasUtility.IsPointerOverUI)
            {
                receiver.InduceStress(stress);
                foreach (BoxCreature creature in creatures.GetComponentsInChildren<BoxCreature>())
                {
                    creature.ReplaceWithRagdoll();
                }
            }
        }
        #endregion
    }
}