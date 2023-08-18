using System;
using UnityEngine;
using Action = CBA.Actions.Core.Action;

namespace CBA.Actions.Management.GameObject.Core
{
    public abstract class GameObjectAction : Action
    {
        [Header("References")]
        [SerializeField] protected UnityEngine.GameObject _gameObject;

        #region MonoBehaviour

        private void OnValidate()
        {
            _gameObject ??= GetComponent<UnityEngine.GameObject>();
        }

        #endregion
    }
}
