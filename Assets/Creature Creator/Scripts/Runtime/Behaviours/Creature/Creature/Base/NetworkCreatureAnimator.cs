using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    public class NetworkCreatureAnimator : NetworkBehaviour
    {
        public CreatureAnimator Animator { get; set; }

        private void Awake()
        {
            Animator = GetComponent<CreatureAnimator>();
        }

        private void Start()
        {
            Animator.OnBuild += delegate
            {
                CreatureAnimation[] animations = Animator.Animator.GetBehaviours<CreatureAnimation>();
                foreach (CreatureAnimation animation in animations)
                {
                    animation.PerformLogic = IsOwner;
                }
            };
        }
    }
}