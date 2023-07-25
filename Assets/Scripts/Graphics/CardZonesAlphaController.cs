using Runtime.Commands;
using Runtime.Map;
using UnityEngine;
using Zenject;

namespace Graphics
{
    public class CardZonesAlphaController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _defaultAlpha = 0.5f;
        [SerializeField] private float _alphaAfterSpawnedStartedCards = 1f;

        private StarterCardsSpawner _starterCardsSpawner;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(StarterCardsSpawner starterCardsSpawner,
            GameRestartCommand gameRestartCommand)
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
            SetDefaultAlpha();
            _gameRestartCommand.OnExecute += SetDefaultAlpha;
            _starterCardsSpawner.OnSpawnedAllCards += SetTargetAlpha;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= SetDefaultAlpha;
            _starterCardsSpawner.OnSpawnedAllCards -= SetTargetAlpha;
        }

        #endregion

        private void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        private void SetDefaultAlpha() => SetAlpha(_defaultAlpha);

        private void SetTargetAlpha() => SetAlpha(_alphaAfterSpawnedStartedCards);
    }
}
