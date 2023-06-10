// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCloner))]
    public class CreatureRagdoll : MonoBehaviour
    {
        #region Fields
        [SerializeField] private MinMax minMaxForce;
        #endregion

        #region Properties
        private CreatureCloner CreatureCloner { get; set; }
        private CreatureConstructor Constructor { get; set; }

        public CreatureConstructor Ragdoll { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureCloner = GetComponent<CreatureCloner>();
            Constructor = GetComponent<CreatureConstructor>();
        }

        public CreatureConstructor Generate(Vector3? pos = null, bool dismember = false)
        {
            if (pos == null)
            {
                pos = Constructor.Body.position;
            }
            return Generate(Constructor.Data, (Vector3)pos, dismember);
        }
        public CreatureConstructor Generate(CreatureData creatureData, Vector3 pos, bool dismember)
        {
            Ragdoll = CreatureCloner.Clone(creatureData, transform.position, transform.rotation, transform.parent);
            Ragdoll.Body.position = pos;
            Ragdoll.SkinnedMeshRenderer.updateWhenOffscreen = true;

            CopyPositionsAndRotations(Constructor, Ragdoll);
            Ragdoll.Body.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Ragdoll"));

            for (int i = 0; i < Ragdoll.Bones.Count; ++i)
            {
                SetupBoneRagdoll(Ragdoll.Bones[i], Ragdoll.BoneSettings.Radius * Mathf.Lerp(1, 5, Ragdoll.GetWeight(i) / 100f), 30);
            }
            foreach (LimbConstructor limb in Ragdoll.Limbs)
            {
                SetupLimbRagdoll(limb);
                SetupLimbRagdoll(limb.FlippedLimb);
            }

            if (SettingsManager.Data.OptimizeCreatures)
            {
                Ragdoll.GetComponent<CreatureOptimizer>().Optimize();
            }
            else
            if (dismember)
            {
                foreach (BodyPartConstructor bpc in Ragdoll.BodyParts)
                {
                    if (bpc.IsVisible)
                    {
                        DismemberBodyPart(bpc);
                    }
                    if (bpc.Flipped.IsVisible)
                    {
                        DismemberBodyPart(bpc.Flipped);
                    }
                }
            }

            return Ragdoll;
        }

        private void CopyPositionsAndRotations(CreatureConstructor from, CreatureConstructor to)
        {
            to.Body.localRotation = from.Body.localRotation;
            to.Root.localPosition = from.Root.localPosition;

            for (int i = 0; i < to.Bones.Count; i++)
            {
                to.Bones[i].SetPositionAndRotation(from.Bones[i].position, from.Bones[i].rotation);
            }
            for (int i = 0; i < to.Limbs.Count; i++)
            {
                for (int j = 0; j < to.Limbs[i].Bones.Length; j++)
                {
                    to.Limbs[i].Bones[j].SetPositionAndRotation(from.Limbs[i].Bones[j].position, from.Limbs[i].Bones[j].rotation);
                }
            }
        }
        private void AddRigidbody(GameObject obj, float radius)
        {
            SphereCollider collider = obj.AddComponent<SphereCollider>();
            collider.radius = radius;

            Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rigidbody.AddForce(new Vector3(minMaxForce.Random, minMaxForce.Random, minMaxForce.Random), ForceMode.Impulse);
        }

        private void SetupBoneRagdoll(Transform bone, float radius, float angleLimit)
        {
            AddRigidbody(bone.gameObject, radius);

            int index = bone.GetSiblingIndex();
            if (index > 0)
            {
                HingeJoint hingeJoint = bone.gameObject.AddComponent<HingeJoint>();
                hingeJoint.useLimits = true;
                hingeJoint.limits = new JointLimits()
                {
                    min = -angleLimit,
                    max = angleLimit,
                    bounceMinVelocity = 0
                };
                hingeJoint.enablePreprocessing = false;

                Transform prevBone = bone.parent.GetChild(index - 1);
                hingeJoint.anchor = new Vector3(0, 0, -(Vector3.Distance(bone.position, prevBone.position) / 2f));
                hingeJoint.connectedBody = prevBone.GetComponent<Rigidbody>();
            }
        }
        private void SetupLimbRagdoll(LimbConstructor limb)
        {
            //for (int i = 0; i < limb.Bones.Length; ++i)
            //{
            //    SetupBoneRagdoll(limb.Bones[i], 0.05f, 10);
            //}

            //FixedJoint fixedJoint = limb.Bones[0].gameObject.AddComponent<FixedJoint>();
            //fixedJoint.connectedBody = limb.transform.parent.GetComponent<Rigidbody>();

            if (limb.ConnectedExtremity != null)
            {
                limb.InvokeAtEndOfFrame(delegate
                {
                    limb.ConnectedExtremity.transform.SetParent(limb.Extremity, true);
                });
            }
        }

        private void DismemberBodyPart(BodyPartConstructor bpc)
        {
            bpc.transform.SetParent(Ragdoll.transform, true);
            AddRigidbody(bpc.gameObject, 0.1f);
        }
        #endregion
    }
}