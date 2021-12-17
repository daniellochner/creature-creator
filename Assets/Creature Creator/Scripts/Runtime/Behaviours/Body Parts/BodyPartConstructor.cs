// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BodyPartConstructor : MonoBehaviour, IFlippable<BodyPartConstructor>
    {
        #region Fields
        [SerializeField] private BodyPart bodyPart;

        [Header("Body Part")]
        [SerializeField] private Transform model;
        [SerializeField] private SerializableDictionaryBase<StretchAxis, StretchIndices> stretchMap = new SerializableDictionaryBase<StretchAxis, StretchIndices>();
        #endregion

        #region Properties
        public BodyPart BodyPart
        {
            get => bodyPart;
            set
            {
                bodyPart = value;
            }
        }
        public Transform Model
        {
            get => model;
            set
            {
                model = value;
            }
        }
        public SerializableDictionaryBase<StretchAxis, StretchIndices> StretchMap
        {
            get => stretchMap;
            set
            {
                stretchMap = value;
            }
        }

        public CreatureConstructor CreatureConstructor { get; private set; }
        public BodyPartConstructor Flipped { get; set; }
        public AttachedBodyPart AttachedBodyPart { get; set; }

        public Action OnStretch { get; set; }
        public Action OnAttach { get; set; }
        public Action<Vector3> OnScale { get; set; }

        public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }
        public Material BodyPartPrimaryMat { get; private set; }
        public Material BodyPartSecondaryMat { get; private set; }

        public bool IsFlipped { get; set; }

        public int NearestBone
        {
            get
            {
                int nearestBoneIndex = -1;
                float minDistance = float.PositiveInfinity;

                for (int boneIndex = 0; boneIndex < CreatureConstructor.Bones.Count; boneIndex++)
                {
                    float distance = Vector3.Distance(CreatureConstructor.Bones[boneIndex].position, transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestBoneIndex = boneIndex;
                    }
                }

                return nearestBoneIndex;
            }
        }
        public virtual bool CanMirror
        {
            get
            {
                return Mathf.Abs(CreatureConstructor.transform.InverseTransformPoint(transform.position).x) > CreatureConstructor.MergeThreshold;
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            SkinnedMeshRenderer = model.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public virtual void Setup(CreatureConstructor creatureConstructor)
        {
            CreatureConstructor = creatureConstructor;

            BodyPartPrimaryMat = IsFlipped ? Flipped.BodyPartPrimaryMat : new Material(CreatureConstructor.BodyPartMaterial);
            BodyPartSecondaryMat = IsFlipped ? Flipped.BodyPartSecondaryMat : new Material(CreatureConstructor.BodyPartMaterial);

            foreach (Renderer renderer in Model.GetComponentsInChildren<Renderer>())
            {
                Material[] materials = renderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    Material material = materials[i];
                    if (material.name.Contains("Body_Primary"))
                    {
                        materials[i] = CreatureConstructor.BodyPrimaryMat;
                    }
                    else 
                    if (material.name.Contains("Body_Secondary"))
                    {
                        materials[i] = CreatureConstructor.BodySecondaryMat;
                    }
                    else 
                    if (material.name.Contains("BodyPart_Primary"))
                    {
                        materials[i] = BodyPartPrimaryMat;
                    }
                    else 
                    if (material.name.Contains("BodyPart_Secondary"))
                    {
                        materials[i] = BodyPartSecondaryMat;
                    }
                }

                renderer.materials = materials;
            }
        }

        public virtual void Add()
        {
            CreatureConstructor.BodyParts.Add(this);
        }
        public virtual void Remove()
        {
            CreatureConstructor.BodyParts.Remove(this);
        }

        public virtual void Attach(AttachedBodyPart attachedBodyPart)
        {
            SetAttached(attachedBodyPart);

            // Transform
            transform.parent = CreatureConstructor.Bones[attachedBodyPart.boneIndex];
            transform.Set(attachedBodyPart.serializableTransform, CreatureConstructor.transform);

            // Stretch
            SetStretch(attachedBodyPart.stretch, Vector3Int.one);

            // Colours
            SetColours(attachedBodyPart.primaryColour, attachedBodyPart.secondaryColour);
        }
        public virtual void Detach()
        {
            CreatureConstructor.RemoveBodyPart(IsFlipped ? Flipped : this);
        }

        public virtual void Flip()
        {
            // Parent
            Flipped.transform.SetParent(transform.parent);

            // Position and rotation
            Vector3 localPosition = CreatureConstructor.transform.InverseTransformPoint(transform.position);
            Quaternion localRotation = Quaternion.Inverse(CreatureConstructor.transform.rotation) * transform.rotation;

            if (CanMirror)
            {
                Flipped.gameObject.SetActive(true);

                localPosition.x *= -1;
                Vector3 worldPosition = CreatureConstructor.transform.TransformPoint(localPosition);
                Flipped.transform.position = worldPosition;

                Quaternion worldRotation = CreatureConstructor.transform.rotation * Quaternion.Euler(localRotation.eulerAngles.x, -localRotation.eulerAngles.y, -localRotation.eulerAngles.z);
                Flipped.transform.rotation = worldRotation;
            }
            else
            {
                Flipped.gameObject.SetActive(false);

                localPosition.x = 0;
                Vector3 worldPosition = CreatureConstructor.transform.TransformPoint(localPosition);
                transform.position = Flipped.transform.position = worldPosition;

                Vector3 localDirection = localRotation * Vector3.forward;
                localDirection.x = 0;
                transform.rotation = Flipped.transform.rotation = CreatureConstructor.transform.rotation * Quaternion.LookRotation(localDirection);
            }

            // Scale
            Flipped.transform.localScale = transform.localScale;

            // Stretch
            Flipped.SetStretch(AttachedBodyPart.stretch, Vector3Int.one);
        }

        public virtual void SetColours(Color primaryColour, Color secondaryColour)
        {
            AttachedBodyPart.primaryColour = primaryColour;
            AttachedBodyPart.secondaryColour = secondaryColour;

            BodyPartPrimaryMat.SetColor("_Color", primaryColour);
            BodyPartSecondaryMat.SetColor("_Color", secondaryColour);
        }

        public virtual void SetScale(Vector3 scale, MinMax minMaxScale)
        {
            transform.localScale = scale;
            transform.localScale = transform.localScale.Clamp(minMaxScale.min, minMaxScale.max);
            Flipped.transform.localScale = transform.localScale;

            OnScale?.Invoke(scale);
        }

        public void AddStretchIndex(StretchAxis axis, bool isPositive, int index)
        {
            if (stretchMap.TryGetValue(axis, out StretchIndices indices))
            {
                if (isPositive)
                {
                    stretchMap[axis].positive = index;
                }
                else
                {
                    stretchMap[axis].negative = index;
                }
            }
            else
            {
                stretchMap.Add(axis, new StretchIndices(isPositive ? index : -1, !isPositive ? index : -1));
            }
        }
        public virtual void SetStretch(Vector3 stretch, Vector3Int mask)
        {
            stretch = stretch.Clamp(-1f, 1f).Multiply(mask) + AttachedBodyPart.stretch.Multiply(Vector3Int.one - mask);

            AttachedBodyPart.stretch = stretch;

            if (stretchMap.ContainsKey(StretchAxis.X)) SetStretchBlendShape(StretchAxis.X, stretch.x);
            if (stretchMap.ContainsKey(StretchAxis.Y)) SetStretchBlendShape(StretchAxis.Y, stretch.y);
            if (stretchMap.ContainsKey(StretchAxis.Z)) SetStretchBlendShape(StretchAxis.Z, stretch.z);

            OnStretch?.Invoke();
        }
        private void SetStretchBlendShape(StretchAxis axis, float stretch)
        {
            float weight = Mathf.Abs(stretch) * 100f;

            float positive = stretch > 0 ? weight : 0;
            float negative = stretch < 0 ? weight : 0;

            SkinnedMeshRenderer.SetBlendShapeWeight(stretchMap[axis].negative, negative);
            SkinnedMeshRenderer.SetBlendShapeWeight(stretchMap[axis].positive, positive);
        }

        public virtual void SetFlipped(BodyPartConstructor bpc)
        {
            IsFlipped = true;

            Flipped = bpc;
            bpc.Flipped = this;

            Model.localScale = new Vector3(-Model.localScale.x, Model.localScale.y, Model.localScale.z);
        }
        public virtual void SetAttached(AttachedBodyPart abp)
        {
            name = abp.GUID;
            Flipped.name = name + " (Flipped)";

            CreatureConstructor.Data.AttachedBodyParts.Add(AttachedBodyPart = Flipped.AttachedBodyPart = abp);

            OnAttach?.Invoke();
        }

        public virtual void UpdateAttachmentConfiguration()
        {
            AttachedBodyPart.boneIndex = NearestBone;
            AttachedBodyPart.serializableTransform = new SerializableTransform(transform, CreatureConstructor.transform);
        }
        public void UpdateDefaultColours()
        {
            Color defaultPrimaryColour = (bodyPart.DefaultColours.primary.a != 0f) ? bodyPart.DefaultColours.primary : CreatureConstructor.Data.PrimaryColour;
            Color defaultSecondaryColour = (bodyPart.DefaultColours.secondary.a != 0f) ? bodyPart.DefaultColours.secondary : CreatureConstructor.Data.SecondaryColour;

            SetColours(defaultPrimaryColour, defaultSecondaryColour);
        }
        #endregion

        #region Enums
        [Serializable] public enum StretchAxis
        {
            X, Y, Z
        }
        #endregion

        #region Inner Classes
        [Serializable] public class StretchIndices
        {
            public int negative = -1;
            public int positive = -1;

            public StretchIndices(int p, int n)
            {
                negative = n;
                positive = p;
            }
        }
        #endregion
    }
}