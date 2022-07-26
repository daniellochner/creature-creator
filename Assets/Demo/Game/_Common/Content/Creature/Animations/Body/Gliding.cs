using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Gliding : CreatureAnimation
    {
        [SerializeField] private float airDrag;
        private Rigidbody rigidbody;
        float prevDrag;

        public override void OnStart(Animator animator)
        {
            rigidbody = Creature.GetComponent<Rigidbody>();
        }
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            prevDrag = rigidbody.drag;
            rigidbody.drag = airDrag;
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rigidbody.drag = prevDrag;
        }
    }
}