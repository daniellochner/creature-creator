using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class VisibilityManager : MonoBehaviourSingleton<VisibilityManager>
    {
        #region Fields
        [SerializeField] private List<VisibilityObject> objects;
        [SerializeField] private float tps = 1;
        [SerializeField] private int batchSize = 100;
        private Coroutine checkCoroutine;
        #endregion

        #region Properties
        public List<VisibilityObject> Objects => objects;

        public VisibilitySource Source { get; set; }
        #endregion

        #region Methods
        private void OnEnable()
        {
            checkCoroutine = StartCoroutine(CheckRoutine());
        }
        private void OnDisable()
        {
            StopCoroutine(checkCoroutine);
        }

        private IEnumerator CheckRoutine()
        {
            while (true)
            {
                yield return new WaitUntil(() => Objects.Count > 0);

                int counter = 0;
                while (counter < Objects.Count)
                {
                    for (int i = 0; i < batchSize && counter < Objects.Count; ++i, ++counter)
                    {
                        if (Source != null && Objects[counter] != null)
                        {
                            Objects[counter].CheckVisibility(Source.transform.position);
                        }
                    }

                    yield return new WaitForSeconds(1f / tps);
                }
            }
        }
        #endregion
    }
}