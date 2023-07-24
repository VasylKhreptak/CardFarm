using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.ObjectPooler.Linker;
using UnityEngine;

namespace Plugins.ObjectPooler.Core
{
    public abstract class ObjectPooler<T> : MonoBehaviour where T : Enum
    {
        [Header("Preferences")]
        [Tooltip("Allocates memory for the maximum size of the pool(MaxExpandSize).This will reduce the amount of memory allocations, but will increase the memory usage.")]
        [SerializeField] private bool _allocateMaxMemorySize;
        [SerializeField] private bool _useFolders = true;

        [Header("Pools")]
        [SerializeField] private PoolPreference[] _poolsPreferences;

        private Dictionary<T, Queue<PooledObject>> _pools;
        private Dictionary<T, HashSet<PooledObject>> _activePools;
        private Dictionary<T, HashSet<PooledObject>> _inactivePools;

        public int ActiveObjectsCount => _activePools.Sum(x => x.Value.Count);
        
        private int GetCollectionCapacity(PoolPreference preference)
        {
            return _allocateMaxMemorySize ? preference.maxExpandSize : preference.initialSize;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            if (_poolsPreferences == null) return;

            ValidateInputData();
        }

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        #endregion

        #region Fill

        private void Init()
        {
            _pools = new Dictionary<T, Queue<PooledObject>>();
            _activePools = new Dictionary<T, HashSet<PooledObject>>();
            _inactivePools = new Dictionary<T, HashSet<PooledObject>>();

            foreach (var poolPreferences in _poolsPreferences)
            {
                _pools.Add(poolPreferences.pool, CreatePool(poolPreferences));
                _activePools.Add(poolPreferences.pool, new HashSet<PooledObject>(GetCollectionCapacity(poolPreferences)));
                _inactivePools.Add(poolPreferences.pool, _pools[poolPreferences.pool].ToHashSet());
            }
        }

        private Queue<PooledObject> CreatePool(PoolPreference poolPreference)
        {
            var objectPool = new Queue<PooledObject>(GetCollectionCapacity(poolPreference));

            Transform poolFolder = _useFolders ? CreatePoolFolder(poolPreference.pool) : transform;

            for (int i = 0; i < poolPreference.initialSize; i++)
            {
                PooledObject newPoolObject = CreateNewPoolObject(poolPreference.pool, poolFolder);
                objectPool.Enqueue(newPoolObject);
            }

            return objectPool;
        }

        protected virtual GameObject InstantiateObject(GameObject prefab)
        {
            return Instantiate(prefab);
        }

        private Transform CreatePoolFolder(T pool)
        {
            GameObject poolParentObj = new GameObject(pool.ToString());

            poolParentObj.transform.SetParent(transform);

            return poolParentObj.transform;
        }

        #endregion

        #region ActiveInactivePoolsManagement

        private void AddListener(PooledObject pooledObject)
        {
            pooledObject.onEnable += OnObjectEnabled;
            pooledObject.onDisable += OnObjectDisabled;
        }

        private void RemoveListener(PooledObject pooledObject)
        {
            pooledObject.onEnable -= OnObjectEnabled;
            pooledObject.onDisable -= OnObjectDisabled;
        }

        private void RemoveListeners()
        {
            foreach (var pool in _pools)
            {
                foreach (var pooledObject in _pools[pool.Key])
                {
                    RemoveListener(pooledObject);
                }
            }
        }

        private void AddListeners()
        {
            foreach (var pool in _pools)
            {
                foreach (var pooledObject in _pools[pool.Key])
                {
                    AddListener(pooledObject);
                }
            }
        }

        private void RemoveFromActivePool(PooledObject pooledObject) => _activePools[(T)pooledObject.pool].Remove(pooledObject);

        private void AddToActivePool(PooledObject pooledObject) => _activePools[(T)pooledObject.pool].Add(pooledObject);

        private void RemoveFromInactivePool(PooledObject pooledObject) => _inactivePools[(T)pooledObject.pool].Remove(pooledObject);

        private void AddToInactivePool(PooledObject pooledObject) => _inactivePools[(T)pooledObject.pool].Add(pooledObject);

        private void OnObjectEnabled(PooledObject pooledObject)
        {
            AddToActivePool(pooledObject);
            RemoveFromInactivePool(pooledObject);
        }

        private void OnObjectDisabled(PooledObject pooledObject)
        {
            RemoveFromActivePool(pooledObject);
            AddToInactivePool(pooledObject);
        }

        #endregion

        #region Spawn

        public GameObject Spawn(T pool) => Spawn(pool, Vector3.zero, Quaternion.identity);

        public GameObject Spawn(T pool, Vector3 position) => Spawn(pool, position, Quaternion.identity);

        public GameObject Spawn(T pool, Vector3 position, Quaternion rotation)
        {
            if (_pools.ContainsKey(pool) == false)
            {
                throw new ArgumentException("Pool '" + pool + "' does not exist.");
            }

            PooledObject poolObject = GetAppropriatePoolObject(pool);
            GameObject pooledGameObject = poolObject.gameObject;

            Transform poolObjectTransform = pooledGameObject.transform;
            poolObjectTransform.position = position;
            poolObjectTransform.rotation = rotation;

            pooledGameObject.SetActive(true);

            return pooledGameObject;
        }

        public GameObject Spawn(T pool, Vector3 position, LinkerData data) => Spawn(pool, position, Quaternion.identity, data);

