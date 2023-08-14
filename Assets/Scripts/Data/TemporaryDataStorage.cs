using System.Collections.Generic;
using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace Data
{
    public class TemporaryDataStorage : MonoBehaviour
    {
        private Dictionary<string, float> _floats = new Dictionary<string, float>();
        private Dictionary<string, int> _ints = new Dictionary<string, int>();
        private Dictionary<string, string> _strings = new Dictionary<string, string>();
        private Dictionary<string, bool> _bools = new Dictionary<string, bool>();

        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand)
        {
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += ClearData;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= ClearData;
        }

        #endregion

        #region Float

        public void SetFloat(string key, float value) => _floats[key] = value;

        public float GetFloat(string key) => _floats[key];

        public float GetFloat(string key, float defaultValue)
        {
            if (_floats.TryGetValue(key, out var value))
            {
                return value;
            }

            return defaultValue;
        }

        #endregion

        #region Int

        public void SetInt(string key, int value) => _ints[key] = value;

        public int GetInt(string key) => _ints[key];

        public int GetInt(string key, int defaultValue)
        {
            if (_ints.TryGetValue(key, out var value))
            {
                return value;
            }

            return defaultValue;
        }

        #endregion

        #region String

        public void SetString(string key, string value) => _strings[key] = value;

        public string GetString(string key) => _strings[key];

        public string GetString(string key, string defaultValue)
        {
            if (_strings.TryGetValue(key, out var value))
            {
                return value;
            }

            return defaultValue;
        }

        #endregion

        #region Bool

        public void SetBool(string key, bool value) => _bools[key] = value;

        public bool GetBool(string key) => _bools[key];

        public bool GetBool(string key, bool defaultValue)
        {
            if (_bools.TryGetValue(key, out var value))
            {
                return value;
            }

            return defaultValue;
        }

        #endregion

        private void ClearData()
        {
            _floats.Clear();
            _ints.Clear();
            _strings.Clear();
            _bools.Clear();
        }
    }
}
