// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.IO;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DemoManager : MonoBehaviourSingleton<DemoManager>
    {
        #region Fields
        [Header("Settings")]
        [SerializeField] private DemoKeys keys;

        [Header("Data")]
        [SerializeField, ReadOnly] private DemoProgress progress;
        [SerializeField, ReadOnly] private DemoSettings settings;
        [Space]
        [Button("Save")] [SerializeField] private bool save;
        [Button("Load")] [SerializeField] private bool load;
        #endregion

        #region Properties
        public static DemoKeys Keys => Instance.keys;
        public static DemoProgress Progress => Instance.progress;
        public static DemoSettings Settings => Instance.settings;

        private string DataDir { get; set; }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }
        private void Start()
        {
            Setup();
        }


        private void Initialize()
        {
            DataDir = Path.Combine(Application.persistentDataPath, Application.version);
            if (!Directory.Exists(DataDir))
            {
                Directory.CreateDirectory(DataDir);
            }
            Load();
            Save();
        }
        private void Setup()
        {
            NetworkShutdownManager.Instance.OnUncontrolledShutdown += OnUncontrolledShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledClientShutdown += OnUncontrolledClientShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledHostShutdown += OnUncontrolledHostShutdown;
        }

        public void Save()
        {
            SaveUtility.Save(Path.Combine(DataDir, "progress.dat"), progress, keys.ProgressEncryption.Value);
            SaveUtility.Save(Path.Combine(DataDir, "settings.dat"), settings);
        }
        public void Load()
        {
            progress = SaveUtility.Load<DemoProgress>(Path.Combine(DataDir, "progress.dat"), keys.ProgressEncryption.Value);
            settings = SaveUtility.Load<DemoSettings>(Path.Combine(DataDir, "settings.dat"));
        }

        public void OnUncontrolledShutdown()
        {
            SceneManager.LoadScene("MainMenu");
        }
        public void OnUncontrolledClientShutdown()
        {
            InformationDialog.Inform("Disconnected!", "You lost connection.");
        }
        public void OnUncontrolledHostShutdown()
        {
            InformationDialog.Inform("Disconnected!", "The host lost connection.");
        }

        public void OnInactivityKick()
        {
            InformationDialog.Instance.Close(true);
        }
        public void OnInactivityWarn(int warnTime)
        {
            InformationDialog.Inform("Inactivity Warning!", $"You will be kicked due to inactivity in {warnTime} seconds.", "Cancel");
        }
        #endregion
    }
}