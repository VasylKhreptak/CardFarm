using DG.Tweening;
using Graphics.Animations.Quests.QuestPanel;
using Quests.Logic;
using Runtime.Commands;
using Runtime.Map;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _quest;

        [FormerlySerializedAs("_questAppearAnimation")]
        [Header("Preferences")]
        [SerializeField] private QuestShowAnimation _questShowAnimation;
        [SerializeField] private QuestHideAnimation _questHideAnimation;

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
        }

        private void OnDisable()
        {
            StopObservingQuests();
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
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => UpdatePanelState()).AddTo(_questsSubscriptions);
        }

        private void StopObservingQuests()
        {
            _questsSubscriptions.Clear();
        }

        private void UpdatePanelState()
        {
            if (_questsManager.CurrentQuest.Value != null && _questsManager.CurrentNonRewardedQuest.Value == null)
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
            Disable();
            Enable();
            _questShowAnimation.Play();
        }

        private void Hide()
        {
            _questHideAnimation.Play(Disable);
        }

        private void Enable() => _quest.SetActive(true);

        private void Disable() => _quest.SetActive(false);

        private void OnSpawnedStarterCards()
        {
            StartObservingQuests();
        }

        private void OnRestart()
        {
            StopObservingQuests();
            Disable();
        }
    }
}
