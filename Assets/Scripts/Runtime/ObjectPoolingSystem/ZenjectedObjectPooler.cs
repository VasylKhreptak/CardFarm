using Plugins.ObjectPooler;
using UnityEngine;
using Zenject;

namespace Runtime.ObjectPoolingSystem
{
    public class ZenjectedObjectPooler : ObjectPooler
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
