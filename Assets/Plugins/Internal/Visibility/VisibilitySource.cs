using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class VisibilitySource : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float tps = 1;
        [SerializeField] private int batchSize = 100;
        private Coroutine checkCoroutine;
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
                yield return new WaitUntil(() => VisibilityManager.Instance.Objects.Count > 0);

                int counter = 0;
                while (counter < VisibilityManager.Instance.Objects.Count)
                {
                    for (int i = 0; i < batchSize && counter < VisibilityManager.Instance.Objects.Count; ++i, ++counter)
                    {
                        VisibilityManager.Instance.Objects[counter].CheckVisibility(transform.position);
                    }

                    yield return new WaitForSeconds(1f / tps);
                }
            }
        }
        #endregion
    }
}