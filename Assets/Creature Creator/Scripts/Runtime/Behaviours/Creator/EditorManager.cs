// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RotaryHeart.Lib.SerializableDictionary;
using UnityFBXExporter;
using ProfanityDetector;
using SimpleFileBrowser;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets.CreatureCreator
{
    public class EditorManager : MonoBehaviourSingleton<EditorManager>
    {
        #region Fields
        [SerializeField] private bool unlockAllBodyParts;
        [SerializeField] private bool unlockAllPatterns;
        [SerializeField] private SecretKey creatureEncryptionKey;

        [Header("General")]
        [SerializeField] private Platform platform;
        [SerializeField] private AudioSource editorAudioSource;
        [SerializeField] private CanvasGroup editorCanvasGroup;
        [SerializeField] private CanvasGroup paginationCanvasGroup;

        [Header("Build")]
        [SerializeField] private Menu buildMenu;
        [SerializeField] private BodyPartSettingsMenu bodyPartSettingsMenu;
        [SerializeField] private BodyPartUI bodyPartUIPrefab;
        [SerializeField] private TextMeshProUGUI cashText;
        [SerializeField] private TextMeshProUGUI complexityText;
        [SerializeField] private TextMeshProUGUI heightText;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private TextMeshProUGUI dietText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI bonesText;
        [SerializeField] private TextMeshProUGUI bodyPartsText;
        [SerializeField] private TextMeshProUGUI abilitiesText;
        [SerializeField] private Toggle bodyPartsToggle;
        [SerializeField] private Toggle abilitiesToggle;
        [SerializeField] private Animator cashWarningAnimator;
        [SerializeField] private Animator complexityWarningAnimator;
        [SerializeField] private AudioClip whooshAudioClip;
        [SerializeField] private AudioClip errorAudioClip;
        [SerializeField] private AudioClip createAudioClip;
        [SerializeField] private RectTransform bodyPartsRT;
        [SerializeField] private BodyPartGrids bodyPartGrids;

        [Header("Play")]
        [SerializeField] private Menu playMenu;
        [SerializeField] private CreatureInformationMenu informationMenu;

        [Header("Paint")]
        [SerializeField] private Menu paintMenu;
        [SerializeField] private PatternSettingsMenu patternSettingsMenu;
        [SerializeField] private ColourSettingsMenu colourSettingsMenu;
        [SerializeField] private PatternUI patternUIPrefab;
        [SerializeField] private Material patternMaterial;
        [SerializeField] private ColourPicker primaryColourPicker;
        [SerializeField] private ColourPicker secondaryColourPicker;
        [SerializeField] private ToggleGroup patternsToggleGroup;
        [SerializeField] private RectTransform patternsRT;
        [SerializeField] private TextMeshProUGUI noColoursText;

        [Header("Options")]
        [SerializeField] private SimpleSideMenu optionsSideMenu;
        [SerializeField] private CanvasGroup optionsCanvasGroup;
        [SerializeField] private CreatureUI creatureUIPrefab;
        [SerializeField] private TMP_InputField creatureNameText;
        [SerializeField] private ToggleGroup creaturesToggleGroup;
        [SerializeField] private RectTransform creaturesRT;

        [Header("Testing")]
        [SerializeField, Button("UnlockRandomBodyPart")] private bool unlockRandomBodyPart;
        [SerializeField, Button("UnlockRandomPattern")] private bool unlockRandomPattern;

        private List<CreatureUI> creaturesUI = new List<CreatureUI>();
        private List<PatternUI> patternsUI = new List<PatternUI>();
        private string creaturesDirectory = null; // null because Application.persistentDataPath cannot be called during serialization.
        private bool isVisible = true, isEditing = true;
        #endregion

        #region Properties
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible == value) return;
                isVisible = value;

                StartCoroutine(editorCanvasGroup.Fade(isVisible, 0.25f));
            }
        }
        public bool IsEditing
        {
            get => isEditing;
            set
            {
                if (isEditing == value) return;
                isEditing = value;

                optionsSideMenu.Close();

                StartCoroutine(paginationCanvasGroup.Fade(isEditing, 0.25f));
                StartCoroutine(optionsCanvasGroup.Fade(isEditing, 0.25f));

                if (isEditing) UpdateLoadableCreatures();
            }
        }

        public bool IsBuilding => buildMenu.IsOpen;
        public bool IsPainting => paintMenu.IsOpen;
        public bool IsPlaying => playMenu.IsOpen;

        public CreaturePlayerLocal Creature => Player.Instance;
        #endregion

        #region Methods
        private void Update()
        {
            HandleKeyboardShortcuts();
        }

        #region Setup
        public void Setup()
        {
            SetupPlayer();
            SetupEditor();
        }
        public void SetupEditor()
        {
            // Build
            List<string> unlockedBodyParts = null;
            if (unlockAllBodyParts)
            {
                unlockedBodyParts = DatabaseManager.GetDatabase("Body Parts").Objects.Keys.ToList();
            }
            else
            {
                unlockedBodyParts = ProgressManager.Data.UnlockedBodyParts;
            }
            foreach (string bodyPartID in unlockedBodyParts)
            {
                if (!SettingsManager.Data.HiddenBodyParts.Contains(bodyPartID)) AddBodyPartUI(bodyPartID);
            }
            UpdateBodyPartTotals();
            bodyPartsToggle.onValueChanged.AddListener(delegate
            {
                // Create a dictionary of distinct body parts with their respective counts.
                Dictionary<string, int> distinctBodyParts = new Dictionary<string, int>();
                foreach (AttachedBodyPart attachedBodyPart in Creature.Constructor.Data.AttachedBodyParts)
                {
                    string bodyPartID = attachedBodyPart.bodyPartID;
                    if (distinctBodyParts.ContainsKey(bodyPartID))
                    {
                        distinctBodyParts[bodyPartID]++;
                    }
                    else
                    {
                        distinctBodyParts.Add(bodyPartID, 1);
                    }
                }

                // Create a list of (joinable) strings using the dictionary of distinct body parts.
                List<string> bodyPartTotals = new List<string>();
                foreach (KeyValuePair<string, int> distinctBodyPart in distinctBodyParts)
                {
                    BodyPart bodyPart = DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", distinctBodyPart.Key);
                    int bodyPartCount = distinctBodyPart.Value;

                    if (bodyPartCount > 1)
                    {
                        bodyPartTotals.Add($"{bodyPart} ({bodyPartCount})");
                    }
                    else
                    {
                        bodyPartTotals.Add(bodyPart.ToString());
                    }
                }

                int count = Creature.Constructor.Data.AttachedBodyParts.Count;
                string bodyParts = (bodyPartTotals.Count > 0) ? string.Join(", ", bodyPartTotals) : "None";

                bodyPartsText.text = $"<b>Body Parts:</b> [{(bodyPartsToggle.isOn ? bodyParts : count.ToString())}]";
            });
            abilitiesToggle.onValueChanged.AddListener(delegate
            {
                int count = Creature.Abilities.Abilities.Count;
                string abilities = (count > 0) ? string.Join(", ", (IEnumerable<Ability>)Creature.Abilities.Abilities) : "None";

                abilitiesText.text = $"<b>Abilities:</b> [{(abilitiesToggle.isOn ? abilities : count.ToString())}]";
            });

            // Paint
            patternMaterial = new Material(patternMaterial);
            List<string> unlockedPatterns = null;
            if (unlockAllPatterns)
            {
                unlockedPatterns = DatabaseManager.GetDatabase("Patterns").Objects.Keys.ToList();
            }
            else
            {
                unlockedPatterns = ProgressManager.Data.UnlockedPatterns;
            }
            foreach (string patternID in unlockedPatterns)
            {
                if (!SettingsManager.Data.HiddenBodyParts.Contains(patternID)) AddPatternUI(patternID);
            }
            primaryColourPicker.ClickUI.OnRightClick.AddListener(delegate
            {
                if (primaryColourPicker.Name.text.Contains("Override"))
                {
                    ConfirmationDialog.Confirm("Revert Colour", "Are you sure you want to revert to the body's primary colour?", onYes: (UnityAction)delegate
                    {
                        Creature.Editor.PaintedBodyPart.BodyPartConstructor.IsPrimaryOverridden = false;
                        SetPrimaryColourUI((Color)Creature.Constructor.Data.PrimaryColour, false);
                    });
                }
            });
            secondaryColourPicker.ClickUI.OnRightClick.AddListener(delegate
            {
                if (secondaryColourPicker.Name.text.Contains("Override"))
                {
                    ConfirmationDialog.Confirm("Revert Colour", "Are you sure you want to revert to the body's secondary colour?", onYes: (UnityAction)delegate
                    {
                        Creature.Editor.PaintedBodyPart.BodyPartConstructor.IsSecondaryOverridden = false;
                        SetSecondaryColourUI((Color)Creature.Constructor.Data.SecondaryColour, false);
                    });
                }
            });

            // Options
            creaturesDirectory = Path.Combine(Application.persistentDataPath, Application.version, "Creatures");
            if (!Directory.Exists(creaturesDirectory))
            {
                Directory.CreateDirectory(creaturesDirectory);
            }
            foreach (string creaturePath in Directory.GetFiles(creaturesDirectory))
            {
                CreatureData creatureData = SaveUtility.Load<CreatureData>(creaturePath, creatureEncryptionKey.Value);
                if (creatureData != null)
                {
                    AddCreatureUI(Path.GetFileNameWithoutExtension(creaturePath));
                }
            }
            UpdateLoadableCreatures();
        }
        public void SetupPlayer()
        {
            Creature.Setup();

            // Load preset/null creature (and defaults).
            List<CreatureData> presets = SettingsManager.Data.CreaturePresets;
            if (presets.Count > 0)
            {
                Creature.Editor.Load(presets[UnityEngine.Random.Range(0, presets.Count)]);
            }
            else
            {
                Creature.Editor.Load(null);
            }
            Creature.Editor.IsInteractable = true;
            Creature.Editor.IsEditing = true;
            Creature.Editor.Cash = ProgressManager.Data.Cash;
            Creature.Informer.Setup(informationMenu);
            Creature.Mover.Teleport(platform, true);
        }
        #endregion

        #region Modes
        public void TryBuild()
        {
            if (!buildMenu.IsOpen)
            {
                Build();
            }
        }
        public void TryPlay()
        {
            if (!playMenu.IsOpen)
            {
                Play();
            }
        }
        public void TryPaint()
        {
            if (!paintMenu.IsOpen)
            {
                Paint();
            }
        }

        public void Build()
        {
            // Editor
            buildMenu.Open();
            playMenu.Close();
            paintMenu.Close();
            SetMode();

            // Player
            Creature.Mover.IsMovable = false;
            Creature.Constructor.IsTextured = false;
            Creature.Editor.IsInteractable = true;
            Creature.Editor.IsEditing = true;
            Creature.Editor.IsDraggable = true;
            Creature.Editor.UseTemporaryOutline = false;
            Creature.Editor.Deselect();
            Creature.Animator.IsAnimated = false;
            Creature.Spawner.Despawn();

            SetCameraOffset(-1.5f);
        }
        public void Play()
        {
            // Editor
            buildMenu.Close();
            playMenu.Open();
            paintMenu.Close();
            SetMode();

            // Player
            Creature.Constructor.Recenter();
            Creature.Constructor.UpdateConfiguration();
            Creature.Collider.UpdateCollider();
            Creature.Mover.Teleport(Creature.Mover.Platform);
            Creature.Mover.IsMovable = true;
            Creature.Constructor.IsTextured = true;
            Creature.Editor.IsInteractable = false;
            Creature.Editor.IsEditing = false;
            Creature.Editor.IsDraggable = false;
            Creature.Editor.UseTemporaryOutline = false;
            Creature.Editor.Deselect();
            Creature.Animator.Velocity.Reset();
            Creature.Animator.IsAnimated = true;
            Creature.Informer.Capture();
            Creature.Collider.UpdateCollider();
            Creature.Spawner.Spawn();

            SetCameraOffset(0f);
        }
        public void Paint()
        {
            // Editor
            buildMenu.Close();
            playMenu.Close();
            paintMenu.Open();
            SetMode();

            // Player
            Creature.Mover.IsMovable = false;
            Creature.Constructor.IsTextured = true;
            Creature.Editor.IsInteractable = true;
            Creature.Editor.IsEditing = true;
            Creature.Editor.IsDraggable = false;
            Creature.Editor.UseTemporaryOutline = true;
            Creature.Editor.Deselect();
            Creature.Animator.IsAnimated = false;
            Creature.Spawner.Despawn();

            SetCameraOffset(1.5f);
        }

        private void SetCameraOffset(float x)
        {
            Creature.Camera.CameraOrbit.OffsetPosition = new Vector3(x, 2f, Creature.Camera.CameraOrbit.OffsetPosition.z);
        }
        private void SetMode()
        {
            editorAudioSource.PlayOneShot(whooshAudioClip);

            bodyPartSettingsMenu.Close();
            patternSettingsMenu.Close();
            colourSettingsMenu.Close();
        }
        #endregion

        #region Operations
        public void TrySave()
        {
            string savedCreatureName = creatureNameText.text;
            if (IsValidName(savedCreatureName))
            {
                Creature.Constructor.SetName(savedCreatureName);
                if (!IsPlaying){ // Update only when the configuration could have changed (i.e., when building or painting).
                    Creature.Constructor.UpdateConfiguration();
                }

                bool exists = creaturesUI.Find(x => x.name == savedCreatureName) != null;
                bool isLoaded = savedCreatureName == Creature.Editor.LoadedCreature;

                ConfirmOperation(() => Save(Creature.Constructor.Data), exists && !isLoaded, "Overwrite creature?", $"Are you sure you want to overwrite \"{savedCreatureName}\"?");
            }
        }
        public void Save(CreatureData creatureData)
        {
            CreatureUI creatureUI = creaturesUI.Find(x => x.name.Equals(creatureData.Name));
            if (creatureUI == null)
            {
                creatureUI = AddCreatureUI(creatureData.Name);
            }
            creatureUI.SelectToggle.SetIsOnWithoutNotify(true);

            string path = Path.Combine(creaturesDirectory, $"{creatureData.Name}.dat");
            SaveUtility.Save(path, creatureData, creatureEncryptionKey.Value);

            Creature.Editor.LoadedCreature = creatureData.Name;
            Creature.Editor.IsDirty = false;
        }
        public void TryLoad()
        {
            string creatureName = default;

            CreatureUI selectedCreatureUI = creaturesUI.Find(x => x.SelectToggle.isOn);
            if (selectedCreatureUI != null)
            {
                creatureName = selectedCreatureUI.name;
            }
            else return;

            CreatureData creatureData = SaveUtility.Load<CreatureData>(Path.Combine(creaturesDirectory, $"{creatureName}.dat"), creatureEncryptionKey.Value);
            ConfirmUnsavedChanges(() => Load(creatureData), "loading");
        }
        public void Load(CreatureData creatureData)
        {
            Creature.Editor.Load(creatureData);

            if (IsBuilding)
            {
                Build();
            }
            else
            if (IsPlaying)
            {
                Play();
            }
            else
            if (IsPainting)
            {
                Paint();
            }

            // Colour
            primaryColourPicker.SetColour(Creature.Constructor.Data.PrimaryColour, false);
            secondaryColourPicker.SetColour(Creature.Constructor.Data.SecondaryColour, false);

            // Pattern
            patternsToggleGroup.SetAllTogglesOff(false);
            if (!string.IsNullOrEmpty(Creature.Constructor.Data.PatternID))
            {
                patternsUI.Find(x => x.name.Equals(Creature.Constructor.Data.PatternID)).SelectToggle.SetIsOnWithoutNotify(true);
            }
            patternMaterial.SetColor("_PrimaryCol", primaryColourPicker.Colour);
            patternMaterial.SetColor("_SecondaryCol", secondaryColourPicker.Colour);
        }
        public void TryClear()
        {
            ConfirmUnsavedChanges(Clear, "clearing");
        }
        public void Clear()
        {
            Load(null);
        }
        public void TryImport()
        {
            ConfirmUnsavedChanges(Import, "importing");
        }
        public void Import()
        {
            FileBrowser.ShowLoadDialog(
                onSuccess: delegate (string[] paths)
                {
                    string creaturePath = paths[0];

                    CreatureData creatureData = SaveUtility.Load<CreatureData>(creaturePath);
                    if (creatureData != null && IsValidName(creatureData.Name))
                    {
                        Save(creatureData);

                        if (CanLoadCreature(creatureData, out string errorMessage))
                        {
                            Load(creatureData);
                        }
                        else
                        {
                            InformationDialog.Inform("Creature unavailable!", errorMessage);
                        }
                    }
                    else
                    {
                        InformationDialog.Inform("Invalid creature!", "An error occurred while reading this file.");
                    }
                },
                onCancel: null,
                pickMode: FileBrowser.PickMode.Files,
                initialPath: Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                title: "Import Creature (Data)",
                loadButtonText: "Import");
        }
        public void TryExport()
        {
            string exportedCreatureName = creatureNameText.text;
            if (IsValidName(exportedCreatureName))
            {
                Creature.Constructor.SetName(exportedCreatureName);
                if (!IsPlaying){
                    Creature.Constructor.UpdateConfiguration();
                }

                FileBrowser.ShowSaveDialog(
                    onSuccess: (path) => Export(Creature.Constructor.Data),
                    onCancel: null,
                    pickMode: FileBrowser.PickMode.Folders,
                    initialPath: Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    title: "Export Creature (Data, Screenshot and 3D Model)",
                    saveButtonText: "Export");
            }
        }
        public void Export(CreatureData creatureData)
        {
            string creaturePath = Path.Combine(FileBrowser.Result[0], creatureData.Name);
            if (!Directory.Exists(creaturePath))
            {
                Directory.CreateDirectory(creaturePath);
            }

            // Data
            SaveUtility.Save(Path.Combine(creaturePath, $"{creatureData.Name}.dat"), creatureData);

            // Screenshot
            Creature.Photographer.TakePhoto(1024, delegate(Texture2D photo)
            {
                File.WriteAllBytes(Path.Combine(creaturePath, $"{creatureData.Name}.png"), photo.EncodeToPNG());
            });

            // 3D Model
            //PerformOperation(delegate
            //{
            //    List<GameObject> tools = player.Creature.gameObject.FindChildrenWithTag("Tool");
            //    foreach (GameObject tool in tools)
            //    {
            //        tool.SetActive(false);
            //    }
            //    FBXExporter.ExportGameObjToFBX(player.Creature.Constructor.gameObject, Path.Combine(creaturePath, $"{creatureData.Name}.fbx"));
            //    foreach (GameObject tool in tools)
            //    {
            //        tool.SetActive(true);
            //    }
            //}, true);
            
            GameObject export = Creature.Cloner.Clone(creatureData).gameObject;
            export.SetLayerRecursively(LayerMask.NameToLayer("Export"));

            foreach (GameObject tool in export.FindChildrenWithTag("Tool"))
            {
                tool.SetActive(false);
            }
            FBXExporter.ExportGameObjToFBX(export, Path.Combine(creaturePath, $"{creatureData.Name}.fbx"));
            Destroy(export);
        }

        public bool CanLoadCreature(CreatureData creatureData, out string errorMessage)
        {
            // Load Conditions
            bool patternIsUnlocked = ProgressManager.Data.UnlockedPatterns.Contains(creatureData.PatternID) || string.IsNullOrEmpty(creatureData.PatternID);

            bool bodyPartsAreUnlocked = true;
            int totalCost = 0, totalComplexity = 0;
            foreach (AttachedBodyPart attachedBodyPart in creatureData.AttachedBodyParts)
            {
                BodyPart bodyPart = DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", attachedBodyPart.bodyPartID);
                if (bodyPart != null)
                {
                    totalCost += bodyPart.Price;
                    totalComplexity += bodyPart.Complexity;
                }
                else
                {
                    bodyPartsAreUnlocked = false;
                }

                if (!ProgressManager.Data.UnlockedBodyParts.Contains(attachedBodyPart.bodyPartID))
                {
                    bodyPartsAreUnlocked = false;
                }
            }
            totalComplexity += creatureData.Bones.Count;

            bool creatureIsTooComplicated = totalComplexity > Creature.Constructor.MaxComplexity;
            bool creatureIsTooExpensive = totalCost > ProgressManager.Data.Cash;

            // Error Message
            List<string> errors = new List<string>();
            if (creatureIsTooComplicated)
            {
                errors.Add($"is too complicated. ({totalComplexity}/{Creature.Constructor.MaxComplexity})");
            }
            if (creatureIsTooExpensive)
            {
                errors.Add($"is too expensive. ({totalCost}/{ProgressManager.Data.Cash})");
            }
            if (!patternIsUnlocked)
            {
                errors.Add($"uses a pattern that has not yet been unlocked.");
            }
            if (!bodyPartsAreUnlocked)
            {
                errors.Add($"uses body parts that have not yet been unlocked.");
            }

            errorMessage = $"\"{creatureData.Name}\" cannot be loaded because:\n";
            if (errors.Count > 0)
            {
                for (int i = 0; i < errors.Count; i++)
                {
                    errorMessage += $"{i + 1}. it {errors[i]}\n";
                }
            }

            return patternIsUnlocked && bodyPartsAreUnlocked && !creatureIsTooComplicated && !creatureIsTooExpensive;
        }
        public bool CanAddBodyPart(string bodyPartID)
        {
            BodyPart bodyPart = DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartID);

            bool tooComplicated = Creature.Constructor.Statistics.complexity + bodyPart.Complexity > Creature.Constructor.MaxComplexity;
            bool notEnoughCash = Creature.Editor.Cash < bodyPart.Price;

            if (notEnoughCash || tooComplicated)
            {
                editorAudioSource.PlayOneShot(errorAudioClip);

                if (notEnoughCash && cashWarningAnimator.CanTransitionTo("Warning"))
                {
                    cashWarningAnimator.SetTrigger("Warn");
                }
                if (tooComplicated && complexityWarningAnimator.CanTransitionTo("Warning"))
                {
                    complexityWarningAnimator.SetTrigger("Warn");
                }

                return false;
            }

            return true;
        }
        #endregion

        #region Unlocks
        public void UnlockPattern(string patternID)
        {
            if (ProgressManager.Data.UnlockedPatterns.Contains(patternID)) return;

            ProgressManager.Data.UnlockedPatterns.Add(patternID);

            Texture pattern = DatabaseManager.GetDatabaseEntry<Texture>("Patterns", patternID);
            Sprite icon = Sprite.Create(pattern as Texture2D, new Rect(0, 0, pattern.width, pattern.height), new Vector2(0.5f, 0.5f));
            string title = pattern.name;
            string description = $"You unlocked a new pattern! Click to view all. ({ProgressManager.Data.UnlockedPatterns.Count}/{DatabaseManager.GetDatabase("Patterns").Objects.Count})";
            UnityAction onClose = () => UnlockablePatternsMenu.Instance.Open();
            float iconScale = 0.8f;
            NotificationsManager.Notify(icon, title, description, onClose, iconScale);

            AddPatternUI(patternID);
        }
        public void UnlockBodyPart(string bodyPartID)
        {
            if (ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartID)) return;

            ProgressManager.Data.UnlockedBodyParts.Add(bodyPartID);

            BodyPart bodyPart = DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartID);
            Sprite icon = bodyPart.Icon;
            string title = bodyPart.name;
            string description = $"You unlocked a new body part! Click to view all. ({ProgressManager.Data.UnlockedBodyParts.Count}/{DatabaseManager.GetDatabase("Body Parts").Objects.Count})";
            UnityAction onClose = () => UnlockableBodyPartsMenu.Instance.Open();
            NotificationsManager.Notify(icon, title, description, onClose);

            AddBodyPartUI(bodyPartID, true);
        }
        public void UnlockRandomBodyPart()
        {
            Database bodyParts = DatabaseManager.GetDatabase("Body Parts");
            UnlockBodyPart((bodyParts.Objects.ToArray()[UnityEngine.Random.Range(0, bodyParts.Objects.Count)]).Key);
        }
        public void UnlockRandomPattern()
        {
            Database patterns = DatabaseManager.GetDatabase("Patterns");
            UnlockPattern((patterns.Objects.ToArray()[UnityEngine.Random.Range(0, patterns.Objects.Count)]).Key);
        }
        #endregion

        #region UI
        public CreatureUI AddCreatureUI(string creatureName)
        {
            CreatureUI creatureUI = Instantiate(creatureUIPrefab, creaturesRT);
            creaturesUI.Add(creatureUI);
            creatureUI.Setup(creatureName);

            creatureUI.SelectToggle.group = creaturesToggleGroup;
            creatureUI.SelectToggle.onValueChanged.AddListener(delegate (bool isSelected)
            {
                if (isSelected)
                {
                    creatureNameText.text = creatureName;
                }
                else
                {
                    creatureNameText.text = "";
                }
            });

            creatureUI.RemoveButton.onClick.AddListener((UnityAction)delegate
            {
                ConfirmationDialog.Confirm("Remove Creature?", $"Are you sure you want to permanently remove \"{creatureName}\"?", onYes: (UnityAction)delegate
                {
                    creaturesUI.Remove(creatureUI);
                    Destroy(creatureUI.gameObject);
                    File.Delete(Path.Combine(creaturesDirectory, $"{creatureName}.dat"));

                    if (creatureName.Equals((string)Creature.Editor.LoadedCreature))
                    {
                        Creature.Editor.LoadedCreature = "";
                        Creature.Editor.IsDirty = false;
                    }
                    creatureNameText.text = "";
                });
            });

            return creatureUI;
        }
        public BodyPartUI AddBodyPartUI(string bodyPartID, bool update = false)
        {
            BodyPart bodyPart = DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartID);

            Transform grid = bodyPartGrids[bodyPart.PluralForm].grid.transform;
            if (!grid.gameObject.activeSelf)
            {
                int index = grid.GetSiblingIndex();
                grid.parent.GetChild(index).gameObject.SetActive(true);
                grid.parent.GetChild(index - 1).gameObject.SetActive(true);
            }

            BodyPartUI bodyPartUI = Instantiate(bodyPartUIPrefab, grid as RectTransform);
            bodyPartUI.Setup(bodyPart);
            bodyPartUI.name = bodyPartID;

            bodyPartUI.HoverUI.OnEnter.AddListener(delegate
            {
                if (!Input.GetMouseButton(0))
                {
                    StatisticsMenu.Instance.Setup(bodyPart);
                }
            });
            bodyPartUI.HoverUI.OnExit.AddListener(delegate
            {
                StatisticsMenu.Instance.Clear();
            });

            bodyPartUI.DragUI.OnPress.AddListener(delegate
            {
                StatisticsMenu.Instance.Close();
            });
            bodyPartUI.DragUI.OnRelease.AddListener(delegate
            {
                bodyPartGrids[bodyPart.PluralForm].grid.enabled = false;
                bodyPartGrids[bodyPart.PluralForm].grid.enabled = true;
            });
            bodyPartUI.DragUI.OnDrag.AddListener(delegate
            {
                if (!CanAddBodyPart(bodyPartID))
                {
                    bodyPartUI.DragUI.OnPointerUp(null);
                }

                if (!RectTransformUtility.RectangleContainsScreenPoint(bodyPartsRT, Input.mousePosition))
                {
                    bodyPartUI.DragUI.OnPointerUp(null);
                    editorAudioSource.PlayOneShot(createAudioClip);

                    Ray ray = Creature.Camera.Camera.ScreenPointToRay(Input.mousePosition);
                    Plane plane = new Plane(Creature.Camera.Camera.transform.forward, Creature.Mover.Platform.transform.position);

                    if (plane.Raycast(ray, out float distance))
                    {
                        BodyPartConstructor main = Creature.Constructor.AddBodyPart(bodyPartID);
                        main.transform.position = ray.GetPoint(distance);
                        main.transform.rotation = Quaternion.LookRotation(-plane.normal, Creature.Constructor.transform.up);
                        main.transform.parent = Dynamic.Transform;

                        main.SetAttached(new AttachedBodyPart(bodyPartID));

                        main.Flipped.gameObject.SetActive(false);
                        BodyPartEditor bpe = main.GetComponent<BodyPartEditor>();
                        if (bpe != null)
                        {
                            bpe.LDrag.OnMouseButtonDown();
                            bpe.LDrag.Plane = plane;
                        }
                    }
                }
            });

            bodyPartUI.ClickUI.OnRightClick.AddListener(delegate
            {
                ConfirmationDialog.Confirm("Hide Body Part?", $"Are you sure you want to hide \"{bodyPart.name}\" from the editor?", onYes: delegate 
                {
                    RemoveBodyPartUI(bodyPartUI);
                    SettingsManager.Data.HiddenBodyParts.Add(bodyPartID);
                });
            });

            if (update)
            {
                UpdateBodyPartTotals();
                UpdateLoadableCreatures();
            }

            return bodyPartUI;
        }
        public PatternUI AddPatternUI(string patternID, bool update = false)
        {
            Texture pattern = DatabaseManager.GetDatabaseEntry<Texture>("Patterns", patternID);

            PatternUI patternUI = Instantiate(patternUIPrefab, patternsRT.transform);
            patternsUI.Add(patternUI);
            patternUI.Setup(pattern, patternMaterial);
            patternUI.name = patternID;

            patternUI.SelectToggle.group = patternsToggleGroup;
            patternUI.SelectToggle.onValueChanged.AddListener((UnityAction<bool>)delegate (bool isSelected)
            {
                if (isSelected)
                {
                    Creature.Constructor.SetPattern(patternID);
                }
                else
                {
                    Creature.Constructor.SetPattern("");
                }
            });

            patternUI.ClickUI.OnRightClick.AddListener(delegate
            {
                ConfirmationDialog.Confirm("Hide Pattern?", $"Are you sure you want to hide \"{pattern.name}\" from the editor?", onYes: delegate
                {
                    RemovePatternUI(patternUI);
                    SettingsManager.Data.HiddenPatterns.Add(patternID);
                });
            });

            if (update)
            {
                UpdateLoadableCreatures();
            }

            return patternUI;
        }

        public void RemoveBodyPartUI(BodyPartUI bodyPartUI)
        {
            Transform grid = bodyPartUI.transform.parent;
            if (grid.childCount == 1)
            {
                int index = grid.GetSiblingIndex();
                grid.parent.GetChild(index).gameObject.SetActive(false);
                grid.parent.GetChild(index - 1).gameObject.SetActive(false);
            }

            Destroy(bodyPartUI.gameObject);
            this.InvokeAtEndOfFrame(UpdateBodyPartTotals);
        }
        public void RemovePatternUI(PatternUI patternUI)
        {
            Destroy(patternUI.gameObject);
        }

        public void SetTilingUI(Vector2 tiling)
        {
            patternMaterial.SetTextureScale("_MainTex", tiling);
            patternSettingsMenu.TilingX.SetTextWithoutNotify(tiling.x.ToString());
            patternSettingsMenu.TilingY.SetTextWithoutNotify(tiling.y.ToString());
        }
        public void SetOffsetUI(Vector2 offset)
        {
            patternMaterial.SetTextureOffset("_MainTex", offset);
            patternSettingsMenu.OffsetX.SetTextWithoutNotify(offset.x.ToString());
            patternSettingsMenu.OffsetY.SetTextWithoutNotify(offset.y.ToString());
        }
        public void SetShineUI(float shine)
        {
            colourSettingsMenu.ShineSlider.SetValueWithoutNotify(shine);
        }
        public void SetMetallicUI(float metallic)
        {
            colourSettingsMenu.MetallicSlider.SetValueWithoutNotify(metallic);
        }

        public void SetPrimaryColourUI(Color colour, bool isOverride)
        {
            primaryColourPicker.SetColour(colour);
            primaryColourPicker.gameObject.SetActive(colour.a != 0);
            SetPrimaryColourOverrideUI(isOverride);

            SetColourNoneUI();
        }
        public void SetPrimaryColourOverrideUI(bool isOverride)
        {
            SetColourOverrideUI(primaryColourPicker, "Primary", isOverride);
        }
        public void SetSecondaryColourUI(Color colour, bool isOverride)
        {
            secondaryColourPicker.SetColour(colour);
            secondaryColourPicker.gameObject.SetActive(colour.a != 0);
            SetSecondaryColourOverrideUI(isOverride);

            SetColourNoneUI();
        }
        public void SetSecondaryColourOverrideUI(bool isOverride)
        {
            SetColourOverrideUI(secondaryColourPicker, "Secondary", isOverride);
        }
        private void SetColourOverrideUI(ColourPicker colourPicker, string name, bool isOverride)
        {
            colourPicker.Name.text = isOverride ? $"{name}\n<size=20>(Override)</size>" : name;
        }
        private void SetColourNoneUI()
        {
            noColoursText.gameObject.SetActive(!primaryColourPicker.gameObject.activeSelf && !secondaryColourPicker.gameObject.activeSelf);
        }

        public void UpdateBodyPartTotals()
        {
            foreach (string type in bodyPartGrids.Keys)
            {
                int count = bodyPartGrids[type].grid.transform.childCount;
                TextMeshProUGUI title = bodyPartGrids[type].title;
                title.SetText($"{type.ToString()} ({count})");
                title.gameObject.SetActive(count > 0);
            }
        }
        public void UpdatePrimaryColour()
        {
            Color colour = primaryColourPicker.Colour;
            BodyPartEditor bodyPart = Creature.Editor.PaintedBodyPart;
            if (bodyPart)
            {
                bodyPart.BodyPartConstructor.SetPrimaryColour(colour);
                SetPrimaryColourOverrideUI(bodyPart.BodyPartConstructor.IsPrimaryOverridden);
            }
            else
            {
                Creature.Constructor.SetPrimaryColour(primaryColourPicker.Colour);
                patternMaterial.SetColor("_PrimaryCol", colour);
                SetPrimaryColourOverrideUI(false);
            }
        }
        public void UpdateSecondaryColour()
        {
            Color colour = secondaryColourPicker.Colour;
            BodyPartEditor bodyPart = Creature.Editor.PaintedBodyPart;
            if (bodyPart)
            {
                bodyPart.BodyPartConstructor.SetSecondaryColour(colour);
                SetSecondaryColourOverrideUI(bodyPart.BodyPartConstructor.IsSecondaryOverridden);
            }
            else
            {
                Creature.Constructor.SetSecondaryColour(secondaryColourPicker.Colour);
                patternMaterial.SetColor("_SecondaryCol", colour);
                SetSecondaryColourOverrideUI(false);
            }
        }
        public void UpdateStatistics()
        {
            CreatureStatistics statistics = Creature.Constructor.Statistics;
            CreatureDimensions dimensions = Creature.Constructor.Dimensions;

            complexityText.text = $"<b>Complexity:</b> {statistics.complexity}/{Creature.Constructor.MaxComplexity}";
            heightText.text = $"<b>Height:</b> {Math.Round(dimensions.height, 2)}m";
            weightText.text = $"<b>Weight:</b> {Math.Round(statistics.weight, 2)}kg";
            dietText.text = $"<b>Diet:</b> {statistics.Diet}";
            healthText.text = $"<b>Health:</b> {statistics.health}";
            speedText.text = $"<b>Speed:</b> {statistics.speed}";
            bonesText.text = $"<b>Bones:</b> {Creature.Constructor.Bones.Count}";

            bodyPartsToggle.onValueChanged.Invoke(bodyPartsToggle.isOn);
            abilitiesToggle.onValueChanged.Invoke(abilitiesToggle.isOn);

            cashText.text = $"${Creature.Editor.Cash}";
        }
        public void UpdateCreaturesFormatting()
        {
            foreach (CreatureUI creatureUI in creaturesUI)
            {
                creatureUI.NameText.text = creatureUI.NameText.text.Replace("<u>", "").Replace("</u>", "").Replace("*", ""); // Clear all formatting.

                if (!string.IsNullOrEmpty(Creature.Editor.LoadedCreature) && creatureUI.NameText.text.Equals(Creature.Editor.LoadedCreature))
                {
                    creatureUI.NameText.text = $"<u>{creatureUI.NameText.text}</u>";
                    if (Creature.Editor.IsDirty)
                    {
                        creatureUI.NameText.text += "*";
                    }
                }
            }
        }
        public void UpdateLoadableCreatures()
        {
            foreach (CreatureUI creatureUI in creaturesUI)
            {
                CreatureData creatureData = SaveUtility.Load<CreatureData>(Path.Combine(creaturesDirectory, $"{creatureUI.name}.dat"), creatureEncryptionKey.Value);

                bool canLoadCreature = CanLoadCreature(creatureData, out string errorMessage);

                // Button
                creatureUI.ErrorButton.gameObject.SetActive(!canLoadCreature);
                if (!canLoadCreature)
                {
                    creatureUI.ErrorButton.onClick.RemoveAllListeners();
                    creatureUI.ErrorButton.onClick.AddListener(delegate
                    {
                        InformationDialog.Inform("Creature unavailable!", errorMessage);
                    });
                }

                // Background
                Color colour = canLoadCreature ? Color.white : Color.black;
                colour.a = 0.4f;
                creatureUI.SelectToggle.targetGraphic.color = colour;
                creatureUI.SelectToggle.interactable = canLoadCreature;
            }
        }
        #endregion

        #region Keyboard Shortcuts
        private void HandleKeyboardShortcuts()
        {
            if (!Creature || Creature.Health.IsDead) return;

            if (IsBuilding)
            {
                HandleBuildShortcuts();
            }
            else
            if (IsPlaying)
            {
                HandlePlayShortcuts();
            }
            else
            if (IsPainting)
            {
                HandlePaintShortcuts();
            }

            if (IsEditing)
            {
                HandleEditorShortcuts();
            }
        }

        private void HandleBuildShortcuts()
        {
        }
        private void HandlePlayShortcuts()
        {
            if (InputUtility.GetKeyDown(KeybindingsManager.Data.ToggleUI))
            {
                IsVisible = !IsVisible;
            }
            else
            if (InputUtility.GetKeyDown(KeybindingsManager.Data.Respawn))
            {
                Creature.Health.Die();
            }
        }
        private void HandlePaintShortcuts()
        {
        }

        private void HandleEditorShortcuts()
        {
            if (InputUtility.GetKeyDown(KeybindingsManager.Data.Save))
            {
                TrySave();
            }
            else
            if (InputUtility.GetKeyDown(KeybindingsManager.Data.Export))
            {
                TryExport();
            }
            else
            if (InputUtility.GetKeyDown(KeybindingsManager.Data.Import))
            {
                TryImport();
            }
            else
            if (InputUtility.GetKeyDown(KeybindingsManager.Data.Clear) && EventSystem.current.currentSelectedGameObject == null) // Must still allow for copying of UI text (e.g., World ID)
            {
                TryClear();
            }
        }
        #endregion

        #region Helper
        /// <summary>
        /// Is this a valid name for a creature? Checks for invalid file name characters and profanity, and corrects when possible.
        /// </summary>
        /// <param name="creatureName">The name to be validated.</param>
        /// <returns>The validity of the name.</returns>
        private bool IsValidName(string creatureName)
        {
            if (string.IsNullOrEmpty(creatureName))
            {
                InformationDialog.Inform("Creature Unnamed", "Please enter a name for your creature.");
                return false;
            }
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                creatureName = creatureName.Replace(c.ToString(), "");
            }
            creatureName = creatureName.Trim();

            ProfanityFilter filter = new ProfanityFilter();
            if (filter.ContainsProfanity(creatureName))
            {
                IReadOnlyCollection<string> profanities = filter.DetectAllProfanities(creatureName);
                if (profanities.Count > 0)
                {
                    InformationDialog.Inform("Profanity Detected", $"Please remove the following from your creature's name:\n<i>{string.Join(", ", profanities)}</i>");
                }
                else
                {
                    InformationDialog.Inform("Profanity Detected", $"Please don't include profane terms in your creature's name.");
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Prompts the user to confirm before performing an operation.
        /// </summary>
        /// <param name="operation">The operation to be performed.</param>
        /// <param name="confirm">Whether or not confirmation is necessary.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="message">The message of the dialog.</param>
        private void ConfirmOperation(UnityAction operation, bool confirm, string title, string message)
        {
            if (confirm)
            {
                ConfirmationDialog.Confirm(title, message, onYes: operation);
            }
            else
            {
                operation();
            }
        }
        private void ConfirmUnsavedChanges(UnityAction operation, string operationPCN)
        {
            ConfirmOperation(operation, Creature.Editor.IsDirty && !string.IsNullOrEmpty(Creature.Editor.LoadedCreature), "Unsaved changes!", $"You have made changes to \"{Creature.Editor.LoadedCreature}\" without saving. Are you sure you want to continue {operationPCN}?");
        }
        #endregion
        #endregion

        #region Inner Classes
        [Serializable]
        public struct GridInfo
        {
            public TextMeshProUGUI title;
            public GridLayoutGroup grid;
        }

        [Serializable]
        public class BodyPartGrids : SerializableDictionaryBase<string, GridInfo> { }
        #endregion
    }
}