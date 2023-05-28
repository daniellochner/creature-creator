// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LimbEditor : BodyPartEditor
    {
        #region Fields
        protected LookAtConstraint[] boneConstraints;
        protected RotationConstraint extremityConstraint;
        private MeshRenderer[] toolRenderers;
        private SphereCollider[] toolColliders;
        private bool isHandlingAlignment;
        private Quaternion[] offsets;
        #endregion

        #region Properties
        public LimbConstructor LimbConstructor => BodyPartConstructor as LimbConstructor;
        public LimbEditor FlippedLimb => Flipped as LimbEditor;

        public ExtremityEditor ConnectedExtremity { get; set; }

        public bool IsHandlingAlignment
        {
            get => isHandlingAlignment;
            set
            {
                isHandlingAlignment = value;

                foreach (LookAtConstraint boneConstraint in boneConstraints)
                {
                    boneConstraint.constraintActive = isHandlingAlignment;
                }
                extremityConstraint.constraintActive = isHandlingAlignment;
            }
        }
        public override bool IsInteractable
        {
            get => base.IsInteractable;
            set
            {
                base.IsInteractable = value;

                if (LimbConstructor.ConnectedExtremity != null)
                {
                    LimbConstructor.ConnectedExtremity.GetComponent<ExtremityEditor>().IsInteractable = value;
                }
            }
        }
        public override bool CanCopy
        {
            get
            {
                bool canCopy = base.CanCopy;

                if (LimbConstructor.ConnectedExtremity != null)
                {
                    canCopy &= EditorManager.Instance.CanAddBodyPart(LimbConstructor.ConnectedExtremity.AttachedBodyPart.bodyPartID);
                }

                return canCopy;
            }
        }
        #endregion

        #region Methods
        private void OnEnable()
        {
            IsHandlingAlignment = true;
        }
        private void OnDisable()
        {
            IsHandlingAlignment = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            toolRenderers = LimbConstructor.Root.GetComponentsInChildren<MeshRenderer>();
            toolColliders = LimbConstructor.Root.GetComponentsInChildren<SphereCollider>();

            Transform[] bones = LimbConstructor.Bones;
            boneConstraints = new LookAtConstraint[bones.Length - 1];
            for (int i = 0; i < bones.Length - 1; i++)
            {
                boneConstraints[i] = bones[i].GetComponent<LookAtConstraint>();
            }
            extremityConstraint = bones[bones.Length - 1].GetComponent<RotationConstraint>();
        }

        public override void Setup(CreatureEditor creatureEditor)
        {
            base.Setup(creatureEditor);

            SetupInteraction();
            SetupBones();
            SetupConstructor();
        }
        private void SetupInteraction()
        {
            Hover.OnEnter.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                {
                    SetBonesVisibility(true);
                }
            });
            Hover.OnExit.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0) && !IsSelected)
                {
                    SetBonesVisibility(false);
                }
            });

            Scroll.OnScrollUp.RemoveAllListeners();
            Scroll.OnScrollUp.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    LimbConstructor.AddWeight(0, 5);
                    FlippedLimb.LimbConstructor.AddWeight(0, 5);

                    UpdateMeshCollider();
                    FlippedLimb.UpdateMeshCollider();

                    EditorManager.Instance.UpdateStatistics();

                    EditorManager.Instance.TakeSnapshot(Change.ScrollLimbBoneUp, 1f);
                }
            });
            Scroll.OnScrollDown.RemoveAllListeners();
            Scroll.OnScrollDown.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    LimbConstructor.RemoveWeight(0, 5);
                    FlippedLimb.LimbConstructor.RemoveWeight(0, 5);

                    UpdateMeshCollider();
                    FlippedLimb.UpdateMeshCollider();

                    EditorManager.Instance.UpdateStatistics();

                    EditorManager.Instance.TakeSnapshot(Change.ScrollLimbBoneDown, 1f);
                }
            });

            LDrag.OnEndDrag.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    ExtremityConstructor connectedExtremity = LimbConstructor.ConnectedExtremity;
                    if (connectedExtremity != null)
                    {
                        connectedExtremity.transform.parent = connectedExtremity.Flipped.transform.parent = transform.parent;
                        connectedExtremity.UpdateAttachmentConfiguration();
                    }

                    LimbConstructor.UpdateAttachmentConfiguration();

                    UpdateMeshCollider();
                    FlippedLimb.UpdateMeshCollider();
                }
            });

            Select.OnSelect.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    SetBonesVisibility(IsSelected);
                }
            });
        }
        private void SetupBones()
        {
            for (int i = 0; i < LimbConstructor.Bones.Length; i++)
            {
                Transform bone = LimbConstructor.Bones[i];
                if (i > 0)
                {
                    int index = i;

                    Scroll boneScroll = bone.gameObject.GetComponent<Scroll>();
                    boneScroll.OnScrollUp.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            LimbConstructor.AddWeight(index, 5);
                            FlippedLimb.LimbConstructor.AddWeight(index, 5);

                            UpdateMeshCollider();
                            FlippedLimb.UpdateMeshCollider();

                            EditorManager.Instance.UpdateStatistics();

                            EditorManager.Instance.TakeSnapshot(Change.ScrollLimbBoneUp, 1f);
                        }
                    });
                    boneScroll.OnScrollDown.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            LimbConstructor.RemoveWeight(index, 5);
                            FlippedLimb.LimbConstructor.RemoveWeight(index, 5);

                            UpdateMeshCollider();
                            FlippedLimb.UpdateMeshCollider();

                            EditorManager.Instance.UpdateStatistics();

                            EditorManager.Instance.TakeSnapshot(Change.ScrollLimbBoneUp, 1f);
                        }
                    });

                    Hover boneHover = bone.gameObject.GetComponent<Hover>();
                    boneHover.OnEnter.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                        {
                            CreatureEditor.Camera.CameraOrbit.Freeze();
                            SetBonesVisibility(true);
                        }
                    });
                    boneHover.OnExit.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                        {
                            CreatureEditor.Camera.CameraOrbit.Unfreeze();

                            if (!IsSelected)
                            {
                                SetBonesVisibility(false);
                            }
                        }
                    });

                    Drag boneDrag = bone.gameObject.GetComponent<Drag>();
                    boneDrag.world = LDrag.world;
                    boneDrag.cylinderRadius = CreatureEditor.Constructor.MaxRadius;
                    if (index < LimbConstructor.Bones.Length - 1 || LimbConstructor.Limb is Arm)
                    {
                        boneDrag.cylinderHeight = CreatureEditor.Constructor.MaxHeight;
                    }
                    boneDrag.OnPress.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            CreatureEditor.Camera.CameraOrbit.Freeze();

                            if (SystemUtility.IsDevice(DeviceType.Handheld))
                            {
                                StartCoroutine(CreatureEditor.HoldDraggableRoutine(boneDrag));
                            }
                        }
                    });
                    boneDrag.OnHold.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            if (!boneDrag.draggable)
                            {
                                if (Time.time > addedOrRemovedTime + CreatureEditor.AddOrRemoveCooldown)
                                {
                                    InputUtility.GetDelta(out float deltaX, out float deltaY);

                                    if (deltaY > 0)
                                    {
                                        boneScroll.OnScrollUp.Invoke();
                                    }
                                    else
                                    if (deltaY < 0)
                                    {
                                        boneScroll.OnScrollDown.Invoke();
                                    }

                                    addedOrRemovedTime = Time.time;
                                }
                            }
                        }
                    });
                    boneDrag.OnDrag.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            Vector3 localBonePosition = CreatureEditor.transform.InverseTransformPoint(bone.position);
                            localBonePosition.x *= -1;
                            Vector3 worldBonePosition = CreatureEditor.transform.TransformPoint(localBonePosition);

                            FlippedLimb.LimbConstructor.Bones[index].position = worldBonePosition;
                        }
                    });
                    boneDrag.OnRelease.AddListener(delegate
                    {
                        if (EditorManager.Instance.IsBuilding)
                        {
                            if (!boneHover.IsOver && !Hover.IsOver)
                            {
                                CreatureEditor.Camera.CameraOrbit.Unfreeze();

                                if (!IsSelected)
                                {
                                    SetBonesVisibility(false);
                                }
                            }
                        }
                    });
                    boneDrag.OnEndDrag.AddListener(delegate
                    {
                        LimbConstructor.UpdateAttachmentConfiguration();

                        UpdateMeshCollider();
                        FlippedLimb.UpdateMeshCollider();

                        EditorManager.Instance.TakeSnapshot(Change.DragLimbBone);
                    });
                }
                if (i < LimbConstructor.Bones.Length - 1)
                {
                    boneConstraints[i].worldUpObject = CreatureEditor.transform;
                }
            }
            
            SetBonesVisibility(false);
        }
        private void SetupConstructor()
        {
            Vector3 s = Vector3.one * (SystemUtility.IsDevice(DeviceType.Handheld) ? 1.5f : 1f);
            LimbConstructor.OnSetWeight += delegate(int index, float weight)
            {
                toolRenderers[index].transform.localScale = Vector3.Lerp(s, s * 2, weight / 100f);
            };
        }
        
        public override BodyPartEditor Copy()
        {
            LimbEditor copiedLE = base.Copy() as LimbEditor;

            // Limb
            LimbConstructor copiedLC = copiedLE.LimbConstructor;
            for (int i = 0; i < copiedLC.AttachedLimb.bones.Count; i++)
            {
                copiedLC.Bones[i].position = LimbConstructor.Bones[i].position;
                copiedLC.Bones[i].rotation = LimbConstructor.Bones[i].rotation;
                copiedLC.SetWeight(i, LimbConstructor.GetWeight(i));
            }

            // Connected Extremity
            ExtremityConstructor connectedExtremity = LimbConstructor.ConnectedExtremity;
            if (connectedExtremity != null)
            {
                ExtremityEditor copiedEE = connectedExtremity.GetComponent<ExtremityEditor>().Copy() as ExtremityEditor;

                copiedEE.LDrag.IsDragging = copiedEE.LDrag.IsPressing = false;
                copiedEE.IsSelected = false;

                copiedEE.ExtremityConstructor.ConnectToLimb(copiedLE.LimbConstructor);
                copiedEE.ExtremityConstructor.Flip();
            }

            return copiedLE;
        }
        public override void Deselect()
        {
            base.Deselect();

            SetBonesVisibility(false);
        }

        public void SetBonesVisibility(bool isVisible)
        {
            for (int i = 0; i < LimbConstructor.Bones.Length; i++)
            {
                toolRenderers[i].enabled = isVisible;
                toolColliders[i].enabled = isVisible;
            }
            toolColliders[0].enabled = false;
        }
        #endregion
    }
}