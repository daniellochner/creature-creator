using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ColourMatcher : MonoBehaviour
    {
        [SerializeField] private Graphic source;
        [SerializeField] private Graphic target;
        private void Update()
        {
            target.color = source.color;
        }
    }
}