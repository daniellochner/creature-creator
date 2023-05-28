// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCloner))]
    public class CreatureRagdoll : MonoBehaviour
    {
        #region Fields
        private CreatureConstructor original, ragdoll;
        #endregion

        #region Properties
        private CreatureCloner CreatureCloner { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureCloner = GetComponent<CreatureCloner>();
            original = GetComponent<CreatureConstructor>();
        }

        public CreatureConstructor Generate(Vector3? pos = null)
        {
            if (pos == null)
            {
                pos = original.Body.position;
            }
            return Generate(original.Data, (Vector3)pos);
        }
        public CreatureConstructor Generate(CreatureData creatureData, Vector3 pos)
        {
            ragdoll = CreatureCloner.Clone(creatureData, transform.position, transform.rotation, transform.parent);
            ragdoll.Body.position = pos;
            ragdoll.SkinnedMeshRenderer.updateWhenOffscreen = true;

            CopyPositionsAndRotations(original, ragdoll);
            ragdoll.Body.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Ragdoll"));

            for (int i = 0; i < ragdoll.Bones.Count; ++i)
            {
                SetupBoneRagdoll(ragdoll.Bones[i], ragdoll.BoneSettings.Radius * Mathf.Lerp(1, 5, ragdoll.GetWeight(i) / 100f), 30);
            }
            //foreach (LimbConstructor limb in ragdoll.Limbs)
            //{
            //    SetupLimbRagdoll(limb);

            //    limb.FlippedLimb.Root.localScale = Vector3.one;
            //    limb.Flip();
            //    SetupLimbRagdoll(limb.FlippedLimb);
            //}

            ragdoll.GetComponent<CreatureOptimizer>().Optimize();

            return ragdoll;
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

        private void SetupBoneRagdoll(Transform bone, float radius, float angleLimit)
        {
            SphereCollider collider = bone.gameObject.AddComponent<SphereCollider>();
            collider.radius = radius;
            Rigidbody rigidbody = bone.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

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
            for (int i = 0; i < limb.Bones.Length; ++i)
            {
                SetupBoneRagdoll(limb.Bones[i], 0.05f, 10);
            }

            if (limb.ConnectedExtremity != null)
            {
                limb.ConnectedExtremity.transform.parent = limb.Extremity;

                Quaternion prevRotation = limb.ConnectedExtremity.transform.rotation;
                limb.InvokeAtEndOfFrame(delegate
                {
                    limb.ConnectedExtremity.transform.rotation = prevRotation;
                });
            }

            FixedJoint fixedJoint = limb.Bones[0].gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = limb.transform.parent.GetComponent<Rigidbody>();
        }
        #endregion
    }
}