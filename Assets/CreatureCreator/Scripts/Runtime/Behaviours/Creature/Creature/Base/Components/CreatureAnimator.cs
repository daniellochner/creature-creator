// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimatorParams))]
    [RequireComponent(typeof(PlayerEffects))]
    [RequireComponent(typeof(CreatureVelocity))]
    [RequireComponent(typeof(CreatureGrounded))]
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureUnderwater))]
    public class CreatureAnimator : AnimatorSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private bool useEasing;
        [SerializeField] private float extensionThreshold;

        [Header("Internal References")]
        [SerializeField] private Transform rig;
        [SerializeField] private Transform tail;
        [SerializeField] private Transform limbs;

        private RigBuilder rigBuilder;
        private Coroutine moveBodyCoroutine;
        private DynamicBone tailDynamicBone;
        #endregion

        #region Properties
        public Transform Rig => rig;

        public CreatureConstructor Constructor { get; private set; }
        public PlayerEffects Effector { get; private set; }
        public AnimatorParams Params { get; private set; }
        public CreatureVelocity Velocity { get; private set; }
        public CreatureGrounded Grounded { get; private set; }
        public CreatureUnderwater Underwater { get; private set; }

        public Action OnBuild { get; set; }

        public List<LimbAnimator> Limbs { get; private set; } = new List<LimbAnimator>();
        public List<ArmAnimator> Arms { get; private set; } = new List<ArmAnimator>();
        public List<LegAnimator> Legs { get; private set; } = new List<LegAnimator>();
        public List<WingAnimator> Wings { get; private set; } = new List<WingAnimator>();
        public List<MouthAnimator> Mouths { get; private set; } = new List<MouthAnimator>();
        public List<EyeAnimator> Eyes { get; private set; } = new List<EyeAnimator>();

        public float DefaultHeight { get; private set; }

        public bool IsMovingBody
        {
            get; private set;
        }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }
        private void OnEnable()
        {
            Reinitialize();
            Restructure(true);
            Rebuild();
            Animator.enabled = true;
            Grounded.enabled = true;
            SetDamping(true);
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            SetDamping(false);
            RestoreDefaults();
            Restructure(false);
            Grounded.enabled = false;
            Animator.enabled = false;
        }

        private void Initialize()
        {
            rigBuilder = GetComponent<RigBuilder>();
            Constructor = GetComponent<CreatureConstructor>();
            Effector = GetComponent<PlayerEffects>();
            Velocity = GetComponent<CreatureVelocity>();
            Params = GetComponent<AnimatorParams>();
            Grounded = GetComponent<CreatureGrounded>();
            Underwater = GetComponent<CreatureUnderwater>();

            tailDynamicBone = tail.GetComponent<DynamicBone>();

            Rebuild();
        }

        public void Setup()
        {
            SetupConstruction();
            SetupAnimation();
        }
        public void SetupConstruction()
        {
            Constructor.OnAddBodyPartPrefab += delegate (GameObject main, GameObject flipped)
            {
                BodyPartAnimator mainBPA = main.GetComponent<BodyPartAnimator>();
                mainBPA?.Setup(this);

                BodyPartAnimator flippedBPA = flipped.GetComponent<BodyPartAnimator>();
                flippedBPA?.SetFlipped(mainBPA);
                flippedBPA?.Setup(this);
            };
            Constructor.OnBodyPartPrefabOverride += delegate (BodyPart bodyPart)
            {
                return bodyPart.GetPrefab(BodyPart.PrefabType.Animatable) ?? bodyPart.GetPrefab(BodyPart.PrefabType.Constructible);
            };
        }
        public void SetupAnimation()
        {
            tailDynamicBone.m_ReferenceObject = Player.Instance.transform;
        }

        public void RestoreDefaults()
        {
            Constructor.Body.localPosition = Vector3.up * DefaultHeight;

            foreach (LimbAnimator limb in Limbs)
            {
                if (!limb.IsFlipped)
                {
                    limb.RestoreDefaults();
                }
            }
        }
        public void Reinitialize()
        {
            DefaultHeight = Constructor.Body.localPosition.y;

            Limbs.Clear();
            Arms.Clear();
            Legs.Clear();
            List<LimbConstructor> sorted = new List<LimbConstructor>(Constructor.Limbs);
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

            Wings = new List<WingAnimator>(Constructor.Body.GetComponentsInChildren<WingAnimator>());
            Mouths = new List<MouthAnimator>(Constructor.Body.GetComponentsInChildren<MouthAnimator>());
            Eyes = new List<EyeAnimator>(Constructor.Body.GetComponentsInChildren<EyeAnimator>());

            foreach (LegAnimator leg in Legs)
            {
                leg.Reinitialize();
            }
        }
        public void Rebuild()
        {
            rigBuilder.Build();
            Animator.Rebind();
            CreatureAnimation.Initialize(Animator, this);

            OnBuild?.Invoke();
        }
        public void Restructure(bool isAnimated)
        {
            if (isAnimated)
            {
                bool isUpper = Limbs.Count > 0;
                int n = Constructor.Bones.Count, h = 0, t = n - 1;
                for (int i = n - 1; i >= 0; --i)
                {
                    if (Constructor.Bones[i].GetComponentsInChildren<LimbConstructor>().Length > 0)
                    {
                        if (isUpper)
                        {
                            h = i;
                            isUpper = false;
                        }
                        t = i;
                    }

                    if (i > 0)
                    {
                        if (isUpper)
                        {
                            Constructor.Bones[i].SetParent(Constructor.Bones[i - 1]);
                        }
                        else
                        {
                            Constructor.Bones[i - 1].SetParent(Constructor.Bones[i]);
                        }
                    }
                }
                
                int tIndex = t - 1;
                if (tIndex >= 0)
                {
                    tailDynamicBone.m_Root = Constructor.Bones[tIndex];
                }


                // Unity's DampedTransform is broken...

                //if (Limbs.Count > 0)
                //{
                //    for (int i = n - 1; i > h; --i)
                //    {
                //        Transform bone = new GameObject($"Bone.{i}").transform;
                //        bone.SetParent(head, false);
                //        bone.SetAsFirstSibling();

                //        DampedTransform damping = bone.gameObject.AddComponent<DampedTransform>();
                //        damping.data = new DampedTransformData()
                //        {
                //            constrainedObject = Constructor.Bones[i],
                //            sourceObject = Constructor.Bones[i - 1],
                //            dampPosition = 0f,
                //            dampRotation = 0f,
                //            maintainAim = true
                //        };
                //    }
                //}
                //for (int i = 0; i < t; ++i)
                //{
                //    Transform bone = new GameObject($"Bone.{i}").transform;
                //    bone.SetParent(tail, false);
                //    bone.SetAsFirstSibling();

                //    DampedTransform damping = bone.gameObject.AddComponent<DampedTransform>();
                //    damping.data = new DampedTransformData()
                //    {
                //        constrainedObject = Constructor.Bones[i],
                //        sourceObject = Constructor.Bones[i + 1],
                //        dampPosition = 0f,
                //        dampRotation = 0f,
                //        maintainAim = true
                //    };
                //}


                EasingFunction.Function function = null;
                Vector3 offset = Vector3.zero;

                if (Legs.Count == 0)
                {
                    Mesh bodyMesh = new Mesh();
                    Constructor.SkinnedMeshRenderer.BakeMesh(bodyMesh);

                    float minY = Mathf.Infinity;
                    foreach (Vector3 vertex in bodyMesh.vertices)
                    {
                        if (vertex.y < minY)
                        {
                            minY = vertex.y;
                        }
                    }

                    offset = transform.position - Constructor.Body.L2WSpace(Vector3.up * minY);
                    function = EasingFunction.EaseOutBounce;
                }
                else
                {
                    // Determine the most extended leg and record its extension percentage.
                    LegAnimator mostExtendedLeg = null;
                    float maxExtension = Mathf.NegativeInfinity;
                    foreach (LegAnimator leg in Legs)
                    {
                        if (leg.IsFlipped)
                        {
                            continue;
                        }

                        float extension = leg.Length / leg.MaxLength;
                        if (extension > maxExtension)
                        {
                            maxExtension = extension;
                            mostExtendedLeg = leg;
                        }
                    }

                    // If the most extended leg is too extended (i.e., exceeds the extension threshold), then
                    // slump body down by an offset:
                    // offset = targetHeight - currentHeight, where targetHeight is the height of the mostExtendedLeg
                    // when it has a targetLength that satisfies the extensionThreshold.
                    if (maxExtension > extensionThreshold)
                    {
                        float targetLength = extensionThreshold * mostExtendedLeg.MaxLength;

                        float a = Vector3.ProjectOnPlane(mostExtendedLeg.transform.position - mostExtendedLeg.LegConstructor.Extremity.position, transform.up).magnitude;
                        float c = targetLength;
                        float b = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2));

                        float currentHeight = transform.InverseTransformPoint(mostExtendedLeg.transform.position).y;
                        float targetHeight = b;

                        offset = Vector3.up * (targetHeight - currentHeight);
                        function = EasingFunction.EaseOutExpo;
                    }
                }

                if (offset != Vector3.zero)
                {
                    if (useEasing && function != null)
                    {
                        moveBodyCoroutine = StartCoroutine(MoveBodyRoutine(offset, 1f, function));
                    }
                    else
                    {
                        Constructor.Body.localPosition += offset;
                    }
                }
            }
            else
            {
                foreach (Transform bone in Constructor.Bones)
                {
                    bone.SetParent(Constructor.Root);
                    bone.SetAsLastSibling();
                }

                tail.DestroyChildren();
                
                if (moveBodyCoroutine != null)
                {
                    StopCoroutine(moveBodyCoroutine);
                    IsMovingBody = false;
                }
            }

            foreach (LimbAnimator limb in Limbs)
            {
                limb.Restructure(isAnimated);
            }
        }
        public void SetDamping(bool isDamped)
        {
            tailDynamicBone.enabled = false;

            tailDynamicBone.m_Exclusions.Clear();
            foreach (BodyPartAnimator bpa in Constructor.Root.GetComponentsInChildren<BodyPartAnimator>())
            {
                if (!(bpa is TailAnimator))
                {
                    tailDynamicBone.m_Exclusions.Add(bpa.transform);
                }
            }

            tailDynamicBone.SetupParticles();

            tailDynamicBone.enabled = isDamped;
        }
        
        private IEnumerator MoveBodyRoutine(Vector3 offset, float timeToMove, EasingFunction.Function easingFunction)
        {
            IsMovingBody = true;

            Vector3 pos1 = Constructor.Body.localPosition;
            Vector3 pos2 = pos1 + offset;
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
            {
                float t = 1f - easingFunction(1f, 0f, progress);
                Vector3 position = Vector3.Lerp(pos1, pos2, t);
                Constructor.Body.localPosition = position;
            },
            timeToMove);

            IsMovingBody = false;
        }
        #endregion
    }
}