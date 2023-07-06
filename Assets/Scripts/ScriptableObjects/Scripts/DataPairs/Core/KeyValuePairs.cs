using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScriptableObjects.Scripts.DataPairs.Core
{
    public class KeyValuePairs<TKey, TValue> : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private KVPair[] _pairs;

        private Dictionary<TKey, TValue> _dictionary;

        public TValue GetValue(TKey key)
        {
            if (_dictionary == null)
            {
                _dictionary = new Dictionary<TKey, TValue>(_pairs.Length);
                foreach (var pair in _pairs)
                {
                    _dictionary.Add(pair.Key, pair.Value);
                }
            }

            return _dictionary[key];
        }

        [Serializable]
        private class KVPair
        {
            public TKey Key;
            public TValue Value;
        }
    }
}
