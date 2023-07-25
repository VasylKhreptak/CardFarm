using Runtime.Commands;
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
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(StarterCardsSpawner starterCardsSpawner, GameRestartCommand gameRestartCommand)
        {
            _starterCardsSpawner = starterCardsSpawner;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            Disable();
            _gameRestartCommand.OnExecute += Disable;
        }

        private void OnEnable()
        {
            _starterCardsSpawner.OnSpawnedAllCards += Enable;
        }

        private void OnDisable()
        {
            _starterCardsSpawner.OnSpawnedAllCards -= Enable;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= Disable;
        }

        #endregion

        private void Enable() => _gameObject.SetActive(true);

        private void Disable() => _gameObject.SetActive(false);
    }
}
