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
        [SerializeField, ReadOnly] private DemoData data;
        [Space]
        [Button("Save")] [SerializeField] private bool save;
        [Button("Load")] [SerializeField] private bool load;
        #endregion

        #region Properties
        public DemoKeys Keys => keys;
        public MinMax MinMaxMusicDB => minMaxMusicDB;
        public MinMax MinMaxSoundEffectsDB => minMaxSoundEffectsDB;
        public DemoData Data => data;

        public Vector2Int DisplaySize
        {
            get
            {
                switch (data.DisplaySize)
                {
                    case 1:
                        return new Vector2Int(1024, 576);
                    case 2:
                        return new Vector2Int(1024, 768);
                    case 3:
                        return new Vector2Int(1280, 720);
                    case 4:
                        return new Vector2Int(1600, 900);
                    case 5:
                        return new Vector2Int(1920, 1080);                     
                }
                return new Vector2Int(Screen.width, Screen.height); // Fullscreen
            }
        }

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
            SaveUtility.Save(JsonUtility.ToJson(data), Path.Combine(DataDir, "demo.json"));
        }
        public void Load()
        {
            data = JsonUtility.FromJson<DemoData>(SaveUtility.Load(Path.Combine(DataDir, "demo.json"))) ?? new DemoData();
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