using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class LayoutGroupFix : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}