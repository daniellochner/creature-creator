// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureAnimator : MonoBehaviour
    {
        #region Fields
        [Header("Setup")]
        [SerializeField] private Transform rig;

        private bool isAnimated, hasCapturedDefaults;
        private Vector3 velocity, prevPosition, angularVelocity;
        private Quaternion prevRotation;

        private Coroutine dropDownCoroutine;
        #endregion

        #region Properties
        public Transform Rig => rig;
        public Vector3 Velocity => velocity;
        public Vector3 AngularVelocity => angularVelocity;

        public float DefaultHeight { get; private set; } = Mathf.NegativeInfinity;
        public Animator Animator { get; private set; }
        public RigBuilder RigBuilder { get; private set; }

        public CreatureConstructor CreatureConstructor { get; private set; }

        public LimbAnimator[] Limbs { get; private set; }
        public LegAnimator[] Legs { get; private set; }
        public LegAnimator[] LLegs { get; private set; }
        public LegAnimator[] RLegs { get; private set; }

        public bool IsAnimated
        {
            get => isAnimated;
            set
            {
                isAnimated = value;

                if (isAnimated)
                {
                    InitializeLimbs();
                }

                Restructure(isAnimated);
                CaptureAndRestoreDefaults(isAnimated);
                Reposition(isAnimated);

                if (isAnimated)
                {
                    Rebuild();
                }
                Animator.enabled = isAnimated; // Comment out to temporarily disable creature animations!
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
            Vector3 deltaPosition = transform.position - prevPosition;
            velocity = deltaPosition / Time.fixedDeltaTime;
            prevPosition = transform.position;
            Animator.SetFloat("Speed", velocity.magnitude);

            Quaternion deltaRotation = CreatureConstructor.Body.localRotation * Quaternion.Inverse(prevRotation);
            deltaRotation.ToAngleAxis(out float angle, out var axis);
            angularVelocity = (angle / Time.fixedDeltaTime) * axis;
            prevRotation = CreatureConstructor.Body.localRotation;
            Animator.SetFloat("Angular Velocity", angularVelocity.y);
            Animator.SetBool("IsTurning", !Mathf.Approximately(angularVelocity.y, 0f));
        }

        private void Initialize()
        {
            Animator = GetComponent<Animator>();
            RigBuilder = GetComponent<RigBuilder>();
            CreatureConstructor = GetComponent<CreatureConstructor>();

            Rebuild();
        }

        public void Setup()
        {
            SetupConstruction();
        }
        private void SetupConstruction()
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
                InitializeLimbs();
            };
        }

        public void Restructure(bool isAnimated)
        {
            if (isAnimated)
            {
                bool isUpper = Limbs.Length > 0;
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
        public void CaptureAndRestoreDefaults(bool isAnimated)
        {
            if (isAnimated)
            {
                CaptureDefaults();
            }
            else if (hasCapturedDefaults)
            {
                RestoreDefaults();
            }

            foreach (LimbAnimator limb in Limbs)
            {
                limb.Reposition(isAnimated);
            }
        }
        public void Reposition(bool isAnimated)
        {
            if (isAnimated)
            {
                if (Legs.Length > 0) // Creatures with legs should slump down (to allow for slack while walking)
                {
                    LegAnimator mostExtendedLeg = null;
                    float maxPExtended = Mathf.NegativeInfinity;
                    foreach (LegAnimator leg in RLegs) // Unnecessary to loop through both left and right legs
                    {
                        float length = Vector3.Distance(leg.transform.position, leg.LegConstructor.Extremity.position);
                        float pExtended = length / leg.Length;

                        if (pExtended > maxPExtended)
                        {
                            maxPExtended = pExtended;
                            mostExtendedLeg = leg;
                        }
                    }

                    if (maxPExtended > 0.9f)
                    {
                        float targetLength = 0.9f * mostExtendedLeg.Length;

                        float a = CreatureConstructor.ToBodySpace(mostExtendedLeg.LegConstructor.Extremity.position).x;
                        float c = targetLength;
                        float b = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2)); // Pythagorean theorem

                        float currentHeight = CreatureConstructor.transform.InverseTransformPoint(mostExtendedLeg.transform.position).y;
                        float o = currentHeight - b;

                        Vector3 offset = CreatureConstructor.Body.localPosition - Vector3.up * o;
                        dropDownCoroutine = StartCoroutine(DropDownRoutine(offset, EasingFunction.EaseOutExpo, 1f));
                    }
                }
                else // Creatures without legs should fall down to the ground
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
                    dropDownCoroutine = StartCoroutine(DropDownRoutine(offset, EasingFunction.EaseOutBounce, 1f));
                }
            }
        }
        public void Rebuild()
        {
            RigBuilder.Build();
            Animator.Rebind();

            SceneLinkedSMB<CreatureAnimator>.Initialize(Animator, this);
        }

        private void CaptureDefaults()
        {
            DefaultHeight = CreatureConstructor.Body.localPosition.y;
            hasCapturedDefaults = true;
        }
        private void RestoreDefaults()
        {
            if (dropDownCoroutine != null)
            {
                StopCoroutine(dropDownCoroutine);
            }
            CreatureConstructor.Body.localPosition = Vector3.up * DefaultHeight;
            CreatureConstructor.Root.localPosition = Vector3.zero;
            hasCapturedDefaults = false;
        }
        private void InitializeLimbs()
        {
            Limbs = GetComponentsInChildren<LimbAnimator>();

            List<LegAnimator> legs = new List<LegAnimator>(GetComponentsInChildren<LegAnimator>());
            legs.Sort((legA, legB) =>
            {
                float posA = legA.LimbConstructor.CreatureConstructor.ToBodySpace(legA.transform.position).z;
                float posB = legB.LimbConstructor.CreatureConstructor.ToBodySpace(legB.transform.position).z;

                return posB.CompareTo(posA);
            });
            Legs = legs.ToArray();
            
            LLegs = new LegAnimator[Legs.Length / 2];
            RLegs = new LegAnimator[Legs.Length / 2];
            int li = 0, ri = 0;
            foreach (LegAnimator leg in Legs)
            {
                if (leg.transform.localPosition.x < 0)
                {
                    LLegs[li++] = leg;
                }
                else
                {
                    RLegs[ri++] = leg;
                }
            }
        }

        private IEnumerator DropDownRoutine(Vector3 dropPosition, EasingFunction.Function easingFunction, float timeToMove)
        {
            Vector3 initialPosition = CreatureConstructor.Body.localPosition;

            float timeElapsed = 0f, progress = 0f;
            while (progress < 1f)
            {
                timeElapsed += Time.deltaTime;
                progress = timeElapsed / timeToMove;

                float t = easingFunction(1f, 0f, progress);
                Vector3 position = Vector3.Lerp(initialPosition, dropPosition, 1f - t);
                CreatureConstructor.Body.localPosition = position;

                yield return null;
            }
        }
        #endregion
    }
}