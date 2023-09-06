using System.IO;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class DataManager<T, M> : MonoBehaviourSingleton<T> where T : DataManager<T, M> where M : Data, new()
    {
        #region Fields
        [Header("Data")]
        [SerializeField] private string fileName;
        [SerializeField] private SecretKey encryptionKey;
        [SerializeField, ReadOnly] private M data;

        [SerializeField, Button("Save")] private bool save;
        [SerializeField, Button("Load")] private bool load;
        [SerializeField, Button("Revert")] private bool revert;
        #endregion

        #region Properties
        public static M Data => Instance.data;

        public virtual string SALT { get; }
        private string EncryptionKeyValue => (encryptionKey != null) ? (encryptionKey.Value + SALT) : "";

        public bool HasFailed { get; set; } = false;
        #endregion

        #region Methods
        protected virtual void Start()
        {
            Setup();
        }
        protected void Setup()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, fileName)))
            {
                Revert();
            }
            Load();
        }

        [ContextMenu("Save")]
        public virtual void Save()
        {
            SaveUtility.Save(Path.Combine(Application.persistentDataPath, fileName), data, EncryptionKeyValue);
        }
        [ContextMenu("Load")]
        public virtual void Load()
        {
            M loaded = SaveUtility.Load<M>(Path.Combine(Application.persistentDataPath, fileName), EncryptionKeyValue);

            HasFailed = (loaded == null);

            if (!HasFailed)
            {
                data = loaded;
            }
            else
            {
                Debug.Log($"Failed to load {Path.Combine(Application.persistentDataPath, fileName)}.");
                data.Revert();
            }
        }
        [ContextMenu("Revert")]
        public virtual void Revert()
        {
            data.Revert();
            Save();
        }
        #endregion
    }
}