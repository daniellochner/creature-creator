// Databases
// Copyright (c) Daniel Lochner

using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [CreateAssetMenu(menuName = "Database")]
    public class Database : ScriptableObject
    {
        #region Fields
        [SerializeField] private ObjectDictionary objects = new ObjectDictionary();
        #endregion

        #region Properties
        public ObjectDictionary Objects => objects;
        #endregion

        #region Methods
        public T GetEntry<T>(string key) where T : UnityEngine.Object
        {
            if (objects.ContainsKey(key))
            {
                return objects[key] as T;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Inner Classes
        [Serializable] public class ObjectDictionary : SerializableDictionaryBase<string, UnityEngine.Object> { }
        #endregion
    }
}