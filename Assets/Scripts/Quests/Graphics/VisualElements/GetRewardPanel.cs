using Graphics.Animations.Quests.RewardPanel;
using Graphics.Animations.Reminder;
using Quests.Logic;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using UnlockedCardPanel.Graphics.VisualElements;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class GetRewardPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _quest;

        [Header("Animations")]
        [SerializeField] private RewardPanelShowAnimation _showAnimation;
        [SerializeField] private RewardPanelHideAnimation _hideAnimation;
        [SerializeField] private ScalePunchReminderAnimation _scalePunchReminder;
        [SerializeField] private AnchorPositionPunchReminderAnimation _positionPunchReminder;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private BoolReactiveProperty _isActive = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

        private GameRestartCommand _gameRestartCommand;
        private QuestsManager _questsManager;
        private NewCardPanel _newCardPanel;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand,
            QuestsManager questsManager,
            NewCardPanel newCardPanel)
        {
            _gameRestartCommand = gameRestartCommand;
            _questsManager = questsManager;
            _newCardPanel = newCardPanel;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _showAnimation ??= GetComponentInChildren<RewardPanelShowAnimation>(true);
            _hideAnimation ??= GetComponentInChildren<RewardPanelHideAnimation>(true);
            _positionPunchReminder ??= GetComponentInChildren<AnchorPositionPunchReminderAnimation>(true);
            _scalePunchReminder ??= GetComponentInChildren<ScalePunchReminderAnimation>(true);
        }

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnEnable()
        {
            StartObserving();
            Disable();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _questsManager.NonRewardedQuests
                .ObserveCountChanged()
                .DoOnSubscribe(UpdatePanelState)
                .Subscribe(_ => UpdatePanelState())
                .AddTo(_subscriptions);

            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => UpdatePanelState()).AddTo(_subscriptions);

            _newCardPanel.IsActive.Subscribe(_ =>
            {
                UpdatePanelState();
                OnNewCardPanelUpdated();
            }).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void UpdatePanelState()
        {
            if (_questsManager.NonRewardedQuests.Count > 0 && _questsManager.CurrentNonRewardedQuest.Value != null)
            {
                if (_newCardPanel.IsActive.Value == false && IsEnabled() == false)
                {
                    Show();
                }
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            Enable();

            _hideAnimation.Stop();

            _showAnimation.Play(() =>
            {
                _positionPunchReminder.Play();
                _scalePunchReminder.Play();
            });
        }

        private void Hide()
        {
            _showAnimation.Stop();
            _positionPunchReminder.Stop();
            _scalePunchReminder.Stop();
            _hideAnimation.Play(Disable);
        }

        private void Enable()
        {
            _quest.SetActive(true);
            _isActive.Value = true;
        }

        private void Disable()
        {
            _quest.SetActive(false);
            _isActive.Value = false;
        }

        private bool IsEnabled() => _quest.activeSelf;

        private void OnRestart()
        {
            Disable();
            _showAnimation.Stop();
            _positionPunchReminder.Stop();
            _scalePunchReminder.Stop();
            _hideAnimation.Stop();
        }

        private void OnNewCardPanelUpdated()
        {
            bool isActive = _newCardPanel.IsActive.Value;

            if (isActive)
            {
                _positionPunchReminder.SetStartPositionSmoothly();
                _scalePunchReminder.SetStartScaleSmoothly();
            }
            else if (IsEnabled())
            {
                _positionPunchReminder.Play();
                _scalePunchReminder.Play();
            }
        }
    }
}
