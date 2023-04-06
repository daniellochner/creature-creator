using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureRenderer : MonoBehaviour
    {
        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        public void SetRendered(bool isRendered)
        {
            foreach (BodyPartConstructor bpc in Constructor.BodyParts)
            {
                List<Renderer> renderers = new List<Renderer>(bpc.GetComponentsInChildren<Renderer>());
                renderers.AddRange(bpc.Flipped.GetComponentsInChildren<Renderer>());
                foreach (Renderer renderer in renderers)
                {
                    renderer.enabled = isRendered;
                }
            }
            Constructor.SkinnedMeshRenderer.enabled = isRendered;
        }
        #endregion
    }
}