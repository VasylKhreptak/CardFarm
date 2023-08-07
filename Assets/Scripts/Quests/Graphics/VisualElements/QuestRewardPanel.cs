using System;
using DG.Tweening;
using Quests.Logic;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestRewardPanel : MonoBehaviour
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

        private IDisposable _nonRewardedQuestSubscription;

        private GameRestartCommand _gameRestartCommand;
        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand,
            QuestsManager questsManager)
        {
            _gameRestartCommand = gameRestartCommand;
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnEnable()
        {
            StartObserving();
            Disable();
            SetParameters(_startScale, 0f);
        }

        private void OnDisable()
        {
            StopObserving();
            KillAnimation();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _nonRewardedQuestSubscription = _questsManager.NonRewardedQuests
                .ObserveCountChanged()
                .DoOnSubscribe(UpdatePanelState)
                .Subscribe(_ => UpdatePanelState());
        }

        private void StopObserving()
        {
            _nonRewardedQuestSubscription?.Dispose();
        }

        private void UpdatePanelState()
        {
            int nonRewardedQuestsCount = _questsManager.NonRewardedQuests.Count;

            if (nonRewardedQuestsCount > 0)
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
            SetParameters(_startScale, 0f);
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

        private void SetParameters(Vector3 scale, float alpha)
        {
            _canvasGroup.alpha = alpha;
            _quest.transform.localScale = scale;
        }

        private void KillAnimation()
        {
            _showSequence?.Kill();
        }

        private void OnRestart()
        {
            SetParameters(_startScale, 0f);
            Disable();
        }
    }
}
