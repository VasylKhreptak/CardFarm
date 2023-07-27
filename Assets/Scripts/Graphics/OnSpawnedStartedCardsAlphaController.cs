using Runtime.Commands;
using Runtime.Map;
using UnityEngine;
using Zenject;

namespace Graphics
{
    public class OnSpawnedStartedCardsAlphaController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _startAlpha = 0.5f;
        [SerializeField] private float _targetAlpha = 1f;

        private StarterCardsSpawner _starterCardsSpawner;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(StarterCardsSpawner starterCardsSpawner, GameRestartCommand gameRestartCommand)
        {
            _starterCardsSpawner = starterCardsSpawner;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        private void Awake()
        {
            SetStartAlpha();
            _gameRestartCommand.OnExecute += OnRestart;
            _starterCardsSpawner.OnSpawnedAllCards += SetTargetAlpha;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
            _starterCardsSpawner.OnSpawnedAllCards -= SetTargetAlpha;
        }

        #endregion

        private void OnRestart()
        {
            SetStartAlpha();
        }

        private void SetStartAlpha() => _canvasGroup.alpha = _startAlpha;

        private void SetTargetAlpha() => _canvasGroup.alpha = _targetAlpha;
    }
}
