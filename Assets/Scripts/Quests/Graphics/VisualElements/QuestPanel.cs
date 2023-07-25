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

        private IDisposable _currentQuestSubscription;

        private CurrentQuestCompletionObserver _currentQuestCompletionObserver;
        private GameRestartCommand _gameRestartCommand;
        private StarterCardsSpawner _starterCardsSpawner;

        [Inject]
        private void Constructor(CurrentQuestCompletionObserver currentQuestCompletionObserver,
            GameRestartCommand gameRestartCommand,
            StarterCardsSpawner starterCardsSpawner)
        {
            _currentQuestCompletionObserver = currentQuestCompletionObserver;
            _gameRestartCommand = gameRestartCommand;
            _starterCardsSpawner = starterCardsSpawner;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
            _starterCardsSpawner.OnSpawnedAllCards += OnSpawnedStarterCards;
        }

        private void OnEnable()
        {
            StartObservingQuestCompletion();
            Disable();
            SetScale(_startScale);
            SetAlpha(0f);
        }

        private void OnDisable()
        {
            StopObservingQuestCompletion();
            KillAnimation();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
            _starterCardsSpawner.OnSpawnedAllCards -= OnSpawnedStarterCards;
        }

        #endregion

        private void StartObservingQuestCompletion()
        {
            StopObservingQuestCompletion();
            _currentQuestSubscription = _currentQuestCompletionObserver.IsCurrentQuestCompleted.Subscribe(UpdateQuestState);
        }

        private void StopObservingQuestCompletion()
        {
            _currentQuestSubscription?.Dispose();
        }

        private void UpdateQuestState(bool isCurrentQuestCompleted)
        {
            if (isCurrentQuestCompleted)
            {
                Hide();
            }
            else
            {
                Show();
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
            StartObservingQuestCompletion();
        }

        private void OnRestart()
        {
            Disable();
            StopObservingQuestCompletion();
        }
    }
}
