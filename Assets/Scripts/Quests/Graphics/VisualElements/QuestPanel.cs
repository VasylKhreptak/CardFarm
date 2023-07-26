using System;
using DG.Tweening;
using Quests.Logic;
using Runtime.Commands;
using Runtime.Map;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _quest;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Fade Preferences")]
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _targetScale = Vector3.one;
        [SerializeField] private AnimationCurve _scaleCurve;

        private Sequence _showSequence;

        private CompositeDisposable _questsSubscriptions = new CompositeDisposable();

        private GameRestartCommand _gameRestartCommand;
        private StarterCardsSpawner _starterCardsSpawner;
        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand,
            StarterCardsSpawner starterCardsSpawner,
            QuestsManager questsManager)
        {
            _gameRestartCommand = gameRestartCommand;
            _starterCardsSpawner = starterCardsSpawner;
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
            _starterCardsSpawner.OnSpawnedAllCards += OnSpawnedStarterCards;
        }

        private void OnEnable()
        {
            Disable();
            SetScale(_startScale);
            SetAlpha(0f);
        }

        private void OnDisable()
        {
            StopObservingQuests();
            KillAnimation();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
            _starterCardsSpawner.OnSpawnedAllCards -= OnSpawnedStarterCards;
        }

        #endregion

        private void StartObservingQuests()
        {
            StopObservingQuests();

            _questsManager.CurrentQuest.Subscribe(_ => UpdatePanelState()).AddTo(_questsSubscriptions);
            _questsManager.NonRewardedQuests
                .ObserveCountChanged()
                .DoOnSubscribe(UpdatePanelState)
                .Subscribe(_ => UpdatePanelState())
                .AddTo(_questsSubscriptions);
        }

        private void StopObservingQuests()
        {
            _questsSubscriptions.Clear();
        }

        private void UpdatePanelState()
        {
            if (_questsManager.CurrentQuest.Value != null && _questsManager.NonRewardedQuests.Count == 0)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            Enable();
            SetParametersSmooth(_targetScale, 1f);
        }

        private void Hide()
        {
            SetParametersSmooth(_startScale, 0f, Disable);
        }

        private void Enable()
        {
            _quest.SetActive(true);
        }

        private void Disable()
        {
            _quest.SetActive(false);
        }

        private void SetParametersSmooth(Vector3 scale, float alpha, Action onComplete = null)
        {
            KillAnimation();

            _showSequence = DOTween.Sequence();
            _showSequence.Append(_canvasGroup.DOFade(alpha, _fadeDuration).SetEase(_fadeCurve));
            _showSequence.Join(_quest.transform.DOScale(scale, _fadeDuration).SetEase(_scaleCurve));
            _showSequence.OnComplete(() => onComplete?.Invoke());
            _showSequence.Play();
        }

        private void KillAnimation()
        {
            _showSequence?.Kill();
        }

        private void SetScale(Vector3 scale)
        {
            _quest.transform.localScale = scale;
        }

        private void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        private void OnSpawnedStarterCards()
        {
            SetScale(_startScale);
            SetAlpha(0f);
            StartObservingQuests();
        }

        private void OnRestart()
        {
            Disable();
            SetScale(_startScale);
            SetAlpha(0f);
            StopObservingQuests();
        }
    }
}
