using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets
{
    public class PostProcessManager : MonoBehaviourSingleton<PostProcessManager>
    {
        #region Fields
        [SerializeField] private PostProcessVolume globalPPV;

        private Coroutine blendToProfileRoutine;
        private PostProcessVolume tempPPV;
        #endregion

        #region Methods
        private void OnEnable()
        {
            globalPPV.enabled = true;
        }
        private void OnDisable()
        {
            globalPPV.enabled = false;
        }

        public void BlendToProfile(PostProcessProfile profile, float timeToBlend)
        {
            if (blendToProfileRoutine != null)
            {
                StopCoroutine(blendToProfileRoutine);
                Destroy(tempPPV);
            }

            blendToProfileRoutine = StartCoroutine(BlendToProfileRoutine(profile, timeToBlend));
        }
        public IEnumerator BlendToProfileRoutine(PostProcessProfile profile, float timeToBlend)
        {
            tempPPV = globalPPV.gameObject.AddComponent<PostProcessVolume>();
            tempPPV.isGlobal = true;
            tempPPV.profile = profile;
            tempPPV.weight = 0;

            while (tempPPV.weight < 1)
            {
                float deltaWeight = Time.deltaTime / timeToBlend;

                globalPPV.weight = Mathf.Clamp01(globalPPV.weight - deltaWeight);
                tempPPV.weight = Mathf.Clamp01(tempPPV.weight + deltaWeight);

                yield return null;
            }
            Destroy(globalPPV);

            globalPPV = tempPPV;
            tempPPV = null;
        }
        #endregion
    }
}