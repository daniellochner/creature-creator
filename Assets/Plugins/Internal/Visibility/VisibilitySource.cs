using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class VisibilitySource : MonoBehaviourSingleton<VisibilitySource>
    {
        #region Fields
        [SerializeField] private int tps = 1;
        private Coroutine checkCoroutine;
        #endregion

        #region Properties
        public static List<VisibilityObject> Objects { get; set; } = new List<VisibilityObject>();
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
                foreach (VisibilityObject obj in Objects)
                {
                    obj.CheckVisibility(transform.position);
                }

                yield return new WaitForSeconds(1f / tps);
            }
        }
        #endregion
    }
}