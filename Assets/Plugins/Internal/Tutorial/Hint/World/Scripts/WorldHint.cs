using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets
{
    public class WorldHint : MonoBehaviour
    {
        [SerializeField] private LookAtConstraint lookAtConstraint;
        private void Start()
        {
            lookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });
        }
    }
}