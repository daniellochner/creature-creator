// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DemoManager : MonoBehaviourSingleton<DemoManager>
    {
        #region Fields
        [Header("Settings")]
        [SerializeField] private DemoKeys keys;
        [SerializeField] private MinMax minMaxMusicDB;
        [SerializeField] private MinMax minMaxSoundEffectsDB;

        [Header("Data")]
        [SerializeField, ReadOnly] private DemoProgress progress;
        [SerializeField, ReadOnly] private DemoSettings settings;
        [Space]
        [Button("Save")] [SerializeField] private bool save;
        [Button("Load")] [SerializeField] private bool load;
        #endregion

        #region Properties
        public DemoKeys Keys => keys;
        public MinMax MinMaxMusicDB => minMaxMusicDB;
        public MinMax MinMaxSoundEffectsDB => minMaxSoundEffectsDB;
        public DemoProgress Progress => progress;
        public DemoSettings Settings => settings;

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

            if (File.Exists(Path.Combine(DataDir, "demo.json")))
            {
                Load();
            }
            else
            {
                Save();
            }
        }
        private void Setup()
        {
            NetworkShutdownManager.Instance.OnUncontrolledShutdown += OnUncontrolledShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledClientShutdown += OnUncontrolledClientShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledHostShutdown += OnUncontrolledHostShutdown;
        }

        public void Save()
        {
            SaveUtility.Save(JsonUtility.ToJson(progress), Path.Combine(DataDir, "demo.json"));
            SaveUtility.Save(JsonUtility.ToJson(settings), Path.Combine(DataDir, "settings.json"));
        }
        public void Load()
        {
            progress = JsonUtility.FromJson<DemoProgress>(SaveUtility.Load(Path.Combine(DataDir, "demo.json"))) ?? new DemoProgress();
            settings = JsonUtility.FromJson<DemoSettings>(SaveUtility.Load(Path.Combine(DataDir, "settings.json"))) ?? new DemoSettings();
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