using System;
using Cards.Data;
using CardsTable.PoolLogic;
using Quests.Data;
using Quests.Logic;
using Quests.Logic.Core;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardRestartIconDrawer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _height = 1f;

        private CompositeDisposable _subscriptions = new CompositeDisposable();
        private IDisposable _positionUpdateSubscription;

        private GameObject _restartIconObject;

        private CardTablePooler _cardTablePooler;
        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(CardTablePooler cardTablePooler, QuestsManager questsManager)
        {
            _cardTablePooler = cardTablePooler;
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            StartObservingCardData();
        }

        private void OnDisable()
        {
            StopObservingCardData();
            StopDrawing();
        }

        #endregion

        private void StartObservingCardData()
        {
            StopObservingCardData();

            _questsManager.CurrentQuest.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.CurrentRecipe.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.RecipeExecutor.Progress.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);

            _questsManager.NonRewardedQuests.ObserveCountChanged()
                .DoOnSubscribe(OnEnvironmentChanged)
                .Subscribe(_ => OnEnvironmentChanged())
                .AddTo(_subscriptions);
        }

        private void StopObservingCardData()
        {
            _subscriptions?.Clear();
        }

        private void OnEnvironmentChanged()
        {
            QuestData currentQuest = _questsManager.CurrentQuest.Value;
            CardRecipe currentRecipe = _cardData.CurrentRecipe.Value;
            float progress = _cardData.RecipeExecutor.Progress.Value;
            int nonRewardedQuestsCount = _questsManager.NonRewardedQuests.Count;

            bool canDraw =
                currentQuest != null
                && currentRecipe != null
                && currentQuest.Quest == Quest.RestartRecipe
                && Mathf.Approximately(progress, 0)
                && nonRewardedQuestsCount == 0;

            if (canDraw)
            {
                StartDrawing();
            }
            else
            {
                StopDrawing();
            }
        }

        private void StartDrawing()
        {
            StopDrawing();
            GameObject restartIcon = _cardTablePooler.Spawn(CardTablePool.RestartIcon);

            _restartIconObject = restartIcon;

            restartIcon.transform.localRotation = Quaternion.identity;

            _positionUpdateSubscription = _cardData
                .GroupCenter
                .DoOnSubscribe(() =>
                {
                    UpdateSortingLayer();
                    UpdatePosition();
                })
                .Subscribe(_ =>
                {
                    UpdateSortingLayer();
                    UpdatePosition();
                });
        }

        private void UpdatePosition()
        {
            if (_restartIconObject == null) return;

            Vector3 position = _cardData.GroupCenter.Value;
            position.y = _height;

            _restartIconObject.transform.position = position;
        }

        private void UpdateSortingLayer()
        {
            if (_restartIconObject == null) return;

            CardData lastGroupCard = _cardData.LastGroupCard.Value;

            if (lastGroupCard == null)
            {
                _restartIconObject.transform.SetAsLastSibling();
            }
            else
            {
                _restartIconObject.transform.SetSiblingIndex(lastGroupCard.transform.GetSiblingIndex() + 1);
            }
        }

        private void StopDrawing()
        {
            if (_restartIconObject != null)
            {
                _restartIconObject.SetActive(false);
                _restartIconObject = null;
            }

            _positionUpdateSubscription?.Dispose();
        }
    }
}
