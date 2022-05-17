// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

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
    [RequireComponent(typeof(CreatureEffector))]
    [RequireComponent(typeof(Animator))]
    public class CreatureAnimator : MonoBehaviour
    {
        #region Fields
        [Header("Setup")]
        [SerializeField] private Transform rig;
        [SerializeField] private float extensionThreshold = 0.95f;
        [SerializeField] private float baseMovementSpeed = 0.8f;
        [SerializeField] private float baseTurnSpeed = 120;
        [SerializeField] private float contactDistance = 0.01f;

        private Transform head, tail, limbs;
        private RigBuilder rigBuilder;
        private Coroutine moveBodyCoroutine;
        private bool isAnimated, hasCapturedDefaults;
        #endregion

        #region Properties
        public Transform Rig => rig;

        public KinematicVelocity Velocity { get; private set; }
        public CreatureConstructor Constructor { get; private set; }
        public CreatureEffector Effector { get; private set; }
        public Animator Animator { get; private set; }

        public List<LimbAnimator> Limbs { get; private set; } = new List<LimbAnimator>();
        public List<ArmAnimator> Arms { get; private set; } = new List<ArmAnimator>();
        public List<LegAnimator> Legs { get; private set; } = new List<LegAnimator>();
        public List<WingAnimator> Wings { get; private set; } = new List<WingAnimator>();

        public float DefaultHeight { get; private set; } = Mathf.NegativeInfinity;

        public Transform InteractTarget { get; set; }
        public Transform LookTarget { get; set; }

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

                Animator.enabled = isAnimated; // Remove to temporarily disable creature animations.
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void Update()
        {
            Animator.SetBool("HasInteractTarget", InteractTarget != null);
            Animator.SetBool("HasLookTarget", LookTarget != null);
        }
        private void FixedUpdate()
        {
            IsGrounded = Physics.Raycast(transform.position + Vector3.up * contactDistance, -transform.up, 2f * contactDistance);
            Animator.SetBool("IsGrounded", IsGrounded);

            float l = Mathf.Clamp01(Vector3.ProjectOnPlane(Velocity.Linear, transform.up).magnitude / baseMovementSpeed);
            float a = Mathf.Clamp01(Mathf.Abs(Velocity.Angular.y) / baseTurnSpeed);
            Animator.SetFloat("%LSpeed", l);
            Animator.SetFloat("%ASpeed", a);
        }

        private void Initialize()
        {
            Animator = GetComponent<Animator>();
            rigBuilder = GetComponent<RigBuilder>();
            Velocity = GetComponent<KinematicVelocity>();
            Constructor = GetComponent<CreatureConstructor>();
            Effector = GetComponent<CreatureEffector>();

            Rebuild();
        }

        public void Setup()
        {
            SetupConstruction();

            head = new GameObject("Head").transform;
            head.SetParent(Rig, false);

            tail = new GameObject("Tail").transform;
            tail.SetParent(Rig, false);

            limbs = new GameObject("Limbs").transform;
            limbs.SetParent(Rig, false);
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
            Constructor.OnConstructCreature += delegate
            {
                Reinitialize();
            };
        }

        public void RestoreDefaults(bool isAnimated)
        {
            if (isAnimated)
            {
                DefaultHeight = Constructor.Body.localPosition.y;

                if (Legs.Count == 0) // Creatures without legs should fall down to the ground.
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

                    Vector3 offset = Vector3.up * Mathf.Abs(minY);
                    moveBodyCoroutine = StartCoroutine(MoveBodyRoutine(offset, 1f, EasingFunction.EaseOutBounce));
                }
                else // Creatures with legs should slump down to put weight on them.
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

                    // If the most extended leg is too extended (i.e., exceeds the extension threshold), slump body down by an offset:
                    // offset = targetHeight - currentHeight, where targetHeight is the height of the mostExtendedLeg when it has a targetLength that satisfies the extensionThreshold.
                    if (maxExtension > extensionThreshold)
                    {
                        float targetLength = extensionThreshold * mostExtendedLeg.MaxLength;

                        float a = Vector3.ProjectOnPlane(mostExtendedLeg.transform.position - mostExtendedLeg.LegConstructor.Extremity.position, transform.up).magnitude;
                        float c = targetLength;
                        float b = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2));

                        float currentHeight = transform.InverseTransformPoint(mostExtendedLeg.transform.position).y;
                        float targetHeight = b;

                        Vector3 offset = Constructor.Body.localPosition + Vector3.up * (targetHeight - currentHeight);
                        moveBodyCoroutine = StartCoroutine(MoveBodyRoutine(offset, 1f, EasingFunction.EaseOutExpo));
                    }
                }

                hasCapturedDefaults = true;
            }
            else if (hasCapturedDefaults)
            {
                if (moveBodyCoroutine != null)
                {
                    StopCoroutine(moveBodyCoroutine);
                    IsMovingBody = false;
                }
                Constructor.Body.localPosition = Vector3.up * DefaultHeight;
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

                if (Limbs.Count > 0)
                {
                    for (int i = n - 1; i > h; --i)
                    {
                        Transform bone = new GameObject($"Bone.{i}").transform;
                        bone.SetParent(head, false);
                        bone.SetAsFirstSibling();

                        //DampedTransform damping = bone.gameObject.AddComponent<DampedTransform>();
                        //damping.data = new DampedTransformData()
                        //{
                        //    constrainedObject = Constructor.Bones[i],
                        //    sourceObject = Constructor.Bones[i - 1],
                        //    dampPosition = 0f,
                        //    dampRotation = 0f,
                        //    maintainAim = true
                        //};
                    }
                }
                for (int i = 0; i < t; ++i)
                {
                    Transform bone = new GameObject($"Bone.{i}").transform;
                    bone.SetParent(tail, false);
                    bone.SetAsFirstSibling();

                    //DampedTransform damping = bone.gameObject.AddComponent<DampedTransform>();
                    //damping.data = new DampedTransformData()
                    //{
                    //    constrainedObject = Constructor.Bones[i],
                    //    sourceObject = Constructor.Bones[i + 1],
                    //    dampPosition = 0f,
                    //    dampRotation = 0.75f,
                    //    maintainAim = true
                    //};
                }
            }
            else
            {
                foreach (Transform bone in Constructor.Bones)
                {
                    bone.SetParent(Constructor.Root);
                    bone.SetAsLastSibling();
                }

                head.DestroyChildren();
                tail.DestroyChildren();
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

            Wings.Clear();
            Wings = new List<WingAnimator>(Constructor.GetComponentsInChildren<WingAnimator>());

            foreach (LegAnimator leg in Legs)
            {
                leg.Reinitialize();
            }
        }
        public void Rebuild()
        {
            rigBuilder.Build();
            Animator.Rebind();
            SceneLinkedSMB<CreatureAnimator>.Initialize(Animator, this);

            Animator.SetBool("HasArms", Arms.Count != 0);
            Animator.SetBool("HasLegs", Legs.Count != 0);
            Animator.SetBool("HasWings", Wings.Count != 0);
        }
        
        private IEnumerator MoveBodyRoutine(Vector3 targetPosition, float timeToMove, EasingFunction.Function easingFunction)
        {
            IsMovingBody = true;

            Vector3 initialPosition = Constructor.Body.localPosition;
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
            {
                float t = 1f - easingFunction(1f, 0f, progress);
                Vector3 position = Vector3.Lerp(initialPosition, targetPosition, t);
                Constructor.Body.localPosition = position;
            }, 
            timeToMove);

            IsMovingBody = false;
        }
        #endregion
    }
}