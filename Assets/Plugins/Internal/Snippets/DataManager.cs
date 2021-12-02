using System.IO;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class DataManager<T, M> : MonoBehaviourSingleton<T> where T : DataManager<T, M> where M : class, new()
    {
        #region Fields
        [SerializeField] private string fileName;
        [SerializeField] private SecretKey encryptionKey;

        private M data;
        #endregion

        #region Properties
        public static M Data => Instance.data;

        private string EncryptionKeyValue => (encryptionKey != null) ? encryptionKey.Value : "";
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            Load();
            Save();
        }
        [ContextMenu("Save")]
        public virtual void Save()
        {
            SaveUtility.Save(Path.Combine(Application.persistentDataPath, fileName), data, EncryptionKeyValue);
        }
        [ContextMenu("Load")]
        public virtual void Load()
        {
            data = SaveUtility.Load<M>(Path.Combine(Application.persistentDataPath, fileName), EncryptionKeyValue);
        }
        #endregion
    }
}