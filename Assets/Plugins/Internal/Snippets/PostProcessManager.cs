using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets
{
    public class PostProcessManager : MonoBehaviourSingleton<PostProcessManager>
    {
        #region Fields
        [SerializeField] private PostProcessVolume curVolume;

        private Coroutine blendToProfileRoutine;
        private PostProcessVolume tarVolume;

        private Stack<PostProcessProfile> blendStack = new Stack<PostProcessProfile>();
        #endregion

        #region Methods
        private void OnEnable()
        {
            curVolume.enabled = true;
        }
        private void OnDisable()
        {
            curVolume.enabled = false;
        }

        private void LateUpdate()
        {
            if (blendStack.Count > 0)
            {
                PostProcessProfile target = blendStack.Peek();

                if (curVolume.profile != target)
                {
                    if (blendToProfileRoutine != null)
                    {
                        StopCoroutine(blendToProfileRoutine);
                        DestroyImmediate(tarVolume);
                    }
                    blendToProfileRoutine = StartCoroutine(BlendToProfileRoutine(target, 0.25f));
                }

                blendStack.Clear();
            }
        }

        public void BlendToProfile(PostProcessProfile profile, float timeToBlend)
        {
            blendStack.Push(profile);
        }
        public IEnumerator BlendToProfileRoutine(PostProcessProfile profile, float timeToBlend)
        {
            tarVolume = gameObject.AddComponent<PostProcessVolume>();
            tarVolume.isGlobal = true;
            tarVolume.profile = profile;
            tarVolume.weight = 0f;

            yield return this.InvokeOverTime(delegate (float p)
            {
                tarVolume.weight = Mathf.Clamp01(p);
                curVolume.weight = Mathf.Clamp01(1f - p);
            },
            timeToBlend);
            curVolume.weight = 0f;
            tarVolume.weight = 1f;

            DestroyImmediate(curVolume);
            curVolume = tarVolume;
            tarVolume = null;
        }
        #endregion
    }
}