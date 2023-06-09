// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(BodyPartConstructor))]
    public class BodyPartEditor : MonoBehaviour, IFlippable<BodyPartEditor>
    {
        #region Fields
        protected Mesh colliderMesh;
        protected MeshCollider meshCollider;

        private bool isInteractable;
        private Transform connectionPoint;

        protected float addedOrRemovedTime;
        #endregion

        #region Properties
        public CreatureEditor CreatureEditor { get; private set; }
        public BodyPartConstructor BodyPartConstructor { get; set; }
        public BodyPartEditor Flipped { get; set; }

        public Hover Hover { get; private set; }
        public Scroll Scroll { get; private set; }
        public Drag LDrag { get; private set; }
        public Drag RDrag { get; private set; }
        public Select Select { get; private set; }

        public bool IsFlipped
        {
            get;
            set;
        }
        public bool IsCopied
        {
            get;
            set;
        }
        public bool IsSelected
        {
            get => Select.IsSelected;
            set => Select.IsSelected = value;
        }
        public virtual bool IsInteractable
        {
            get => isInteractable;
            set
            {
                isInteractable = value;

                foreach (Collider collider in BodyPartConstructor.Model.GetComponentsInChildren<Collider>(true))
                {
                    collider.enabled = isInteractable;
                }
                foreach (Collider collider in Flipped.BodyPartConstructor.Model.GetComponentsInChildren<Collider>(true))
                {
                    collider.enabled = isInteractable;
                }
            }
        }

        public virtual bool CanCopy
        {
            get
            {
                return EditorManager.Instance.CanAddBodyPart(BodyPartConstructor.AttachedBodyPart.bodyPartID);
            }
        }
        #endregion

        #region Methods
        public void Awake()
        {
            Initialize();
        }
        private void OnDestroy()
        {
            if (connectionPoint != null){
                Destroy(connectionPoint.gameObject);
            }
        }

        protected virtual void Initialize()
        {
            BodyPartConstructor = GetComponent<BodyPartConstructor>();

            Hover = GetComponent<Hover>();
            Scroll = GetComponent<Scroll>();
            Select = GetComponent<Select>();

            Drag[] drags = GetComponents<Drag>();
            LDrag = drags[0];
            RDrag = drags[1];

            connectionPoint = new GameObject("Connection.Point").transform;
            connectionPoint.SetParent(Dynamic.Transform);

            meshCollider = BodyPartConstructor.Model.GetComponentInChildren<MeshCollider>();
            colliderMesh = new Mesh();
        }

        public virtual void Setup(CreatureEditor creatureEditor)
        {
            CreatureEditor = creatureEditor;

            SetupInteraction();
            SetupConstruction();
        }
        private void SetupInteraction()
        {
            Hover.OnEnter.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                {
                    CreatureEditor.Camera.CameraOrbit.Freeze();
                }
            });
            Hover.OnExit.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                {
                    CreatureEditor.Camera.CameraOrbit.Unfreeze();
                }
            });

            Scroll.OnScrollUp.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && BodyPartConstructor.BodyPart.Transformations.HasFlag(Transformation.Scale))
                {
                    BodyPartConstructor.SetScale(transform.localScale + Vector3.one * BodyPartConstructor.BodyPart.ScaleIncrement, BodyPartConstructor.BodyPart.MinMaxScale);

                    EditorManager.Instance.UpdateStatistics();

                    EditorManager.Instance.TakeSnapshot(Change.ScrollBodyPartUp, 1f);
                }
            });
            Scroll.OnScrollDown.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && BodyPartConstructor.BodyPart.Transformations.HasFlag(Transformation.Scale))
                {
                    BodyPartConstructor.SetScale(transform.localScale - Vector3.one * BodyPartConstructor.BodyPart.ScaleIncrement, BodyPartConstructor.BodyPart.MinMaxScale);

                    EditorManager.Instance.UpdateStatistics();

                    EditorManager.Instance.TakeSnapshot(Change.ScaleBodyPartDown, 1f);
                }
            });

            LDrag.world = CreatureEditor.transform;
            LDrag.OnPress.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    CreatureEditor.Camera.CameraOrbit.Freeze();

                    if (SystemUtility.IsDevice(DeviceType.Handheld))
                    {
                        StartCoroutine(CreatureEditor.HoldDraggableRoutine(LDrag));
                    }
                }
            });
            LDrag.OnHold.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    if (!LDrag.draggable)
                    {
                        if (Time.time > addedOrRemovedTime + CreatureEditor.AddOrRemoveCooldown)
                        {
                            InputUtility.GetDelta(out float deltaX, out float deltaY);

                            if (deltaY > 0)
                            {
                                Scroll.OnScrollUp.Invoke();
                            }
                            else
                            if (deltaY < 0)
                            {
                                Scroll.OnScrollDown.Invoke();
                            }

                            addedOrRemovedTime = Time.time;
                        }
                    }
                }
                else
                {
                    // Destroy held parts when you switch mode...
                    if (transform.parent == Dynamic.Transform)
                    {
                        Instantiate(CreatureEditor.PoofPrefab, transform.position, Quaternion.identity, Dynamic.Transform);
                        BodyPartConstructor.Detach();
                        EditorManager.Instance.UpdateStatistics();
                    }
                }
            });
            LDrag.OnRelease.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0) && !Hover.IsOver)
                {
                    CreatureEditor.Camera.CameraOrbit.Unfreeze();
                }
            });
            LDrag.OnBeginDrag.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    if (InputUtility.GetKey(KeybindingsManager.Data.Copy) && !IsCopied && BodyPartConstructor.AttachedBodyPart.boneIndex != -1)
                    {
                        LDrag.IsDragging = LDrag.IsPressing = false;

                        if (CanCopy)
                        {
                            Copy();
                        }
                    }
                    else
                    {
                        if (connectionPoint != null) // TODO: Why is connection point null in the first place? Bug?
                        {
                            connectionPoint.parent = Flipped.connectionPoint.parent = transform.parent = Flipped.transform.parent = Dynamic.Transform;
                        }
                        IsInteractable = false;
                    }

                    IsSelected = false;
                }
            });
            LDrag.OnDrag.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    bool isAttachable = CanAttach(out Vector3 aPosition, out Quaternion aRotation);

                    if (isAttachable)
                    {
                        BodyPartConstructor.transform.SetPositionAndRotation(aPosition, aRotation);
                        BodyPartConstructor.Flip();

                        connectionPoint.position = aPosition;
                        Flipped.connectionPoint.position = Flipped.transform.position;
                    }
                    else
                    {
                        Flipped.gameObject.SetActive(false); // Hide flipped body part.
                    }

                    LDrag.controlDrag = Flipped.LDrag.controlDrag = !isAttachable;
                }
            });
            LDrag.OnEndDrag.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    bool isAttached = BodyPartConstructor.AttachedBodyPart.boneIndex != -1;

                    if (CanAttach(out Vector3 aPosition, out Quaternion aRotation))
                    {
                        connectionPoint.parent = Flipped.connectionPoint.parent = transform.parent = Flipped.transform.parent = BodyPartConstructor.CreatureConstructor.Bones[BodyPartConstructor.NearestBone];
                        BodyPartConstructor.UpdateAttachmentConfiguration();

                        IsInteractable = true;
                        IsCopied = false;

                        if (isAttached)
                        {
                            EditorManager.Instance.TakeSnapshot(Change.DragBodyPart);
                        }
                        else
                        {
                            EditorManager.Instance.TakeSnapshot(Change.AttachBodyPart);
                        }
                    }
                    else
                    {
                        Instantiate(CreatureEditor.PoofPrefab, transform.position, Quaternion.identity, Dynamic.Transform);
                        BodyPartConstructor.Detach();

                        CreatureEditor.Camera.CameraOrbit.Unfreeze(); // Since the body part is destroyed immediately, the OnRelease() method is not invoked.

                        if (isAttached)
                        {
                            EditorManager.Instance.TakeSnapshot(Change.DetachBodyPart);
                        }
                    }

                    EditorManager.Instance.UpdateStatistics();
                }
            });
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                LDrag.mobileTouchOffset = CreatureEditor.TouchOffset;
            }

            RDrag.world = connectionPoint;
            RDrag.OnDrag.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    BodyPartConstructor.Flip();
                }
            });
            RDrag.OnEndDrag.AddListener(delegate
            {
                BodyPartConstructor.SetPositionAndRotation(transform.position, transform.rotation);
                EditorManager.Instance.TakeSnapshot(Change.NudgeBodyPart);
            });

            Select.OnSelect.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    SetToolsVisibility(IsSelected);
                }
                else
                if (EditorManager.Instance.IsPainting)
                {
                    if (IsSelected)
                    {
                        CreatureEditor.PaintedBodyPart = this;
                    }
                    else if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(CreatureEditor.Camera.CameraOrbit.Camera, Input.mousePosition), out RaycastHit hitInfo) && !hitInfo.collider.CompareTag("Body Part"))
                    {
                        CreatureEditor.PaintedBodyPart = null;
                    }
                }
                Flipped.Select.Outline.enabled = IsSelected;
            });
        }
        private void SetupConstruction()
        {
            BodyPartConstructor.OnStretch += delegate
            {
                UpdateMeshCollider();
            };
            BodyPartConstructor.OnAttach += delegate
            {
                connectionPoint.parent = Flipped.connectionPoint.parent = transform.parent;
                connectionPoint.position = transform.position;
                Flipped.connectionPoint.position = Flipped.transform.position;
            };

            BodyPartConstructor.OnPreOverrideMaterials += delegate (Renderer renderer)
            {
                renderer.GetComponent<Outline>().enabled = false; // QuickOutline breaks if you modify the material while it is enabled
            };
            BodyPartConstructor.OnOverrideMaterials += delegate (Renderer renderer)
            {
                renderer.GetComponent<Outline>().enabled = true;
            };
        }

        public virtual BodyPartEditor Copy()
        {
            string bodyPartID = BodyPartConstructor.AttachedBodyPart.bodyPartID;

            BodyPartConstructor main = CreatureEditor.Constructor.AddBodyPart(bodyPartID);
            main.SetupAttachment(new AttachedBodyPart(bodyPartID));

            Color pColour = BodyPartConstructor.AttachedBodyPart.primaryColour;
            if (pColour.a != 0)
            {
                main.SetPrimaryColour(pColour);
            }
            Color sColour = BodyPartConstructor.AttachedBodyPart.secondaryColour;
            if (sColour.a != 0)
            {
                main.SetSecondaryColour(sColour);
            }
            main.SetStretch(BodyPartConstructor.AttachedBodyPart.stretch, Vector3Int.one);
            main.transform.SetPositionAndRotation(transform.position, transform.rotation);
            main.transform.localScale = transform.localScale;
            main.transform.parent = Dynamic.Transform;
            main.Flipped.gameObject.SetActive(false);

            BodyPartEditor copiedBPE = main.GetComponent<BodyPartEditor>();
            if (copiedBPE != null)
            {
                copiedBPE.LDrag.OnMouseButtonDown();
                copiedBPE.LDrag.Plane = LDrag.Plane;
                copiedBPE.IsCopied = true;
            }

            return copiedBPE;
        }

        public virtual bool CanAttach(out Vector3 aPosition, out Quaternion aRotation)
        {
            Vector3 origin = Input.mousePosition;

            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                origin += Vector3.up * CreatureEditor.TouchOffset;
            }

            if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(CreatureEditor.Camera.CameraOrbit.Camera, origin), out RaycastHit raycastHit) && CanAttachToCollider(raycastHit.collider))
            {
                aPosition = raycastHit.point;
                aRotation = Quaternion.LookRotation(raycastHit.normal, CreatureEditor.transform.up);
                return true;
            }
            else
            {
                aPosition = transform.position;
                aRotation = transform.rotation;
                return false;
            }
        }
        private bool CanAttachToCollider(Collider collider)
        {
            // Body
            bool tryAttachToBody = collider.CompareTag("Body");

            // Body Parts
            bool tryAttachToMouth = false;
            if (collider.CompareTag("Body Part"))
            {
                BodyPartConstructor bpc = collider.GetComponentInParent<BodyPartConstructor>();
                if (bpc != null && bpc.BodyPart is Mouth && (BodyPartConstructor.BodyPart is Eye || BodyPartConstructor.BodyPart is Ear || BodyPartConstructor.BodyPart is Nose))
                {
                    tryAttachToMouth = true;
                }
            }
            
            return tryAttachToBody || tryAttachToMouth;
        }

        public virtual void Deselect()
        {
            IsSelected = false;

            SetToolsVisibility(false);
        }

        public void SetToolsVisibility(bool isVisible)
        {
            if (isVisible)
            {
                CreatureEditor.TransformationTools.Show(this, BodyPartConstructor.BodyPart.Transformations);
            }
            else
            {
                CreatureEditor.TransformationTools.Hide();
            }
        }
        public void SetFlipped(BodyPartEditor main)
        {
            IsFlipped = true;

            Flipped = main;
            main.Flipped = this;
        }

        public void UpdateMeshCollider()
        {
            if (BodyPartConstructor.SkinnedMeshRenderer != null)
            {
                colliderMesh.Clear();
                BodyPartConstructor.SkinnedMeshRenderer.BakeMesh(colliderMesh);
                meshCollider.sharedMesh = colliderMesh;
            }
        }
        #endregion
    }
}