using System;
using Plugins.ObjectPooler.Linker;
using UnityEngine;

namespace Plugins.ObjectPooler.Core
{
    public class PooledObject : MonoBehaviour
    {
        public PositionLinker linker;
        public Enum pool;
        
        public event Action<PooledObject> onEnable;
        public event Action<PooledObject> onDisable;

        #region MonoBehaviour

        private void OnEnable()
        {
            onEnable?.Invoke(this);
        }

        private void OnDisable()
        {
            onDisable?.Invoke(this);
        }

        #endregion

    }
}
