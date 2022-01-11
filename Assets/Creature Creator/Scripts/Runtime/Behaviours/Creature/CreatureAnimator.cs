// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DanielLochner.Assets.CreatureCreator
{
    /// <summary>
    /// Provides useful interface for creature animations.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(KinematicVelocity))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureAnimator : MonoBehaviour
    {
        #region Fields
        [Header("Setup")]
        [SerializeField] private Transform rig;
        [SerializeField] private float extensionThreshold = 0.9f;
        [SerializeField] private float baseMovementSpeed = 0.8f;
        [SerializeField] private float baseTurnSpeed = 120;
        [SerializeField] private float contactDistance = 0.01f;

        private Animator animator;
        private RigBuilder rigBuilder;
        private Coroutine moveBodyCoroutine;
        private bool isAnimated, hasCapturedDefaults;
        #endregion

        #region Properties
        public Transform Rig => rig;

        public CreatureConstructor CreatureConstructor { get; private set; }
        public KinematicVelocity Velocity { get; private set; }

        public List<LimbAnimator> Limbs { get; private set; } = new List<LimbAnimator>();
        public List<ArmAnimator> Arms { get; private set; } = new List<ArmAnimator>();
        public List<LegAnimator> Legs { get; private set; } = new List<LegAnimator>();

        public float DefaultHeight { get; private set; } = Mathf.NegativeInfinity;

        public bool IsMovingBody
        {
            get; private set;
        }
        public bool IsGrounded
        {
            get; private set;
        }
        public bool IsAnimated
        {
            get => isAnimated;
            set
            {
                isAnimated = value;

                if (isAnimated)
                {
                    Reinitialize();
                }
                RestoreDefaults(isAnimated);
                Restructure(isAnimated);
                if (isAnimated)
                {
                    Rebuild();
                }

                animator.enabled = isAnimated; // Remove to temporarily disable creature animations.
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void FixedUpdate()
        {
            IsGrounded = Physics.Raycast(transform.position + Vector3.up * contactDistance, -transform.up, 2f * contactDistance);
            animator.SetBool("IsGrounded", IsGrounded);

            float l = Mathf.Clamp01(Vector3.ProjectOnPlane(Velocity.Linear, transform.up).magnitude / baseMovementSpeed);
            float a = Mathf.Clamp01(Mathf.Abs(Velocity.Angular.y) / baseTurnSpeed);
            animator.SetFloat("%LSpeed", l);
            animator.SetFloat("%ASpeed", a);
        }

        private void Initialize()
        {
            animator = GetComponent<Animator>();
            rigBuilder = GetComponent<RigBuilder>();
            CreatureConstructor = GetComponent<CreatureConstructor>();
            Velocity = GetComponent<KinematicVelocity>();

            Rebuild();
        }

        public void Setup()
        {
            SetupConstruction();
        }
        public void SetupConstruction()
        {
            CreatureConstructor.OnAddBodyPartPrefab += delegate (GameObject main, GameObject flipped)
            {
                BodyPartAnimator mainBPA = main.GetComponent<BodyPartAnimator>();
                mainBPA?.Setup(this);

                BodyPartAnimator flippedBPA = flipped.GetComponent<BodyPartAnimator>();
                flippedBPA?.SetFlipped(mainBPA);
                flippedBPA?.Setup(this);
            };
            CreatureConstructor.OnBodyPartPrefabOverride += delegate (BodyPart bodyPart)
            {
                return bodyPart.GetPrefab(BodyPart.PrefabType.Animatable) ?? bodyPart.GetPrefab(BodyPart.PrefabType.Constructible);
            };
            CreatureConstructor.OnConstructCreature += delegate
            {
                Reinitialize();
            };
        }

        public void RestoreDefaults(bool isAnimated)
        {
            if (isAnimated)
            {
                DefaultHeight = CreatureConstructor.Body.localPosition.y;

                if (Legs.Count == 0) // Creatures without legs should fall down to the ground.
                {
                    Mesh bodyMesh = new Mesh();
                    CreatureConstructor.SkinnedMeshRenderer.BakeMesh(bodyMesh);

                    float minY = Mathf.Infinity;
                    foreach (Vector3 vertex in bodyMesh.vertices)
                    {
                        if (vertex.y < minY)
                        {
                            minY = vertex.y;
                        }
                    }

                    Vector3 offset = Vector3.up * Mathf.Abs(minY);
                    moveBodyCoroutine = StartCoroutine(MoveBodyRoutine(offset, 1f, EasingFunction.EaseOutBounce));
                }
                //else // Creatures with legs should slump down to put weight on them.
                //{
                //    // Determine the most extended leg and record its percentage extended.
                //    // If too extended (i.e., exceeds extension threshold), slump body down by an offset:
                //    // offset = targetHeight - currentHeight, where targetHeight is the height of the mostExtendedLeg when it has a targetLength that satisfies the extensionThreshold.

                //    LegAnimator mostExtendedLeg = null;
                //    float maxLegExtension = Mathf.NegativeInfinity;
                //    foreach (LegAnimator leg in RLegs) // Unnecessary to loop through both left and right legs.
                //    {
                //        float legExtension = leg.Length / leg.MaxLength;
                //        if (legExtension > maxLegExtension)
                //        {
                //            maxLegExtension = legExtension;
                //            mostExtendedLeg = leg;
                //        }
                //    }

                //    float targetHeight = transform.InverseTransformPoint(mostExtendedLeg.transform.position).y;
                //    if (maxLegExtension > extensionThreshold)
                //    {
                //        float targetLength = extensionThreshold * mostExtendedLeg.MaxLength;

                //        float a = Vector3.ProjectOnPlane(mostExtendedLeg.transform.position - mostExtendedLeg.LegConstructor.Extremity.position, transform.up).magnitude;
                //        float c = targetLength;
                //        float b = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2));

                //        float currentHeight = targetHeight;
                //        targetHeight = b;

                //        Vector3 offset = CreatureConstructor.Body.localPosition + Vector3.up * (targetHeight - currentHeight);
                //        moveBodyCoroutine = StartCoroutine(MoveBodyRoutine(offset, 1f, EasingFunction.EaseOutExpo));
                //    }
                //}

                hasCapturedDefaults = true;
            }
            else if (hasCapturedDefaults)
            {
                if (moveBodyCoroutine != null)
                {
                    StopCoroutine(moveBodyCoroutine);
                    IsMovingBody = false;
                }
                CreatureConstructor.Body.localPosition = Vector3.up * DefaultHeight;
                hasCapturedDefaults = false;
            }

            foreach (LimbAnimator limb in Limbs)
            {
                limb.RestoreDefaults(isAnimated);
            }
        }
        public void Restructure(bool isAnimated)
        {
            if (isAnimated)
            {
                bool isUpper = Limbs.Count > 0;
                for (int i = CreatureConstructor.Bones.Count - 1; i > 0; i--)
                {
                    if (CreatureConstructor.Bones[i].GetComponentsInChildren<LegConstructor>().Length > 0)
                    {
                        isUpper = false;
                    }

                    if (isUpper)
                    {
                        CreatureConstructor.Bones[i].SetParent(CreatureConstructor.Bones[i - 1]);
                    }
                    else
                    {
                        CreatureConstructor.Bones[i - 1].SetParent(CreatureConstructor.Bones[i]);
                    }
                }
            }
            else
            {
                foreach (Transform bone in CreatureConstructor.Bones)
                {
                    bone.SetParent(CreatureConstructor.Root);
                    bone.SetAsLastSibling();
                }
            }

            foreach (LimbAnimator limb in Limbs)
            {
                limb.Restructure(isAnimated);
            }
        }
        public void Reinitialize()
        {
            Limbs.Clear();
            Arms.Clear();
            Legs.Clear();

            List<LimbConstructor> sorted = new List<LimbConstructor>(CreatureConstructor.Limbs);
            sorted.Sort((bodyPartA, bodyPartB) =>
            {
                float posA = bodyPartA.CreatureConstructor.Body.W2LSpace(bodyPartA.transform.position).z;
                float posB = bodyPartB.CreatureConstructor.Body.W2LSpace(bodyPartB.transform.position).z;

                return posB.CompareTo(posA);
            });            
            foreach (LimbConstructor constructor in sorted)
            {
                LimbAnimator animator = constructor.GetComponent<LimbAnimator>();
                Limbs.Add(animator);
                Limbs.Add(animator.FlippedLimb);
                if (animator is ArmAnimator)
                {
                    ArmAnimator arm = animator as ArmAnimator;
                    Arms.Add(arm);
                    Arms.Add(arm.FlippedLimb as ArmAnimator);
                }
                else
                if (animator is LegAnimator)
                {
                    LegAnimator leg = animator as LegAnimator;
                    Legs.Add(leg);
                    Legs.Add(leg.FlippedLeg);
                }
            }
        }
        public void Rebuild()
        {
            rigBuilder.Build();
            animator.Rebind();
            SceneLinkedSMB<CreatureAnimator>.Initialize(animator, this);

            animator.SetBool("HasArms", Arms.Count != 0);
            animator.SetBool("HasLegs", Legs.Count != 0);
        }
        
        private IEnumerator MoveBodyRoutine(Vector3 targetPosition, float timeToMove, EasingFunction.Function easingFunction)
        {
            IsMovingBody = true;

            Vector3 initialPosition = CreatureConstructor.Body.localPosition;
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
            {
                float t = 1f - easingFunction(1f, 0f, progress);
                Vector3 position = Vector3.Lerp(initialPosition, targetPosition, t);
                CreatureConstructor.Body.localPosition = position;
            }, 
            timeToMove);

            IsMovingBody = false;
        }
        #endregion
    }
}