//Based of the following thread https://forum.unity.com/threads/finally-a-serializable-dictionary-for-unity-extracted-from-system-collections-generic.335797/

using System.Collections.Generic;
using UnityEngine;

namespace RotaryHeart.Lib.SerializableDictionary
{
    /// <summary>
    /// This class is only used to be able to draw the custom property drawer
    /// </summary>
    public abstract class DrawableDictionary
    {
        [HideInInspector]
        public ReorderableList reorderableList = null;
        [HideInInspector]
        public RequiredReferences reqReferences;
        public bool isExpanded;
    }

    /// <summary>
    /// Base class that most be used for any dictionary that wants to be implemented
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [System.Serializable]
    public class SerializableDictionaryBase<TKey, TValue> : DrawableDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        private Dictionary<TKey, TValue> _dict;
        private readonly static Dictionary<TKey, TValue> _staticEmptyDict = new Dictionary<TKey, TValue>(0);

        /// <summary>
        /// Copies the data from a dictionary. If an entry with the same key is found it replaces the value
        /// </summary>
        /// <param name="src">Dictionary to copy the data from</param>
        public void CopyFrom(IDictionary<TKey, TValue> src)
        {
            foreach (var data in src)
            {
                if (ContainsKey(data.Key))
                {
                    this[data.Key] = data.Value;
                }
                else
                {
                    Add(data.Key, data.Value);
                }
            }
        }

        /// <summary>
        /// Copies the data from a dictionary. If an entry with the same key is found it replaces the value. Note that if the <paramref name="src"/> is not a dictionary of the same type it will not be copied
        /// </summary>
        /// <param name="src">Dictionary to copy the data from</param>
        public void CopyFrom(object src)
        {
            var dictionary = src as Dictionary<TKey, TValue>;
            if (dictionary != null)
            {
                CopyFrom(dictionary);
            }
        }

        /// <summary>
        /// Copies the data to a dictionary. If an entry with the same key is found it replaces the value
        /// </summary>
        /// <param name="dest">Dictionary to copy the data to</param>
        public void CopyTo(IDictionary<TKey, TValue> dest)
        {
            foreach (var data in this)
            {
                if (dest.ContainsKey(data.Key))
                {
                    dest[data.Key] = data.Value;
                }
                else
                {
                    dest.Add(data.Key, data.Value);
                }
            }
        }

        /// <summary>
        /// Returns a copy of the dictionary.
        /// </summary>
        public Dictionary<TKey, TValue> Clone()
        {
            Dictionary<TKey, TValue> dest = new Dictionary<TKey, TValue>(Count);

            foreach (var data in this)
            {
                dest.Add(data.Key, data.Value);
            }

            return dest;
        }

        /// <summary>
        /// Returns true if the value exists; otherwise, false
        /// </summary>
        /// <param name="value">Value to check</param>
        public bool ContainsValue(TValue value)
        {
            if (_dict == null)
                return false;

            return _dict.ContainsValue(value);
        }

        #region IDictionary Interface

        #region Properties

