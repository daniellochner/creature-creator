// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCamera))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureEditor : MonoBehaviour
    {
        #region Fields
        [Header("Setup")]
        [SerializeField] private GameObject boneToolPrefab;
        [SerializeField] private GameObject stretchToolPrefab;
        [SerializeField] private GameObject transformationToolsPrefab;
        [SerializeField] private GameObject poofPrefab;
        [Space]
        [SerializeField] private AudioClip stretchAudioClip;
        [SerializeField] private AudioClip resizeAudioClip;

        [Header("Settings")]
        [SerializeField] private float addOrRemoveCooldown = 0.05f;
        [SerializeField] private int angleLimit = 30;
        [SerializeField] private float positionSmoothing;
        [SerializeField] private float rotationSmoothing;
        [SerializeField] private GameObject arrow;

        private MeshCollider meshCollider;
        private Mesh colliderMesh;
        private AudioSource toolsAudioSource;
        private Select select;
        private Drag drag;
        private Rigidbody rb;

        private float addedOrRemovedTime;
        private bool isInteractable, isDirty;
        private string loadedCreature;
        private int cash;
        private Vector3? pointerPosOffset;

        private Outline tempModelOutline;
        private GameObject tempModel;

        private BodyPartEditor paintedBodyPart;
        #endregion

        #region Properties
        public float AddOrRemoveCooldown => addOrRemoveCooldown;
        public AudioClip StretchAudioClip => stretchAudioClip;
        public AudioClip ResizeAudioClip => resizeAudioClip;
        public GameObject PoofPrefab => poofPrefab;
        public float AngleLimit => angleLimit;

        public CreatureConstructor Constructor { get; private set; }
        public CreatureCamera Camera { get; private set; }
        
        public TransformationTools TransformationTools { get; private set; }
        public Transform BTool { get; private set; }
        public Transform FTool { get; private set; }

        public Action OnTryAddBone { get; set; }
        public Action OnTryRemoveBone { get; set; }

        public BodyPartEditor PaintedBodyPart
        {
            get => paintedBodyPart;
            set
            {
                paintedBodyPart = value;

                Color primaryColour = default, sColour = default;
                bool isPrimaryOverridden = false, isSOverride = false;
                if (paintedBodyPart)
                {
                    BodyPartConstructor bpc = paintedBodyPart.BodyPartConstructor;

                    primaryColour = bpc.AttachedBodyPart.primaryColour;
                    if (bpc.IsPrimaryOverridden)
                    {
                        isPrimaryOverridden = true;
                    }
                    else if (bpc.CanOverridePrimary)
                    {
                        primaryColour = Constructor.Data.PrimaryColour;
                    }

                    sColour = bpc.AttachedBodyPart.secondaryColour;
                    if (bpc.IsSecondaryOverridden)
                    {
                        isSOverride = true;
                    }
                    else if (bpc.CanOverrideSecondary)
                    {
                        sColour = Constructor.Data.SecondaryColour;
                    }
                }
                else
                {
                    primaryColour = Constructor.Data.PrimaryColour;
                    sColour = Constructor.Data.SecondaryColour;
                }
                EditorManager.Instance.SetPrimaryColourUI(primaryColour, isPrimaryOverridden);
                EditorManager.Instance.SetSecondaryColourUI(sColour, isSOverride);

                if (UseTemporaryOutline) tempModelOutline.enabled = !paintedBodyPart;
            }
        }
        public string LoadedCreature
        {
            get => loadedCreature;
            set
            {
                loadedCreature = value;
                EditorManager.Instance.UpdateCreaturesFormatting();
            }
        }
        public int Cash
        {
            get => cash;
            set
            {
                cash = value;
                EditorManager.Instance.UpdateStatistics();
            }
        }
        public Platform Platform
        {
            get; set;
        }

        public bool IsDirty
        {
            get => isDirty;
            set
            {
                if (isDirty != value) // Optimized to only update list when not equal.
                {
                    isDirty = value;
                    EditorManager.Instance.UpdateCreaturesFormatting();
                }
            }
        }
        public bool IsInteractable
        {
            get => isInteractable;
            set
            {
                isInteractable = value;

                foreach (Collider collider in Constructor.Body.GetComponentsInChildren<Collider>(true))
                {
                    collider.enabled = isInteractable;
                }
            }
        }
        public bool IsSelected
        {
            get => select.IsSelected;
            set => select.IsSelected = value;
        }
        public bool IsDraggable
        {
            get => drag.draggable;
            set => drag.draggable = value;
        }
        public bool UseTemporaryOutline
        {
            get => tempModel != null;
            set
            {
                if (tempModel != null)
                {
                    Destroy(tempModel);
                }
                if (value)
                {
                    tempModel = new GameObject("Temp");
                    tempModel.transform.SetParent(Constructor.Model, false);

                    MeshFilter tempBodyMeshFilter = tempModel.AddComponent<MeshFilter>();
                    tempBodyMeshFilter.mesh = colliderMesh;
                    MeshRenderer tempBodyMeshRenderer = tempModel.AddComponent<MeshRenderer>();
                    tempBodyMeshRenderer.materials = new Material[0];

                    tempModelOutline = tempModel.AddComponent<Outline>();
                    tempModelOutline.OutlineWidth = select.Outline.OutlineWidth;
                    tempModelOutline.enabled = select.UseOutline = false;
                }
                else
                {
                    select.UseOutline = true;
                }
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void OnEnable()
        {
            IsInteractable = true;

            foreach (BodyPartEditor bpe in GetComponentsInChildren<BodyPartEditor>(true))
            {
                bpe.enabled = true;
            }

            foreach (Transform bone in Constructor.Bones)
            {
                bone.GetComponent<Rigidbody>().isKinematic = false;
            }
            rb.constraints = RigidbodyConstraints.FreezeAll;

            arrow.SetActive(true);
        }
        private void OnDisable()
        {
            IsInteractable = false;

            foreach (BodyPartEditor bpe in GetComponentsInChildren<BodyPartEditor>(true))
            {
                bpe.enabled = false;
            }

            arrow.SetActive(false);
        }
        private void Update()
        {
            HandlePlatform();
        }

        private void Initialize()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Camera = GetComponent<CreatureCamera>();

            rb = GetComponent<Rigidbody>();
        }

        public void Setup()
        {
            SetupInteraction();
            SetupConstruction();
        }
        private void SetupInteraction()
        {
            meshCollider = Constructor.Model.GetComponent<MeshCollider>();
            colliderMesh = new Mesh(); // Separate mesh used to contain a snapshot of the body for the collider.

            // Interact
            toolsAudioSource = gameObject.AddComponent<AudioSource>();
            toolsAudioSource.volume = 0.25f;

            Hover hover = Constructor.Body.GetComponent<Hover>();
            hover.OnEnter.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                {
                    Camera.CameraOrbit.Freeze();
                    SetBonesVisibility(true);
                }
            });
            hover.OnExit.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                {
                    Camera.CameraOrbit.Unfreeze();

                    if (!IsSelected)
                    {
                        SetBonesVisibility(false);
                    }
                }
            });

            drag = Constructor.Body.GetComponent<Drag>();
            drag.OnPress.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    Camera.CameraOrbit.Freeze();
                }
            });
            drag.OnRelease.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    if (!hover.IsOver)
                    {
                        Camera.CameraOrbit.Unfreeze();
                    }

                    Constructor.UpdateConfiguration();
                    Constructor.UpdateDimensions();
                    foreach (LimbEditor limb in Constructor.Root.GetComponentsInChildren<LimbEditor>())
                    {
                        limb.UpdateMeshCollider();
                    }

                    EditorManager.Instance.UpdateStatistics();
                    IsDirty = true;
                }
            });
            drag.cylinderRadius = Constructor.MaxRadius;
            drag.cylinderHeight = Constructor.MaxHeight;

            Constructor.Body.GetComponent<Click>();
            select = Constructor.Body.GetComponent<Select>();
            select.OnSelect.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    SetBonesVisibility(IsSelected);
                    SetArrowsVisibility(IsSelected);
                }
                else
                if (EditorManager.Instance.IsPainting)
                {
                    PaintedBodyPart = null;
                }
            });

            // Tools
            TransformationTools = Instantiate(transformationToolsPrefab, Dynamic.Transform).GetComponent<TransformationTools>();

            BTool = Instantiate(stretchToolPrefab).transform;
            Press frontToolPress = BTool.GetComponentInChildren<Press>();
            frontToolPress.OnPress.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    Camera.CameraOrbit.Freeze();
                    pointerPosOffset = null;
                }
            });
            frontToolPress.OnRelease.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    Camera.CameraOrbit.Unfreeze();
                }
            });
            frontToolPress.OnHold.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    HandleStretchTools(0, 0, BTool);
                }
            });

            FTool = Instantiate(stretchToolPrefab).transform;
            Press backToolPress = FTool.GetComponentInChildren<Press>();
            backToolPress.OnPress.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    Camera.CameraOrbit.Freeze();
                    pointerPosOffset = null;
                }
            });
            backToolPress.OnRelease.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    Camera.CameraOrbit.Unfreeze();
                }
            });
            backToolPress.OnHold.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    HandleStretchTools(Constructor.Bones.Count - 1, Constructor.Bones.Count, FTool);
                }
            });
            
            BTool.GetChild(0).localPosition = FTool.GetChild(0).localPosition = Vector3.forward * (Constructor.BoneSettings.Length / 2f + Constructor.BoneSettings.Radius + 0.05f);
        }
        private void SetupConstruction()
        {
            Constructor.OnPreDemolish += delegate ()
            {
                // Prevent tools from being demolished along with creature.
                BTool.parent = FTool.parent = Dynamic.Transform;
                TransformationTools.Hide();
            };
            Constructor.OnConstructBody += delegate ()
            {
                EditorManager.Instance.UpdateStatistics();
                UpdateMeshCollider();
                Constructor.IsTextured = Constructor.IsTextured;
            };
            Constructor.OnSetupBone += delegate (int index)
            {
                // Hinge Joint
                HingeJoint hingeJoint = Constructor.Bones[index].GetComponent<HingeJoint>();
                if (hingeJoint != null)
                {
                    hingeJoint.anchor = new Vector3(0, 0, -Constructor.BoneSettings.Length / 2f);
                    hingeJoint.connectedBody = Constructor.Bones[index - 1].GetComponent<Rigidbody>();
                }

                // Scroll
                Scroll scroll = Constructor.Bones[index].GetComponent<Scroll>();
                scroll.OnScrollDown.RemoveAllListeners();
                scroll.OnScrollDown.AddListener(delegate 
                {
                    if (EditorManager.Instance.IsBuilding)
                    {
                        if (Constructor.GetWeight(index) > 0)
                        {
                            toolsAudioSource.PlayOneShot(ResizeAudioClip);
                        }
                        Constructor.RemoveWeight(index, 5);

                        UpdateMeshCollider();
                        UpdateBodyPartsAlignment(-1);
                    }
                });
                scroll.OnScrollUp.RemoveAllListeners();
                scroll.OnScrollUp.AddListener(delegate 
                {
                    if (EditorManager.Instance.IsBuilding)
                    {
                        if (Constructor.GetWeight(index) < 100)
                        {
                            toolsAudioSource.PlayOneShot(ResizeAudioClip);
                        }
                        Constructor.AddWeight(index, 5);

                        UpdateMeshCollider();
                        UpdateBodyPartsAlignment(1);
                    }
                });
            };
            Constructor.OnAddBone += delegate (int index)
            {
                // Bone
                GameObject addedBoneGO = Constructor.Bones[Constructor.Bones.Count - 1].gameObject;
                Instantiate(boneToolPrefab, addedBoneGO.transform);

                Rigidbody rb = addedBoneGO.AddComponent<Rigidbody>();
                rb.mass = 25f;
                rb.drag = Mathf.Infinity;
                rb.angularDrag = Mathf.Infinity;
                rb.useGravity = false;

                if (Constructor.Data.Bones.Count > 0) // Necessary to use bone data instead of childCount.
                {
                    HingeJoint hingeJoint = addedBoneGO.AddComponent<HingeJoint>();

                    hingeJoint.useLimits = true;
                    hingeJoint.limits = new JointLimits()
                    {
                        min = -angleLimit,
                        max = angleLimit,
                        bounceMinVelocity = 0
                    };
                    hingeJoint.enablePreprocessing = false;
                }

                // Tools
                addedBoneGO.AddComponent<Scroll>();

                Hover hover = addedBoneGO.AddComponent<Hover>();
                hover.OnEnter.AddListener(delegate
                {
                    if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                    {
                        Camera.CameraOrbit.Freeze();
                        SetBonesVisibility(true);
                    }
                });
                hover.OnExit.AddListener(delegate
                {
                    if (EditorManager.Instance.IsBuilding && !Input.GetMouseButton(0))
                    {
                        Camera.CameraOrbit.Unfreeze();

                        if (!IsSelected)
                        {
                            SetBonesVisibility(false);
                        }
                    }
                });

                Drag drag = addedBoneGO.AddComponent<Drag>();
                drag.OnPress.AddListener(delegate
                {
                    if (EditorManager.Instance.IsBuilding)
                    {
                        foreach (Transform bone in Constructor.Bones)
                        {
                            bone.GetComponentInChildren<Collider>().isTrigger = true;
                        }

                        Camera.CameraOrbit.Freeze();
                    }
                });
                drag.OnRelease.AddListener(delegate
                {
                    if (EditorManager.Instance.IsBuilding)
                    {
                        foreach (Transform bone in Constructor.Bones)
                        {
                            bone.GetComponentInChildren<Collider>(true).isTrigger = false;
                        }

                        if (!hover.IsOver)
                        {
                            Camera.CameraOrbit.Unfreeze();
                        }

                        Constructor.UpdateOrigin();
                        Constructor.UpdateConfiguration();
                        Constructor.UpdateDimensions();

                        UpdateMeshCollider();
                        foreach (LimbEditor limb in Constructor.Root.GetComponentsInChildren<LimbEditor>(true))
                        {
                            limb.UpdateMeshCollider();
                        }

                        EditorManager.Instance.UpdateStatistics();
                        IsDirty = true;
                    }
                });
                drag.mousePlaneAlignment = Drag.MousePlaneAlignment.ToWorldDirection;
                drag.world = transform;
                drag.boundsShape = Drag.BoundsShape.Cylinder;
                drag.cylinderHeight = Constructor.MaxHeight;
                drag.cylinderRadius = Constructor.MaxRadius;
                drag.updatePlaneOnPress = true;

                Click click = addedBoneGO.AddComponent<Click>();
                click.OnClick.AddListener(delegate
                {
                    if (EditorManager.Instance.IsBuilding)
                    {
                        IsSelected = true;
                    }
                });

                BTool.SetParent(Constructor.Bones[0]);
                FTool.SetParent(Constructor.Bones[Constructor.Bones.Count - 1]);

                BTool.localPosition = FTool.localPosition = Vector3.zero;
                BTool.localRotation = Quaternion.Euler(0, 180, 0);
                FTool.localRotation = Quaternion.identity;

                // Editor
                IsDirty = true;
            };
            Constructor.OnPreRemoveBone += delegate (int index)
            {
                FTool.SetParent(Constructor.Bones[Constructor.Bones.Count - 2]);
                FTool.localPosition = BTool.localPosition = Vector3.zero;
                FTool.localRotation = Quaternion.identity;
            };
            Constructor.OnRemoveBone += delegate (int index)
            {
                IsDirty = true;
            };
            Constructor.OnSetWeight += delegate (int index, float weight)
            {
                //UpdateMeshCollider(); // Causes a considerable amount of lag when constructing.

                Transform bone = Constructor.Bones[index];

                // Bone model
                Transform model = bone.GetChild(0);
                float x = Mathf.Lerp(0.5f, 1.5f, weight / 100f);
                float y = Mathf.Lerp(0.5f, 1.5f, weight / 100f);
                float z = 1f;
                model.localScale = new Vector3(x, y, z);

                EditorManager.Instance.UpdateStatistics();
                IsDirty = true;
            };
            Constructor.OnSetPrimaryColour += delegate (Color colour)
            {
                IsDirty = true;
            };
            Constructor.OnSetSecondaryColour += delegate (Color colour)
            {
                IsDirty = true;
            };
            Constructor.OnSetPattern += delegate (string patternID)
            {
                IsDirty = true;
            };
            Constructor.OnSetTiling += delegate (Vector2 tiling)
            {
                IsDirty = true;
                EditorManager.Instance.SetTilingUI(tiling);
            };
            Constructor.OnSetOffset += delegate (Vector2 offset)
            {
                IsDirty = true;
                EditorManager.Instance.SetOffsetUI(offset);
            };
            Constructor.OnSetShine += delegate (float shine)
            {
                IsDirty = true;
                EditorManager.Instance.SetShineUI(shine);
            };
            Constructor.OnSetMetallic += delegate (float metallic)
            {
                IsDirty = true;
                EditorManager.Instance.SetMetallicUI(metallic);
            };
            Constructor.OnAddBodyPartPrefab += delegate (GameObject main, GameObject flipped)
            {
                BodyPartEditor mainBPE = main.GetComponent<BodyPartEditor>();
                mainBPE.Setup(this);

                BodyPartEditor flippedBPE = flipped.GetComponent<BodyPartEditor>();
                flippedBPE.SetFlipped(mainBPE);
                flippedBPE.Setup(this);
            };
            Constructor.OnAddBodyPartData += delegate (BodyPart bodyPart)
            {
                Cash -= bodyPart.Price;
            };
            Constructor.OnRemoveBodyPartData += delegate (BodyPart bodyPart)
            {
                Cash += bodyPart.Price;
            };
            Constructor.OnBodyPartPrefabOverride += delegate (BodyPart bodyPart)
            {
                return bodyPart.GetPrefab(BodyPart.PrefabType.Editable);
            };
        }
        
        public void Load(CreatureData creatureData)
        {
            Constructor.Demolish();

            Cash = EditorManager.Instance.BaseCash;
            if (creatureData != null)
            {
                Constructor.Construct(creatureData);

                LoadedCreature = creatureData.Name;
            }
            else
            {
                Constructor.AddBone(0, Vector3.up * 1.5f, Quaternion.identity, 0f);
                Constructor.AddBoneToBack();
                Constructor.Body.localPosition = new Vector3(0, Constructor.Body.localPosition.y, 0); // Added bones aren't initially center-aligned.
                Constructor.SetPrimaryColour(Color.white);
                Constructor.SetSecondaryColour(Color.black);
                Constructor.SetPattern("");
                Constructor.SetTiling(Vector2.one);
                Constructor.SetOffset(Vector2.zero);
                Constructor.SetShine(0f);
                Constructor.SetMetallic(0f);

                LoadedCreature = null;
            }

            Constructor.IsTextured = Constructor.IsTextured;
            IsInteractable = IsInteractable;
            IsDirty = false;

            Deselect();
        }

        public void Deselect()
        {
            IsSelected = false;

            SetBonesVisibility(false);
            SetArrowsVisibility(false);
            PaintedBodyPart = null;

            foreach (BodyPartEditor bpe in GetComponentsInChildren<BodyPartEditor>())
            {
                bpe.Deselect();
            }
        }

        public void SetBonesVisibility(bool isVisible)
        {
            foreach (Transform bone in Constructor.Bones)
            {
                bone.GetChild(0).gameObject.SetActive(isVisible);
            }
        }
        public void SetArrowsVisibility(bool isVisible)
        {
            FTool.gameObject.SetActive(isVisible);
            BTool.gameObject.SetActive(isVisible);
        }

        public void UpdateMeshCollider()
        {
            colliderMesh.Clear();
            Constructor.SkinnedMeshRenderer.BakeMesh(colliderMesh);
            meshCollider.sharedMesh = colliderMesh;
        }
        public void UpdateBodyPartsAlignment(int alignment)
        {
            foreach (Transform bone in Constructor.Bones)
            {
                foreach (BodyPartConstructor bpc in bone.GetComponentsInChildren<BodyPartConstructor>())
                {
                    Vector3 currentDir = (bpc.transform.position - bone.position).normalized;
                    Vector3 alignmentDir = alignment * currentDir;

                    if (!Physics.Raycast(bpc.transform.position + currentDir, -currentDir, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Body")))
                    {
                        continue;
                    }

                    Vector3 displacement = hitInfo.point - bpc.transform.position;
                    if (Vector3.Dot(displacement, alignmentDir) > 0)
                    {
                        bpc.transform.position = hitInfo.point;
                    }
                }
            }
        }

        private void HandleStretchTools(int oldIndex, int newIndex, Transform tool)
        {
            Plane stretchPlane = new Plane(transform.right, Platform.transform.position);

            Ray ray = Camera.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (stretchPlane.Raycast(ray, out float distance))
            {
                Vector3 pointerPos = ray.GetPoint(distance);

                if (pointerPosOffset == null)
                {
                    pointerPosOffset = tool.InverseTransformPoint(pointerPos);
                }

                Vector3 initialPointerPos = tool.TransformPoint((Vector3)pointerPosOffset);
                bool isFarEnough = Vector3Utility.SqrDistanceComp(pointerPos, initialPointerPos, Constructor.BoneSettings.Length);
                bool hasCooledDown = Time.time > addedOrRemovedTime + AddOrRemoveCooldown;

                if (isFarEnough && hasCooledDown)
                {
                    Vector3 pointerDisplacement = pointerPos - initialPointerPos;
                    Vector3 toolDisplacement = pointerPos - tool.position;

                    float pointerAngle = Vector3.Angle(pointerDisplacement, tool.forward);
                    float toolAngle = Vector3.Angle(toolDisplacement, tool.forward);

                    bool tryAdd = (pointerAngle < 90f) && (toolAngle < angleLimit);
                    bool tryRem = (pointerAngle > (180f - angleLimit));
                    bool addOrRemove = false;

                    if (tryAdd)
                    {
                        if (Constructor.CanAddBone(oldIndex))
                        {
                            Constructor.UpdateBoneConfiguration(); // Necessary in-case creature moves slightly while editing (due to hinge-joint bounciness).

                            Vector3 position = Constructor.Bones[oldIndex].position + ((0.5f * Constructor.BoneSettings.Length) * (tool.forward + toolDisplacement.normalized));
                            Quaternion rotation = Quaternion.LookRotation(Mathf.Sign(Vector3.Dot(tool.forward, Constructor.Bones[oldIndex].forward)) * toolDisplacement.normalized, tool.up);

                            position = transform.InverseTransformPoint(position);
                            rotation = Quaternion.Inverse(transform.rotation) * rotation;

                            Constructor.AddBone(newIndex, position, rotation, Mathf.Clamp(Constructor.GetWeight(oldIndex) * 0.75f, 0f, 100f));
                            addOrRemove = true;
                        }
                        OnTryAddBone?.Invoke();
                    }
                    else 
                    if (tryRem)
                    {
                        if (Constructor.CanRemoveBone(oldIndex))
                        {
                            Constructor.RemoveBone(oldIndex);
                            addOrRemove = true;
                        }
                        OnTryRemoveBone?.Invoke();
                    }

                    if (addOrRemove)
                    {
                        toolsAudioSource.PlayOneShot(StretchAudioClip);
                        addedOrRemovedTime = Time.time;
                    }
                }
            }
        }
        private void HandlePlatform()
        {
            if (Platform != null) transform.LerpTo(Platform.Position, positionSmoothing);
        }
        #endregion
    }
}