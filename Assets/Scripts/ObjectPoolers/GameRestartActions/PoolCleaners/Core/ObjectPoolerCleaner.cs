using System;
using Plugins.ObjectPooler.Core;
using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace ObjectPoolers.GameRestartActions
{
    public class ObjectPoolerCleaner<T> : MonoBehaviour where T : Enum
    {
        [Header("References")]
        [SerializeField] private ObjectPooler<T> _targetPooler;

        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand)
        {
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _targetPooler ??= GetComponent<ObjectPooler<T>>();
        }

        private void Awake()
        {
            _gameRestartCommand.OnExecute += _targetPooler.DisableAllObjects;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= _targetPooler.DisableAllObjects;
        }

        #endregion
    }
}
