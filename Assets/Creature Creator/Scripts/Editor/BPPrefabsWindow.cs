// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BPPrefabsWindow : EditorWindow
    {
        #region Fields
        [SerializeField] private GameObject moveToolPrefab;
        [SerializeField] private Database database;
        [SerializeField] private string outputDirectory;
        [SerializeField] private SaveType type;
        [SerializeField] private GameObject[] models;

        private SerializedProperty _models, _type, _database, _outputDirectory, _moveTool;
        private SerializedObject target;

        private Dictionary<SaveType, PrefabTypes> typeMapping = new Dictionary<SaveType, PrefabTypes>()
        {
            {
                SaveType.Detail, new PrefabTypes()
            },
            {
                SaveType.Tail, new PrefabTypes()
            },
            {
                SaveType.Weapon, new PrefabTypes()
            },
            {
                SaveType.Wing, new PrefabTypes()
            },
            {
                SaveType.Foot, new PrefabTypes()
                {
                    constructType = ConstructType.Foot,
                    editType = EditType.Foot
                }
            },
            {
                SaveType.Hand, new PrefabTypes()
                {
                    constructType = ConstructType.Hand,
                    editType = EditType.Hand
                }
            },
            {
                SaveType.Ear, new PrefabTypes()
            },
            {
                SaveType.Eye, new PrefabTypes()
            },
            {
                SaveType.Mouth, new PrefabTypes()
                {
                    constructType = ConstructType.Mouth
                }
            },
            {
                SaveType.Nose, new PrefabTypes()
            },
            {
                SaveType.Arm, new PrefabTypes()
                {
                    constructType = ConstructType.Arm,
                    animateType = AnimateType.Limb,
                    editType = EditType.Arm
                }
            },
            {
                SaveType.Leg, new PrefabTypes()
                {
                    constructType = ConstructType.Leg,
                    animateType = AnimateType.Leg,
                    editType = EditType.Leg
                }
            }
        };
        #endregion

        #region Properties
        private bool IsLimb
        {
            get
            {
                return type == SaveType.Arm || type == SaveType.Leg;
            }
        }
        #endregion

        #region Methods
        private void OnEnable()
        {
            target = new SerializedObject(this);

            _models = target.FindProperty("models");
            _type = target.FindProperty("type");
            _database = target.FindProperty("database");
            _outputDirectory = target.FindProperty("outputDirectory");
            _moveTool = target.FindProperty("moveToolPrefab");
        }
        private void OnGUI()
        {
            target.Update();

            EditorGUILayout.PropertyField(_models, new GUIContent("Models"), true);

            EditorGUILayout.PropertyField(_type);
            if (IsLimb)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_moveTool);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(_database);
            EditorGUILayout.PropertyField(_outputDirectory);

            EditorGUILayout.Space();

            if (!database || string.IsNullOrEmpty(outputDirectory) || (IsLimb && !moveToolPrefab))
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Setup Body Parts"))
            {
                foreach (GameObject model in models)
                {
                    Setup(model);
                }
            }
            GUI.enabled = true;

            target.ApplyModifiedProperties();
        }

        public void Setup(GameObject model)
        {
            string bodyPartName = model.name;

            Transform root = new GameObject(bodyPartName).transform;
            GameObject copy = Instantiate(model, root);
            copy.name = bodyPartName;

            // 0. Prepare
            GameObject prepared = null;
            if (IsLimb || type == SaveType.Tail)
            {
                prepared = copy; // Limbs should already be prepared.
            }
            else
            {
                prepared = Prepare(copy, root);
            }

            // 1. Add To Database
            BodyPart bodyPart = AddToDatabase(type, database);

            // 2. Convert...
            // 2.1. ...to Constructible
            BodyPartConstructor constructible = ConvertToConstructible(prepared, bodyPart, typeMapping[type].constructType);
            constructible.transform.SetParent(root);
            constructible.name = $"{bodyPartName} (C)";

            // 2.2. ...to Animatable
            BodyPartConstructor animatable = Instantiate(constructible, root); // Copy constructible
            animatable.name = $"{bodyPartName} (A)";
            ConvertToAnimatable(animatable, typeMapping[type].animateType);

            // 2.3. ...to Editable
            BodyPartConstructor editable = Instantiate(animatable, root); // Copy animatable
            editable.name = $"{bodyPartName} (E)";
            ConvertToEditable(editable, typeMapping[type].editType, moveToolPrefab);

            // 3. Save
            Save(root, bodyPart, outputDirectory, type, database);

            // 4. Clean Up
            DestroyImmediate(root.gameObject);
        }

        public GameObject Prepare(GameObject model, Transform parent)
        {
            GameObject bodyPart = new GameObject(model.name);
            bodyPart.transform.SetParent(parent);

            model.transform.SetParent(bodyPart.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
            model.name = "Model";

            SkinnedMeshRenderer smr = model.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                smr.updateWhenOffscreen = true;
            }

            return bodyPart;
        }
        public BodyPart AddToDatabase(SaveType type, Database database)
        {
            BodyPart bodyPart = ScriptableObject.CreateInstance(type.ToString()) as BodyPart;
            database.Objects.Add(Guid.NewGuid().ToString().Substring(0, 8), bodyPart);

            return bodyPart;
        }
        public BodyPartConstructor ConvertToConstructible(GameObject bodyPartGO, BodyPart bodyPart, ConstructType constructType)
        {
            // Type
            BodyPartConstructor constructor = null;
            switch (constructType)
            {
                case ConstructType.Arm:
                    constructor = bodyPartGO.AddComponent<ArmConstructor>();
                    break;
                case ConstructType.Leg:
                    constructor = bodyPartGO.AddComponent<LegConstructor>();
                    break;
                case ConstructType.Hand:
                    constructor = bodyPartGO.AddComponent<ExtremityConstructor>();
                    break;
                case ConstructType.Foot:
                    constructor = bodyPartGO.AddComponent<FootConstructor>();
                    break;
                case ConstructType.Mouth:
                    constructor = bodyPartGO.AddComponent<MouthConstructor>();
                    break;
                default:
                    constructor = bodyPartGO.AddComponent<BodyPartConstructor>();
                    break;
            }
            constructor.BodyPart = bodyPart;

            // Model
            Transform model = bodyPartGO.transform.GetChild(0);
            constructor.Model = model;

            // Stretch Dirs
            SkinnedMeshRenderer smr = model.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                for (int i = 0; i < smr.sharedMesh.blendShapeCount; ++i)
                {
                    string stretchAxis = smr.sharedMesh.GetBlendShapeName(i);
                    switch (stretchAxis.Substring(1)) // Ignore the direction by excluding the sign of the axis.
                    {
                        case "X":
                            constructor.AddStretchIndex(BodyPartConstructor.StretchAxis.X, stretchAxis.StartsWith("+"), i);
                            bodyPart.Transformations |= Transformation.StretchX;
                            break;
                        case "Y":
                            constructor.AddStretchIndex(BodyPartConstructor.StretchAxis.Y, stretchAxis.StartsWith("+"), i);
                            bodyPart.Transformations |= Transformation.StretchY;
                            break;
                        case "Z":
                            constructor.AddStretchIndex(BodyPartConstructor.StretchAxis.Z, stretchAxis.StartsWith("+"), i);
                            bodyPart.Transformations |= Transformation.StretchZ;
                            break;
                    }
                }
            }

            // Limbs
            if (constructor is LimbConstructor)
            {
                LimbConstructor limbConstructor = constructor as LimbConstructor;
                limbConstructor.Root = bodyPartGO.transform.GetChild(1);

                limbConstructor.Bones = new Transform[limbConstructor.Root.childCount];
                for (int i = 0; i < limbConstructor.Root.childCount; ++i)
                {
                    limbConstructor.Bones[i] = limbConstructor.Root.GetChild(i);
                }

                limbConstructor.Extremity = new GameObject("Extremity").transform;
                limbConstructor.Extremity.SetParent(limbConstructor.Bones[limbConstructor.Bones.Length - 1]);
            }

            return constructor;
        }
        public BodyPartAnimator ConvertToAnimatable(BodyPartConstructor constructor, AnimateType animateType)
        {
            // Type
            BodyPartAnimator animator = null;
            switch (animateType)
            {
                case AnimateType.Limb:
                    animator = constructor.gameObject.AddComponent<LimbAnimator>();
                    break;
                case AnimateType.Leg:
                    animator = constructor.gameObject.AddComponent<LegAnimator>();
                    break;
                default:
                    animator = constructor.gameObject.AddComponent<BodyPartAnimator>();
                    break;
            }

            return animator;
        }
        public BodyPartEditor ConvertToEditable(BodyPartConstructor constructor, EditType editType, GameObject moveToolPrefab)
        {
            BodyPartEditor editor = null;
            switch (editType)
            {
                case EditType.Arm:
                    editor = constructor.gameObject.AddComponent<ArmEditor>();
                    break;
                case EditType.Leg:
                    editor = constructor.gameObject.AddComponent<LegEditor>();
                    break;
                case EditType.Extremity:
                    editor = constructor.gameObject.AddComponent<ExtremityEditor>();
                    break;
                case EditType.Hand:
                    editor = constructor.gameObject.AddComponent<HandEditor>();
                    break;
                case EditType.Foot:
                    editor = constructor.gameObject.AddComponent<FootEditor>();
                    break;
                default:
                    editor = constructor.gameObject.AddComponent<BodyPartEditor>();
                    break;
            }

            constructor.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Body Parts"));
            constructor.Model.tag = "Body Part";

            Rigidbody rb = constructor.gameObject.GetOrAddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            foreach (Renderer renderer in constructor.Model.GetComponentsInChildren<Renderer>())
            {
                renderer.gameObject.GetOrAddComponent<MeshCollider>();
            }

            constructor.gameObject.GetOrAddComponent<Hover>();
            constructor.gameObject.GetOrAddComponent<Scroll>();

            Drag drag = constructor.gameObject.GetOrAddComponent<Drag>();
            drag.mousePlaneAlignment = Drag.MousePlaneAlignment.ToWorldDirection;
            drag.updatePlaneOnPress = true;
            drag.isBounded = false;
            drag.controlDrag = false;
            drag.useOffsetPosition = false;

            constructor.gameObject.GetOrAddComponent<Click>();
            Select select = constructor.gameObject.GetOrAddComponent<Select>();
            select.IgnoredTags = new string[] { "Tool" };

            Outline outline = constructor.Model.gameObject.AddComponent<Outline>();
            outline.OutlineWidth = 5f;
            outline.IncludeSubMeshes = true;
            outline.enabled = false;

            // Limbs
            if (constructor is LimbConstructor)
            {
                LimbConstructor limbConstructor = constructor as LimbConstructor;
                LimbEditor limbEditor = editor as LimbEditor;

                for (int i = 0; i < limbConstructor.Bones.Length; i++)
                {
                    Transform bone = limbConstructor.Bones[i];

                    // Controls
                    if (i > 0)
                    {
                        Rigidbody boneRB = bone.gameObject.AddComponent<Rigidbody>();
                        boneRB.useGravity = false;
                        boneRB.isKinematic = true;

                        bone.gameObject.AddComponent<Scroll>();
                        bone.gameObject.AddComponent<Hover>();

                        Drag boneDrag = bone.gameObject.AddComponent<Drag>();
                        boneDrag.boundsShape = Drag.BoundsShape.Cylinder;
                        boneDrag.updatePlaneOnPress = true;
                        if (i < limbConstructor.Bones.Length - 1 || limbConstructor is ArmConstructor)
                        {
                            boneDrag.mousePlaneAlignment = Drag.MousePlaneAlignment.WithCamera;
                        }
                        else
                        {
                            boneDrag.mousePlaneAlignment = Drag.MousePlaneAlignment.ToWorldDirection;
                            boneDrag.worldDirection = Vector3.up;
                            boneDrag.cylinderHeight = 0;
                        }
                    }

                    // Constraints
                    if (i < limbConstructor.Bones.Length - 1)
                    {
                        LookAtConstraint boneConstraint = bone.gameObject.AddComponent<LookAtConstraint>();
                        Quaternion offset = GetRotationOffset(limbConstructor.Bones[i + 1], limbConstructor.Bones[i]);

                        boneConstraint.AddSource(new ConstraintSource()
                        {
                            sourceTransform = limbConstructor.Bones[i + 1],
                            weight = 1f
                        });
                        boneConstraint.rotationAtRest = bone.localEulerAngles;
                        boneConstraint.rotationOffset = offset.eulerAngles;
                        boneConstraint.constraintActive = true;
                        boneConstraint.locked = true;
                    }
                    else
                    {
                        RotationConstraint extremityConstraint = bone.gameObject.AddComponent<RotationConstraint>();
                        Quaternion offset = GetRotationOffset(bone, limbConstructor.Bones[i - 1]);

                        extremityConstraint.AddSource(new ConstraintSource()
                        {
                            sourceTransform = limbConstructor.Bones[i - 1],
                            weight = 1f
                        });
                        extremityConstraint.rotationAtRest = bone.localEulerAngles;
                        extremityConstraint.rotationOffset = offset.eulerAngles;
                        extremityConstraint.constraintActive = true;
                        //extremityConstraint.locked = true; // Must lock manually - bug with Unity's RotationConstraint
                    }

                    GameObject moveTool = Instantiate(moveToolPrefab, bone);
                    moveTool.tag = "Tool";
                }
            }

            return editor;
        }
        public void Save(Transform bodyPartRoot, BodyPart bodyPart, string outputDirectory, SaveType type, Database database)
        {
            string bodyPartName = bodyPartRoot.name;

            if (AssetDatabase.IsValidFolder(Path.Combine(outputDirectory, bodyPartName)))
            {
                Debug.Log($"{bodyPartName} already exists!");
                return;
            }

            AssetDatabase.CreateFolder(outputDirectory, bodyPartName);
            string bodyPartDir = Path.Combine(outputDirectory, $"{bodyPartName}");

            // Item
            AssetDatabase.CreateAsset(bodyPart, Path.Combine(bodyPartDir, $"{bodyPartName}.asset"));

            // Prefabs
            AssetDatabase.CreateFolder(bodyPartDir, "Prefabs");
            foreach (Transform prefabT in bodyPartRoot)
            {
                string prefabName = prefabT.name;

                string prefabPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(bodyPartDir, "Prefabs", $"{prefabName}.prefab"));
                GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(prefabT.gameObject, prefabPath, InteractionMode.UserAction);

                string prefabType = prefabName.Substring(prefabName.Length - 2, 1);
                switch (prefabType)
                {
                    case "C":
                        bodyPart.Prefab.constructible = prefab;
                        break;
                    case "A":
                        bodyPart.Prefab.animatable = prefab;
                        break;
                    case "E":
                        bodyPart.Prefab.editable = prefab;
                        break;
                }
            }
        }

        [MenuItem("Creature Creator/Body Part Prefabs")]
        public static void ShowWindow()
        {
            GetWindow<BPPrefabsWindow>("Body Part Prefabs");
        }

        private Quaternion GetRotationOffset(Transform target, Transform source)
        {
            Quaternion lookAt = Quaternion.LookRotation(target.position - source.position);
            Quaternion offset = Quaternion.Inverse(lookAt) * source.rotation;
            return offset;
        }
        #endregion

        #region Inner Classes
        [Serializable]
        private class PrefabTypes
        {
            public ConstructType constructType = ConstructType.Default;
            public AnimateType animateType = AnimateType.Default;
            public EditType editType = EditType.Default;
        }
        #endregion

        #region Enums
        [Serializable]
        public enum ConstructType
        {
            Default,
            Arm,
            Leg,
            Hand,
            Foot,
            Mouth
        }

        [Serializable]
        public enum AnimateType
        {
            Default,
            Limb,
            Leg
        }

        [Serializable]
        public enum EditType
        {
            Default,
            Arm,
            Leg,
            Extremity,
            Hand,
            Foot
        }

        [Serializable]
        public enum SaveType
        {
            Detail,
            Tail,
            Weapon,
            Wing,
            Foot,
            Hand,
            Ear,
            Eye,
            Mouth,
            Nose,
            Arm,
            Leg
        }
        #endregion
    }
}