        public TValue this[TKey key]
        {
            get
            {
                if (_dict == null) throw new KeyNotFoundException();
                return _dict[key];
            }
            set
            {
                if (_dict == null) _dict = new Dictionary<TKey, TValue>();
                _dict[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (_dict == null)
                    _dict = new Dictionary<TKey, TValue>();

                return _dict.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (_dict == null)
                    _dict = new Dictionary<TKey, TValue>();

                return _dict.Values;
            }
        }

        public int Count
        {
            get
            {
                return (_dict != null) ? _dict.Count : 0;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        #endregion Properties

        public void Add(TKey key, TValue value)
        {
            if (_dict == null)
                _dict = new Dictionary<TKey, TValue>();

            _dict.Add(key, value);

            if (Application.isEditor)
            {
                if (_keyValues == null)
                    _keyValues = new List<TKey>();
                if (_keys == null)
                    _keys = new List<TKey>();
                if (_values == null)
                    _values = new List<TValue>();

                _keyValues.Add(key);
                _keys.Add(key);
                _values.Add(value);
            }
        }

        public void Clear()
        {
            if (_dict != null)
                _dict.Clear();

            if (Application.isEditor)
            {
                if (_keyValues != null)
                    _keyValues.Clear();
                if (_keys != null)
                    _keys.Clear();
                if (_values != null)
                    _values.Clear();
            }
        }

        public bool ContainsKey(TKey key)
        {
            if (_dict == null)
                return false;

            return _dict.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            if (_dict == null)
                return false;

            if (Application.isEditor)
            {
                if (_keyValues != null)
                    _keyValues.Remove(key);
                if (_keys != null)
                {
                    int index = _keys.IndexOf(key);
                    _keys.Remove(key);

                    if (_values != null)
                        _values.RemoveAt(index);
                }
            }

            return _dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_dict == null)
            {
                value = default(TValue);
                return false;
            }

            return _dict.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            if (_dict == null) _dict = new Dictionary<TKey, TValue>();
            (_dict as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (_dict == null) return false;
            return (_dict as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (_dict == null) return;
            (_dict as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_dict == null) return false;
            return (_dict as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            if (_dict == null) return _staticEmptyDict.GetEnumerator();
            return _dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ISerializationCallbackReceiver

        [SerializeField]
        private List<TKey> _keyValues;

        [SerializeField]
        private List<TKey> _keys;
        [SerializeField]
        private List<TValue> _values;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_keys != null && _values != null)
            {
                //Need to clear the dictionary
                if (_dict == null)
                    _dict = new Dictionary<TKey, TValue>(_keys.Count);
                else
                    _dict.Clear();

                for (int i = 0; i < _keys.Count; i++)
                {
                    //This should only happen with reference type keys (Generic, Object, etc)
                    if (_keys[i] == null)
                    {
                        //Special case for UnityEngine.Object classes
                        if (typeof(Object).IsAssignableFrom(typeof(TKey)))
                        {
                            //Key type
                            string tKeyType = typeof(TKey).ToString();

                            //We need the reference to the reference holder class
                            if (reqReferences == null)
                            {
                                Debug.LogError("A key of type: " + tKeyType + " requires to have a valid RequiredReferences reference");
                                continue;
                            }

                            //Use reflection to check all the fields included on the class
                            foreach (var field in typeof(RequiredReferences).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic))
                            {
                                //Only set the value if the type is the same
                                if (field.FieldType.ToString().Equals(tKeyType))
                                {
                                    _keys[i] = (TKey)(field.GetValue(reqReferences));
                                    break;
                                }
                            }

                            //References class is missing the field, skip the element
                            if (_keys[i] == null)
                            {
                                Debug.LogError("Couldn't find " + tKeyType + " reference.");
                                continue;
                            }
                        }
                        else
                        {
                            //Create a instance for the key
                            _keys[i] = System.Activator.CreateInstance<TKey>();
                        }
                    }

                    //Add the data to the dictionary. Value can be null so no special step is required
                    if (i < _values.Count)
                        _dict[_keys[i]] = _values[i];
                    else
                        _dict[_keys[i]] = default(TValue);
                }
            }

            _keys = null;
            _values = null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (_dict == null || _dict.Count == 0)
            {
                //Dictionary is empty, erase data
                _keys = null;
                _values = null;
            }
            else
            {
                //Initialize arrays
                int cnt = _dict.Count;
                _keys = new List<TKey>(cnt);
                _values = new List<TValue>(cnt);

                var e = _dict.GetEnumerator();
                while (e.MoveNext())
                {
                    //Set the respective data from the dictionary
                    _keys.Add(e.Current.Key);
                    _values.Add(e.Current.Value);
                }
            }
        }

        #endregion

    }
}
