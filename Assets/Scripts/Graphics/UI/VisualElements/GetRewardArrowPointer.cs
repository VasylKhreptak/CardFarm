using Graphics.Animations.UI;
using Quests.Graphics.VisualElements;
using Quests.Logic;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using UnlockedCardPanel.Graphics.VisualElements;
using Zenject;

namespace Graphics.UI.VisualElements
{
    public class GetRewardArrowPointer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ArrowPointerAnimation _arrowPointerAnimation;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private QuestsManager _questsManager;
        private GameRestartCommand _gameRestartCommand;
        private NewCardPanel _newCardPanel;
        private GetRewardPanel _getRewardPanel;

        [Inject]
        private void Constructor(QuestsManager questsManager,
            GameRestartCommand gameRestartCommand,
            NewCardPanel newCardPanel,
            GetRewardPanel getRewardPanel)
        {
            _questsManager = questsManager;
            _gameRestartCommand = gameRestartCommand;
            _newCardPanel = newCardPanel;
            _getRewardPanel = getRewardPanel;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnValidate()
        {
            _arrowPointerAnimation ??= GetComponentInChildren<ArrowPointerAnimation>(true);
        }

        private void OnEnable()
        {
            StartObserving();
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

            _questsManager.RewardedQuests
                .ObserveCountChanged()
                .DoOnSubscribe(OnEnvironmentUpdated)
                .Subscribe(_ => OnEnvironmentUpdated());

            _newCardPanel.IsActive.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
            _getRewardPanel.IsActive.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void OnEnvironmentUpdated()
        {
            bool canShow =
                _questsManager.RewardedQuests.Count == 0
                && _newCardPanel.IsActive.Value == false
                && _getRewardPanel.IsActive.Value;

            if (canShow)
            {
                _arrowPointerAnimation.Play();
            }
            else
            {
                _arrowPointerAnimation.Stop();
            }
        }

        private void OnRestart()
        {
            _arrowPointerAnimation.gameObject.SetActive(false);
        }
    }
}
