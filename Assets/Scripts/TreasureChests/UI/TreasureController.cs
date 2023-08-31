using CameraManagement.CameraAim.Core;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using CBA.Extensions;
using Constraints.CardTable;
using DG.Tweening;
using Graphics.UI.Particles.Coins.Logic;
using ScriptableObjects.Scripts.Cards.Data;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.Data;
using Zenject;
using RectTransform = UnityEngine.RectTransform;
using Vector3 = UnityEngine.Vector3;

namespace TreasureChests.UI
{
    public class TreasureController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private UITreasureChestData _chestData;

        [Header("Preferences")]
        [SerializeField] private RectTransform _visualizableCard;
        [SerializeField] private RectTransform _coin;

        [Header("Animation Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Vector3 _startScale = Vector3.zero * 0.1f;
        [SerializeField] private Vector3 _endScale = Vector3.one;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private float _startAlpha = 0.3f;
        [SerializeField] private float _endAlpha = 1f;
        [SerializeField] private AnimationCurve _fadeCurve;
        [SerializeField] private Vector2 _startAnchoredPosition;
        [SerializeField] private Vector2 _endAnchoredPosition;

        private Card? _shownTreasure;

        private Sequence _sequence;

        private CardsData _cardsData;
        private CoinsCollector _coinsCollector;
        private CameraAimer _cameraAimer;
        private CardSpawner _cardSpawner;
        private PlayingAreaTableBounds _playingAreaTableBounds;

        [Inject]
        private void Constructor(CardsData cardsData,
            CoinsCollector coinsCollector,
            CameraAimer cameraAimer,
            CardSpawner cardSpawner,
            PlayingAreaTableBounds playingAreaTableBounds)
        {
            _cardsData = cardsData;
            _coinsCollector = coinsCollector;
            _cameraAimer = cameraAimer;
            _cardSpawner = cardSpawner;
            _playingAreaTableBounds = playingAreaTableBounds;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _chestData = GetComponentInParent<UITreasureChestData>(true);
        }

        private void OnDisable()
        {
            KillSequence();
            _shownTreasure = null;

            _coin.gameObject.SetActive(false);
            _visualizableCard.gameObject.SetActive(false);
        }

        #endregion

        public void ShowTreasure()
        {
            _shownTreasure = _chestData.PossibleTreasureCard.Random();

            RectTransform targetTreasure = null;

            if (_shownTreasure == Card.Coin)
            {
                _coin.gameObject.SetActive(true);
                _visualizableCard.gameObject.SetActive(false);
                targetTreasure = _coin;
            }
            else
            {
                _coin.gameObject.SetActive(false);
                _visualizableCard.gameObject.SetActive(true);
                _cardsData.TryGetValue(_shownTreasure.Value, out var cardData);
                _visualizableCard.GetComponentInChildren<VisualizableCardData>(true).VisualizableCard.Value = cardData;
                targetTreasure = _visualizableCard;
            }

            CanvasGroup canvasGroup = targetTreasure.GetComponent<CanvasGroup>();

            canvasGroup.alpha = _startAlpha;
            targetTreasure.transform.localScale = _startScale;
            targetTreasure.anchoredPosition = _startAnchoredPosition;

            KillSequence();
            _sequence = DOTween.Sequence();
            _sequence
                .Join(canvasGroup.DOFade(_endAlpha, _duration).SetEase(_fadeCurve))
                .Join(targetTreasure.transform.DOScale(_endScale, _duration).SetEase(_scaleCurve))
                .Join(targetTreasure.DOAnchorPos(_endAnchoredPosition, _duration).SetEase(_scaleCurve))
                .Play();
        }

        public void SpawnTreasure()
        {
            if (_shownTreasure == null) return;

            if (_shownTreasure == Card.Coin)
            {
                _coinsCollector.Collect(1, _coin.position);
            }
            else
            {
                CardData spawnedCard = _cardSpawner.Spawn(_shownTreasure.Value, _playingAreaTableBounds.transform.position);

                _cameraAimer.Aim(spawnedCard.transform);
            }

            _shownTreasure = null;
        }

        private void KillSequence() => _sequence?.Kill();
    }
}
