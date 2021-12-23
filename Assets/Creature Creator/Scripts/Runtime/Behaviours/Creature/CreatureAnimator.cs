// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

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
        #endregion

        #region Properties
        public Transform Rig => rig;
        public Vector3 Velocity => velocity;
        public Vector3 AngularVelocity => angularVelocity;

        public float DefaultHeight { get; private set; } = Mathf.NegativeInfinity;
        public Animator Animator { get; private set; }
        public RigBuilder RigBuilder { get; private set; }

        public CreatureConstructor CreatureConstructor { get; private set; }

        public LimbAnimator[] Limbs { get; set; }
        public LegAnimator[] Legs { get; set; }

        public LegAnimator[] LLegs { get; set; }
        public LegAnimator[] RLegs { get; set; }

        public bool IsAnimated
        {
            get => isAnimated;
            set
            {
                isAnimated = value;

                Limbs = GetComponentsInChildren<LimbAnimator>();
                Legs = GetComponentsInChildren<LegAnimator>();

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

                Restructure(isAnimated);
                Reposition(isAnimated);

                if (isAnimated)
                {
                    Rebuild();
                }
                Animator.enabled = isAnimated; // Comment out to temporarily disable creature animations!
            }
        }

        public bool IsTurningRight
        {
            get; private set;
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

            Quaternion deltaRotation = CreatureConstructor.Body.rotation * Quaternion.Inverse(prevRotation);
            prevRotation = CreatureConstructor.Body.rotation;
            deltaRotation.ToAngleAxis(out float angle, out var axis);
            angularVelocity = (angle / Time.fixedDeltaTime) * axis;

            if (angularVelocity.y > 0)
            {
                IsTurningRight = true;
            }
            else
            if (angularVelocity.y < 0)
            {
                IsTurningRight = false;
            }
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
            
            // Limbs
            foreach (LimbAnimator limb in Limbs)
            {
                limb.Restructure(isAnimated);
            }
        }
        public void Reposition(bool isAnimated)
        {
            if (isAnimated)
            {
                CaptureDefaults();

                if (CreatureConstructor.Legs.Count == 0) // Legless creatures should "fall" to the ground.
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

                    CreatureConstructor.Body.localPosition = Vector3.up * Mathf.Abs(minY);
                }
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
            CreatureConstructor.Body.localPosition = Vector3.up * DefaultHeight;
            CreatureConstructor.Root.localPosition = Vector3.zero;
            hasCapturedDefaults = false;
        }
        #endregion
    }
}