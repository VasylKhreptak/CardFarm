using System;
using Plugins.ObjectPooler.Example;
using UnityEngine;
using Zenject;

namespace Plugins.ObjectPooler.Core
{
    public class ZenjectedObjectPooler<T> : ObjectPooler<T> where T : Enum
    {
        private DiContainer _container;

        [Inject]
        private void Constructor(DiContainer container)
        {
            _container = container;
        }

        protected override GameObject InstantiateObject(GameObject prefab)
        {
            return _container.InstantiatePrefab(prefab);
        }
    }
}
