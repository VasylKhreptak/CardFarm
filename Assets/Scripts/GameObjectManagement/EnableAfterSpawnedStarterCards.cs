using Runtime.Map;
using UnityEngine;
using Zenject;

namespace GameObjectManagement
{
    public class EnableAfterSpawnedStarterCards : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        private StarterCardsSpawner _starterCardsSpawner;

        [Inject]
        private void Constructor(StarterCardsSpawner starterCardsSpawner)
        {
            _starterCardsSpawner = starterCardsSpawner;
        }

        #region MonoBehaviour

        private void Awake()
        {
            Disable();
        }

        private void OnEnable()
        {
            _starterCardsSpawner.OnSpawnedAllCards += Enable;
        }

        private void OnDisable()
        {
            _starterCardsSpawner.OnSpawnedAllCards -= Enable;
        }

        #endregion

        private void Enable() => _gameObject.SetActive(true);

        private void Disable() => _gameObject.SetActive(false);
    }
}
