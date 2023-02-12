using PathCreation;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Minecart : NetworkBehaviour
    {
        [SerializeField] private PathCreator railway;
        [SerializeField] private float time;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                SnapTo(0f);
            }
        }
#endif

        [ContextMenu("Move")]
        public void Move()
        {

            this.InvokeOverTime(delegate (float p)
            {
                float d = EasingFunction.EaseInOutQuad(0f, 1f, p) * railway.path.length;
                SnapTo(d);
            },
            time);
        }

        public void SnapTo(float d)
        {
            transform.position = railway.path.GetPointAtDistance(d, EndOfPathInstruction.Stop);
            transform.rotation = railway.path.GetRotationAtDistance(d, EndOfPathInstruction.Stop);
        }
    }
}