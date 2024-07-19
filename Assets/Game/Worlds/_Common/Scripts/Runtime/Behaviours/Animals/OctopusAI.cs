using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class OctopusAI : FishAI
    {
        [Serializable]
        public class Spinning : Targeting
        {
            [SerializeField] private float maxSpinDistance;
            [SerializeField] private MinMax spinDelay;
            [SerializeField] private float spinRadius;
            [SerializeField] private MinMax spinDamage;
            [SerializeField] private PlayerEffects.Sound[] spinSounds;
            private Coroutine spinningCoroutine;
            private List<CreatureBase> damagedCreatures = new List<CreatureBase>();

            public OctopusAI OctopusAI => StateMachine as OctopusAI;

            public override void Enter()
            {
                base.Enter();
                UpdateTarget();

                OctopusAI.ResetPath();
                OctopusAI.StopStartCoroutine(SpinningRoutine(), ref spinningCoroutine);
            }
            public override void UpdateLogic()
            {
                if (!OctopusAI.IsAnimationState("Spin"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * GetTargetDistance(OctopusAI.Creature, target);
                    OctopusAI.Agent.SetDestination(target.transform.position - offset);

                    HandleLookAt();

                    NavMeshPath path = new NavMeshPath();
                    OctopusAI.Agent.CalculatePath(target.transform.position, path);
                    if (path.status != NavMeshPathStatus.PathComplete)
                    {
                        OctopusAI.ChangeState("SWI");
                    }
                }
            }
            public override void Exit()
            {
                base.Exit();
                OctopusAI.TryStopCoroutine(spinningCoroutine);
            }

            private IEnumerator SpinningRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float distance = Mathf.Infinity;
                    while (distance > GetTargetDistance(OctopusAI.Creature, target, maxSpinDistance))
                    {
                        UpdateTarget();
                        distance = Vector3.Distance(target.transform.position, OctopusAI.transform.position);
                        yield return null;
                    }

                    // Spin!
                    damagedCreatures.Clear();

                    OctopusAI.Animator.GetBehaviour<Spin>().OnSpinArm = OnSpinArm;
                    OctopusAI.Animator.GetBehaviour<Spin>().OnSpin = OnSpin;

                    OctopusAI.Params.SetTrigger("Body_Spin");

                    // Wait...
                    yield return new WaitForSeconds(spinDelay.Random);
                }
            }

            private void OnSpinArm(ArmAnimator arm)
            {
                Collider[] colliders = Physics.OverlapSphere(arm.LimbConstructor.Extremity.position, spinRadius);
                foreach (Collider collider in colliders)
                {
                    CreatureBase creature = collider.GetComponent<CreatureBase>();
                    if (creature != null && creature != OctopusAI.Creature && !damagedCreatures.Contains(creature))
                    {
                        creature.Health.TakeDamage(spinDamage.Random, DamageReason.Spin);
                        damagedCreatures.Add(creature);
                    }
                }
            }
            private void OnSpin()
            {
                OctopusAI.Creature.Effects.PlaySound(spinSounds);
            }
        }
    }
}