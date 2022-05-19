// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

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
        #endregion

        #region Properties
        public CreatureEditor CreatureEditor { get; private set; }
        public BodyPartConstructor BodyPartConstructor { get; private set; }
        public BodyPartEditor Flipped { get; set; }

        public Hover Hover { get; private set; }
        public Scroll Scroll { get; private set; }
        public Click Click { get; private set; }
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
            Destroy(connectionPoint.gameObject);
        }

        protected virtual void Initialize()
        {
            BodyPartConstructor = GetComponent<BodyPartConstructor>();

            Hover = GetComponent<Hover>();
            Scroll = GetComponent<Scroll>();
            Click = GetComponent<Click>();
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
                    CreatureEditor.CameraOrbit.Freeze();
                }
            });
            Hover.OnExit.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                {
                    CreatureEditor.CameraOrbit.Unfreeze();
                }
            });

            Scroll.OnScrollUp.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && BodyPartConstructor.BodyPart.Transformations.HasFlag(Transformation.Scale))
                {
                    BodyPartConstructor.SetScale(transform.localScale + Vector3.one * BodyPartConstructor.BodyPart.ScaleIncrement, BodyPartConstructor.BodyPart.MinMaxScale);
                    CreatureEditor.IsDirty = true;
                }
            });
            Scroll.OnScrollDown.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && BodyPartConstructor.BodyPart.Transformations.HasFlag(Transformation.Scale))
                {
                    BodyPartConstructor.SetScale(transform.localScale - Vector3.one * BodyPartConstructor.BodyPart.ScaleIncrement, BodyPartConstructor.BodyPart.MinMaxScale);
                    CreatureEditor.IsDirty = true;
                }
            });

            LDrag.world = CreatureEditor.transform;
            LDrag.OnPress.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    CreatureEditor.CameraOrbit.Freeze();
                }
            });
            LDrag.OnRelease.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0) && !Hover.IsOver)
                {
                    CreatureEditor.CameraOrbit.Unfreeze();
                }
            });
            LDrag.OnBeginDrag.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    if (Input.GetKey(KeyCode.LeftAlt) && !IsCopied && BodyPartConstructor.AttachedBodyPart.boneIndex != -1)
                    {
                        LDrag.IsDragging = LDrag.IsPressing = false;

                        if (CanCopy)
                        {
                            Copy();
                        }
                    }
                    else
                    {
                        connectionPoint.parent = Flipped.connectionPoint.parent = transform.parent = Flipped.transform.parent = Dynamic.Transform;
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
                    if (CanAttach(out Vector3 aPosition, out Quaternion aRotation))
                    {
                        connectionPoint.parent = Flipped.connectionPoint.parent = transform.parent = Flipped.transform.parent = BodyPartConstructor.CreatureConstructor.Bones[BodyPartConstructor.NearestBone];
                        BodyPartConstructor.UpdateAttachmentConfiguration();

                        if (IsSelected)
                        {
                            SetToolsVisibility(true);
                        }
                        IsInteractable = true;
                        IsCopied = false;
                    }
                    else
                    {
                        Instantiate(CreatureEditor.PoofPrefab, transform.position, Quaternion.identity, Dynamic.Transform);
                        BodyPartConstructor.Detach();

                        CreatureEditor.CameraOrbit.Unfreeze(); // Since the body part is destroyed immediately, the OnRelease() method is not invoked.
                    }

                    CreatureEditor.IsDirty = true;
                }
            });

            RDrag.world = connectionPoint;
            RDrag.OnDrag.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    BodyPartConstructor.Flip();
                }
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
                    else if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(CreatureEditor.CameraOrbit.Camera, Input.mousePosition), out RaycastHit hitInfo) && !hitInfo.collider.CompareTag("Body Part"))
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
            BodyPartConstructor.OnSetAttached += delegate
            {
                EditorManager.Instance.UpdateStatistics();
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

            BodyPartConstructor main = CreatureEditor.CreatureConstructor.AddBodyPart(bodyPartID);
            main.SetAttached(new AttachedBodyPart(bodyPartID));

            main.SetPrimaryColour(BodyPartConstructor.AttachedBodyPart.primaryColour);
            main.SetSecondaryColour(BodyPartConstructor.AttachedBodyPart.secondaryColour);
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
            if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(CreatureEditor.CameraOrbit.Camera, Input.mousePosition), out RaycastHit raycastHit) && raycastHit.collider.CompareTag("Body"))
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

        public virtual void Deselect()
        {
            Select.Outline.enabled = false;

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