        public GameObject Spawn(T pool, LinkerData data) => Spawn(pool, Vector3.zero, Quaternion.identity, data);

        public GameObject Spawn(T pool, Vector3 position, Quaternion rotation, LinkerData data)
        {
            GameObject spawnedObject = Spawn(pool, position, rotation);
            StartLinking(spawnedObject, data);

            return spawnedObject;
        }

        public void StartLinking(GameObject gameObject, LinkerData data)
        {
            if (IsObjectPooled(gameObject, out PooledObject pooledObject))
            {
                pooledObject.linker.StartUpdating(data);
            }
        }

        public void StopLinking(GameObject gameObject)
        {
            if (IsObjectPooled(gameObject, out PooledObject pooledObject))
            {
                pooledObject.linker.StopUpdating();
            }
        }

        private PooledObject GetAppropriatePoolObject(T pool)
        {
            if (TryGetInactive(pool, out PooledObject poolObject))
            {
                return poolObject;
            }
            if (IsExpandable(pool))
            {
                return AddPoolObject(pool);
            }

            PooledObject poolObj = _pools[pool].Dequeue();
            poolObj.gameObject.SetActive(false);
            _pools[pool].Enqueue(poolObj);

            return poolObj;
        }

        private bool TryGetInactive(T pool, out PooledObject poolObject)
        {
            if (_inactivePools[pool].Count > 0)
            {
                poolObject = _inactivePools[pool].First();
                return true;
            }

            poolObject = null;
            return false;
        }

        public GameObject GetPrefab(T pool)
        {
            foreach (var objectPool in _poolsPreferences)
            {
                if (EqualityComparer<T>.Default.Equals(objectPool.pool, pool))
                {
                    return objectPool.prefab;

                }
            }

            throw new ArgumentException("Pool '" + pool + "' does not exist.");
        }

        private bool IsExpandable(T pool)
        {
            foreach (var poolPreferences in _poolsPreferences)
            {
                if (EqualityComparer<T>.Default.Equals(poolPreferences.pool, pool))
                {
                    return poolPreferences.autoExpand && _pools[pool].Count < poolPreferences.maxExpandSize;
                }
            }

            throw new ArgumentException("Pool '" + pool + "' does not exist.");
        }

        private PooledObject AddPoolObject(T pool) => AddPoolObject(pool, GetPoolFolder(pool));

        private PooledObject AddPoolObject(T pool, Transform folder)
        {
            PooledObject newPoolObject = CreateNewPoolObject(pool, folder);

            _pools[pool].Enqueue(newPoolObject);

            return newPoolObject;
        }

        private PooledObject CreateNewPoolObject(T pool, Transform parent)
        {
            GameObject newPoolObject = InstantiateObject(GetPrefab(pool));

            newPoolObject.SetActive(false);

            newPoolObject.transform.SetParent(parent);

            PooledObject pooledObject = newPoolObject.AddComponent<PooledObject>();
            pooledObject.pool = pool;
            pooledObject.linker = newPoolObject.AddComponent<PositionLinker>();

            AddListener(pooledObject);

            return pooledObject;
        }

        #endregion

        #region Utils

        public void DisableAllObjects()
        {
            foreach (var pool in _activePools)
            {
                DisablePool(pool.Key);
            }
        }

        public void DisablePool(T pool)
        {
            foreach (var poolItem in _activePools[pool].ToList())
            {
                poolItem.gameObject.SetActive(false);
            }
        }

        private Transform GetPoolFolder(T pool)
        {
            if (_useFolders == false) return transform;

            if (_pools[pool].Count == 0)
            {
                throw new Exception("Pool '" + pool + "' is empty.");
            }

            PooledObject poolObject = _pools[pool].First();

            return poolObject.transform.parent;
        }

        private bool IsObjectPooled(GameObject gameObject, out PooledObject pooledObject)
        {
            if (gameObject.TryGetComponent(out PooledObject targetObject))
            {
                pooledObject = targetObject;
                return true;
            }

            pooledObject = null;
            return false;
        }

        #endregion

        #region DataValidation

        private void ValidateInputData()
        {
            foreach (var poolPreferences in _poolsPreferences)
            {
                ValidateInitialSize(poolPreferences);

                ValidateMaxExpandSize(poolPreferences);
            }
        }

        private void ValidateInitialSize(PoolPreference poolPreference)
        {
            if (poolPreference.initialSize > poolPreference.maxExpandSize)
            {
                poolPreference.initialSize = poolPreference.maxExpandSize;
            }

            if (poolPreference.initialSize < 0)
            {
                poolPreference.initialSize = 0;
            }
        }

        private void ValidateMaxExpandSize(PoolPreference poolPreference)
        {
            if (poolPreference.maxExpandSize < poolPreference.initialSize)
            {
                poolPreference.maxExpandSize = poolPreference.initialSize;
            }

            if (poolPreference.maxExpandSize < 1)
            {
                poolPreference.maxExpandSize = 1;
            }
        }

        #endregion

        [Serializable]
        private class PoolPreference
        {
            [Tooltip("Type of pool.")]
            public T pool;
            [Tooltip("Prefab of pool object.")]
            public GameObject prefab;
            [Tooltip("Initial size of pool.")]
            public int initialSize = 5;
            [Tooltip("Should pool expand automatically?")]
            public bool autoExpand;
            [Tooltip("Max expanded size of pool.")]
            public int maxExpandSize = 20;
        }
    }
}
