// Databases
// Copyright (c) Daniel Lochner

using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class DatabaseManager : MonoBehaviourSingleton<DatabaseManager>
    {
        #region Fields
        [SerializeField] private DatabaseDictionary databases;
        #endregion

        #region Methods
        public static Database GetDatabase(string database)
        {
            return Instance.databases[database];
        }
        public static T GetDatabaseEntry<T>(string database, string key) where T : UnityEngine.Object
        {
            return Instance.databases[database].GetEntry<T>(key);
        }
        #endregion

        #region Inner Classes
        [Serializable] public class DatabaseDictionary : SerializableDictionaryBase<string, Database> { }
        #endregion
    }
}