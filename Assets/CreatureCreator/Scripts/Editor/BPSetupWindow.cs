// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.Animations;
using System.Collections.Generic;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BPSetupWindow : EditorWindow
    {
        #region Fields
        [SerializeField] private SaveType type;
        [SerializeField] private GameObject moveToolPrefab;
        [SerializeField] private MinMax minMaxHealth = new MinMax(5, 15);
        [SerializeField] private int maxComplexity = 10;
        [SerializeField] private Database database;
        [SerializeField] private string outputDirectory;
        [SerializeField] private GameObject[] models;

        private SerializedProperty _type, _moveTool, _database, _outputDirectory, _minMaxHealth, _maxComplexity, _models;
        private SerializedObject target;
        private bool showSettings = true;
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

            _type = target.FindProperty("type");
            _moveTool = target.FindProperty("moveToolPrefab");
            _minMaxHealth = target.FindProperty("minMaxHealth");
            _maxComplexity = target.FindProperty("maxComplexity");
            _database = target.FindProperty("database");
            _outputDirectory = target.FindProperty("outputDirectory");
            _models = target.FindProperty("models");
        }
        private void OnGUI()
        {
            target.Update();

            showSettings = EditorGUILayout.Foldout(showSettings, "Settings", true, EditorStyles.foldoutHeader);
            if (showSettings)
            {
                EditorGUILayout.PropertyField(_type);
                if (IsLimb)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_moveTool);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.PropertyField(_minMaxHealth);
                EditorGUILayout.PropertyField(_maxComplexity);
                EditorGUILayout.PropertyField(_database);
                EditorGUILayout.PropertyField(_outputDirectory);
                EditorGUILayout.Space();
            }
            EditorGUILayout.PropertyField(_models, new GUIContent("Models"), true);

            if (!database || string.IsNullOrEmpty(outputDirectory) || (IsLimb && !moveToolPrefab) || maxComplexity == 0f || minMaxHealth.min == 0f || minMaxHealth.max == 0f)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Setup Body Part(s)"))
            {
                foreach (GameObject model in models)
                {
                    Setup(model);
                }
            }
            if (GUILayout.Button("Update"))
            {
                UpdateMissing();
                UpdateStats();
                //UpdateComponentTypes();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();

            target.ApplyModifiedProperties();
        }

        public void Setup(GameObject model)
        {
            string bodyPartName = model.name;

            Transform root = new GameObject(bodyPartName).transform;
            GameObject copy = PrefabUtility.InstantiatePrefab(model, root) as GameObject;
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
            string bodyPartDir = Path.Combine(outputDirectory, bodyPartName);
            if (!AssetDatabase.IsValidFolder(bodyPartDir))
            {
                AssetDatabase.CreateFolder(outputDirectory, bodyPartName);

                // Item
                AssetDatabase.CreateAsset(bodyPart, Path.Combine(bodyPartDir, $"{bodyPartName}.asset"));

                // Prefabs
                AssetDatabase.CreateFolder(bodyPartDir, "Prefabs");
            }

            // 2. Convert...
            // 2.1. ...to Constructible
            BodyPartConstructor constructible = ConvertToConstructible(prepared, bodyPart);
            constructible.transform.SetParent(root);
            constructible.name = $"{bodyPartName} (C)";
            BodyPartConstructor constructiblePrefab = Save(bodyPartDir, constructible.gameObject, bodyPart, outputDirectory, type, database).GetComponent<BodyPartConstructor>();

            // 2.2. ...to Animatable
            BodyPartConstructor animatable = PrefabUtility.InstantiatePrefab(constructiblePrefab, root) as BodyPartConstructor; // Copy constructible
            animatable.name = $"{bodyPartName} (A)";
            ConvertToAnimatable(animatable);
            BodyPartConstructor animatablePrefab = Save(bodyPartDir, animatable.gameObject, bodyPart, outputDirectory, type, database).GetComponent<BodyPartConstructor>();

            // 2.3. ...to Editable
            BodyPartConstructor editable = PrefabUtility.InstantiatePrefab(animatablePrefab, root) as BodyPartConstructor; // Copy animatable
            editable.name = $"{bodyPartName} (E)";
            ConvertToEditable(editable, moveToolPrefab);
            Save(bodyPartDir, editable.gameObject, bodyPart, outputDirectory, type, database);

            // 3. Clean Up
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
            BodyPart bodyPart = ScriptableObject.CreateInstance($"DanielLochner.Assets.CreatureCreator.{type}") as BodyPart;
            database.Objects.Add(Guid.NewGuid().ToString().Substring(0, 8), bodyPart);

            return bodyPart;
        }
        public BodyPartConstructor ConvertToConstructible(GameObject bodyPartGO, BodyPart bodyPart)
        {
            // Type
            BodyPartConstructor constructor = bodyPartGO.AddComponent(GetConstructorType(bodyPart.GetType())) as BodyPartConstructor;
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
                limbConstructor.Extremity.SetParent(limbConstructor.Bones[limbConstructor.Bones.Length - 1], false);
            }

            return constructor;
        }
        public BodyPartAnimator ConvertToAnimatable(BodyPartConstructor constructor)
        {
            // Type
            BodyPartAnimator animator = constructor.gameObject.AddComponent(GetAnimatorType(constructor.BodyPart.GetType())) as BodyPartAnimator;
            return animator;
        }
        public BodyPartEditor ConvertToEditable(BodyPartConstructor constructor, GameObject moveToolPrefab)
        {
            BodyPartEditor editor = constructor.gameObject.AddComponent(GetEditorType(constructor.BodyPart.GetType())) as BodyPartEditor;
            constructor.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Body Parts"));
            constructor.Model.tag = "Body Part";

            Rigidbody rb = constructor.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            foreach (Renderer renderer in constructor.Model.GetComponentsInChildren<Renderer>())
            {
                MeshCollider collider = renderer.gameObject.AddComponent<MeshCollider>();
                if (renderer is MeshRenderer)
                {
                    collider.sharedMesh = renderer.GetComponent<MeshFilter>().sharedMesh;
                }
                else if (renderer is SkinnedMeshRenderer)
                {
                    collider.sharedMesh = (renderer as SkinnedMeshRenderer).sharedMesh;
                }
            }

            constructor.gameObject.AddComponent<Hover>();
            constructor.gameObject.AddComponent<Scroll>();

            Drag lDrag = constructor.gameObject.AddComponent<Drag>();
            lDrag.mousePlaneAlignment = Drag.MousePlaneAlignment.ToWorldDirection;
            lDrag.updatePlaneOnPress = true;
            lDrag.isBounded = false;
            lDrag.controlDrag = false;
            lDrag.useOffsetPosition = false;

            Drag rDrag = constructor.gameObject.AddComponent<Drag>();
            rDrag.mouseButton = 1;
            rDrag.mousePlaneAlignment = Drag.MousePlaneAlignment.WithCamera;
            rDrag.boundsShape = Drag.BoundsShape.Sphere;
            rDrag.sphereRadius = 0.1f;
            rDrag.updatePlaneOnPress = true;

            constructor.gameObject.AddComponent<Click>();
            Select select = constructor.gameObject.AddComponent<Select>();
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
                        if (i < limbConstructor.Bones.Length - 1 || limbConstructor.Limb is Arm)
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
                        Quaternion offset = QuaternionUtility.GetRotationOffset(limbConstructor.Bones[i + 1], limbConstructor.Bones[i]);

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
                        Quaternion offset = QuaternionUtility.GetRotationOffset(bone, limbConstructor.Bones[i - 1]);

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
        public GameObject Save(string bodyPartDir, GameObject bodyPartPrefab, BodyPart bodyPart, string outputDirectory, SaveType type, Database database)
        {
            string prefabName = bodyPartPrefab.name;

            string prefabPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(bodyPartDir, "Prefabs", $"{prefabName}.prefab"));
            GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(bodyPartPrefab, prefabPath, InteractionMode.UserAction);

            string prefabType = prefabName.Substring(prefabName.Length - 2, 1);
            switch (prefabType)
            {
                case "C":
                    return bodyPart.Prefab.constructible = prefab;
                case "A":
                    return bodyPart.Prefab.animatable = prefab;
                case "E":
                    return bodyPart.Prefab.editable = prefab;
            }
            return null;
        }

        public void UpdateMissing()
        {
            List<string> missing = new List<string>();
            foreach (var bodyPart in database.Objects)
            {
                if (bodyPart.Value == null)
                {
                    missing.Add(bodyPart.Key);
                }
            }

            foreach (string id in missing)
            {
                database.Objects.Remove(id);
            }

            EditorUtility.SetDirty(database);
        }
        public void UpdateStats()
        {
            MinMax minMaxWeight = new MinMax(Mathf.Infinity, Mathf.NegativeInfinity);
            foreach (var obj in database.Objects.Values)
            {
                BodyPart bodyPart = obj as BodyPart;

                float weight = bodyPart.Weight;
                if (weight > minMaxWeight.max)
                {
                    minMaxWeight.max = weight;
                }
                else
                if (weight < minMaxWeight.min)
                {
                    minMaxWeight.min = weight;
                }
            }

            foreach (var obj in database.Objects.Values)
            {
                BodyPart bodyPart = obj as BodyPart;

                // Transformations
                if (!(bodyPart is Limb))
                {
                    bodyPart.Transformations |= Transformation.Scale;
                }
                if (!(bodyPart is Limb || bodyPart is Extremity))
                {
                    bodyPart.Transformations |= Transformation.Pivot;
                    bodyPart.Transformations |= Transformation.PivotXY;
                }
                if (!(bodyPart is Foot))
                {
                    bodyPart.Transformations |= Transformation.Rotate;
                }

                // Complexity
                bodyPart.Complexity = Math.Min(bodyPart.BaseComplexity + bodyPart.Abilities.Count, maxComplexity);

                // Health
                float t = Mathf.InverseLerp(minMaxWeight.min, minMaxWeight.max, bodyPart.Weight);
                bodyPart.Health = (int)Mathf.Lerp(minMaxHealth.min, minMaxHealth.max, t);

                // Price
                int tValue = 0;
                if (bodyPart.Transformations.HasFlag(Transformation.Rotate) || bodyPart.Transformations.HasFlag(Transformation.Scale)) tValue++;
                if (bodyPart.Transformations.HasFlag(Transformation.Pivot) || bodyPart.Transformations.HasFlag(Transformation.PivotXY)) tValue++;
                if (bodyPart.Transformations.HasFlag(Transformation.StretchX)) tValue++;
                if (bodyPart.Transformations.HasFlag(Transformation.StretchY)) tValue++;
                if (bodyPart.Transformations.HasFlag(Transformation.StretchZ)) tValue++;

                int nonFunctionalValue = bodyPart.Appeal + tValue;
                int functionalValue = bodyPart.Health + bodyPart.Complexity;

                EditorUtility.SetDirty(bodyPart);
            }
        }
        public void UpdateComponentTypes()
        {
            foreach (var obj in database.Objects.Values)
            {
                BodyPart bodyPart = obj as BodyPart;

                // Editors
                Type expectedEditorType = GetEditorType(bodyPart.GetType());
                GameObject editable = bodyPart.GetPrefab(BodyPart.PrefabType.Editable);
                CheckTypeMismatch(editable.GetComponent<BodyPartEditor>(), expectedEditorType);
                
                // Animators
                Type expectedAnimatorType = GetAnimatorType(bodyPart.GetType());
                GameObject animatable = bodyPart.GetPrefab(BodyPart.PrefabType.Animatable);
                //CheckTypeMismatch(editable.GetComponent<BodyPartAnimator>(), expectedAnimatorType);
                CheckTypeMismatch(animatable.GetComponent<BodyPartAnimator>(), expectedAnimatorType);

                // Constructors
                Type expectedConstructorType = GetConstructorType(bodyPart.GetType());
                GameObject constructible = bodyPart.GetPrefab(BodyPart.PrefabType.Constructible);
                //CheckTypeMismatch(editable.GetComponent<BodyPartConstructor>(), expectedConstructorType);
                //CheckTypeMismatch(animatable.GetComponent<BodyPartConstructor>(), expectedConstructorType);
                CheckTypeMismatch(constructible.GetComponent<BodyPartConstructor>(), expectedConstructorType);

                EditorUtility.SetDirty(bodyPart);
            }
        }
        public void CheckTypeMismatch(Component component, Type expectedType)
        {
            if (component != null)
            {
                component.MoveToTop();
                if (component.GetType() != expectedType)
                {
                    Debug.Log($"Type mismatch for '{component.name}'! ({component.GetType().Name} -> {expectedType.Name})");

                    Component newComponent = component.gameObject.AddComponent(expectedType);
                    component.CopyValues();
                    newComponent.PasteValues();
                    DestroyImmediate(component, true);
                    newComponent.MoveToTop();
                }
            }
            else
            {
                Debug.LogError("Component doesn't exist!");
            }
        }

        private Type GetEditorType(Type type)
        {
            switch (type.Name)
            {
                case "Arm":
                    return typeof(LimbEditor);
                case "Leg":
                    return typeof(LegEditor);
                case "Hand":
                    return typeof(HandEditor);
                case "Foot":
                    return typeof(FootEditor);
            }
            return typeof(BodyPartEditor);
        }
        private Type GetAnimatorType(Type type)
        {
            switch (type.Name)
            {
                case "Eye":
                    return typeof(EyeAnimator);
                case "Arm":
                    return typeof(ArmAnimator);
                case "Leg":
                    return typeof(LegAnimator);
                case "Mouth":
                    return typeof(MouthAnimator);
                case "Tail":
                    return typeof(TailAnimator);
                case "Wing":
                    return typeof(WingAnimator);
                case "Hand":
                case "Foot":
                    return typeof(ExtremityAnimator);
            }
            return typeof(BodyPartAnimator);
        }
        private Type GetConstructorType(Type type)
        {
            switch (type.Name)
            {
                case "Arm":
                    return typeof(LimbConstructor);
                case "Leg":
                    return typeof(LegConstructor);
                case "Hand":
                    return typeof(ExtremityConstructor);
                case "Foot":
                    return typeof(FootConstructor);
                case "Wing":
                    return typeof(WingConstructor);
            }
            return typeof(BodyPartConstructor);
        }

        [MenuItem("Window/Creature Creator/Body Part Setup")]
        public static void ShowWindow()
        {
            GetWindow<BPSetupWindow>("Body Part Setup");
        }

        #region Limbs Setup
        public static List<SerializableTransform> transforms = new List<SerializableTransform>();

        public void LimbEditor_SetupAll()
        {
            foreach (var obj in database.Objects)
            {
                BodyPart bp = obj.Value as BodyPart;
                if (bp is Limb)
                {
                    LimbEditor_Setup(bp);
                }
            }
        }
        public void LimbEditor_Setup(BodyPart bp)
        {
            LimbEditor limbEditor = bp.GetPrefab(BodyPart.PrefabType.Editable).GetComponent<LimbEditor>();

            // Remove constraints
            LimbEditor_RemoveConstraints(limbEditor);

            // Copy serializable transfroms from constructible to animatable and editable
            LimbEditor_Copy(bp.GetPrefab(BodyPart.PrefabType.Constructible).transform);
            LimbEditor_Paste(bp.GetPrefab(BodyPart.PrefabType.Animatable).transform);
            LimbEditor_Paste(limbEditor.transform);

            // Setup constraints
            LimbEditor_SetupConstraints(limbEditor);
            foreach (LookAtConstraint c in limbEditor.GetComponentsInChildren<LookAtConstraint>(true))
            {
                c.useUpObject = false;
            }

            // Set dirty
            EditorUtility.SetDirty(bp.GetPrefab(BodyPart.PrefabType.Animatable));
            EditorUtility.SetDirty(bp.GetPrefab(BodyPart.PrefabType.Constructible));
            EditorUtility.SetDirty(bp.GetPrefab(BodyPart.PrefabType.Editable));
        }

        public void LimbEditor_Copy(Transform copyFromT)
        {
            transforms.Clear();
            foreach (var t in copyFromT.GetComponentsInChildren<Transform>(true))
            {
                transforms.Add(new SerializableTransform(t, copyFromT));
            }
        }
        public void LimbEditor_Paste(Transform pasteToT)
        {
            int counter = 0;
            foreach (var t in pasteToT.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == "Move") continue;
                t.Set(transforms[counter], pasteToT);
                counter++;
            }
            EditorUtility.SetDirty(pasteToT.gameObject);
        }
        public void LimbEditor_RemoveConstraints(LimbEditor limbEditor)
        {
            LookAtConstraint[] lac = limbEditor.GetComponentsInChildren<LookAtConstraint>(true);
            for (int i = 0; i < lac.Length; i++)
            {
                DestroyImmediate(lac[i], true);
            }
            DestroyImmediate(limbEditor.GetComponentInChildren<RotationConstraint>(true), true);
        }
        public void LimbEditor_SetupConstraints(LimbEditor limbEditor)
        {
            limbEditor.BodyPartConstructor = limbEditor.GetComponent<LimbConstructor>();

            // LookAt
            for (int i = 0; i < limbEditor.LimbConstructor.Bones.Length - 1; i++)
            {
                Transform from = limbEditor.LimbConstructor.Bones[i];
                Transform to = limbEditor.LimbConstructor.Bones[i + 1];

                Quaternion look = Quaternion.LookRotation(to.transform.position - from.transform.position);
                Quaternion diff = Quaternion.Inverse(look) * from.transform.rotation;

                LookAtConstraint lookAtConstraint = from.gameObject.GetOrAddComponent<LookAtConstraint>();
                lookAtConstraint.SetSources(new List<ConstraintSource>()
                {
                    new ConstraintSource()
                    {
                        sourceTransform = to,
                        weight = 1f
                    }
                });
                lookAtConstraint.rotationAtRest = Vector3.zero;
                lookAtConstraint.rotationOffset = diff.eulerAngles;
                lookAtConstraint.constraintActive = true;
                lookAtConstraint.locked = true;
                lookAtConstraint.enabled = true;
            }

            // Rotation
            RotationConstraint rotationConstraint = limbEditor.LimbConstructor.Bones[limbEditor.LimbConstructor.Bones.Length - 1].gameObject.GetOrAddComponent<RotationConstraint>();
            rotationConstraint.SetSources(new List<ConstraintSource>()
            {
                new ConstraintSource()
                {
                    sourceTransform = limbEditor.LimbConstructor.Bones[limbEditor.LimbConstructor.Bones.Length - 2],
                    weight = 1f
                }
            });
            rotationConstraint.constraintActive = true;
        }
        public void LimbEditor_EnableRotationConstraint(LimbEditor limbEditor)
        {
            limbEditor.GetComponentInChildren<RotationConstraint>(true).locked = true;
        }
        #endregion
        #endregion

        #region Enums
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