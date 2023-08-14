using System.Collections.Generic;
using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace Data
{
    public class TemporaryDataStorage : MonoBehaviour
    {
        private Dictionary<string, TemporaryData> _data = new Dictionary<string, TemporaryData>();

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

        public bool HasKey(string key) => _data.ContainsKey(key);

        public void SetValue(string key, TemporaryData data)
        {
            _data[key] = data;
        }

        public TemporaryData GetValue(string key)
        {
            if (_data.TryGetValue(key, out TemporaryData data))
            {
                return data;
            }

            return null;
        }

        public void RemoveValue(string key)
        {
            _data.Remove(key);
        }

        public bool TryGetValue(string key, out TemporaryData data)
        {
            return _data.TryGetValue(key, out data);
        }

        public void GetValue(string key, TemporaryData defaultValue, out TemporaryData data)
        {
            if (_data.TryGetValue(key, out data))
            {
                return;
            }

            data = defaultValue;
        }

        private void ClearData()
        {
            _data.Clear();
        }
    }

    public class TemporaryData
    {

    }

    public class TemporaryData<T> : TemporaryData
    {
        public T Value;

        public TemporaryData(T value)
        {
            Value = value;
        }
    }
}
