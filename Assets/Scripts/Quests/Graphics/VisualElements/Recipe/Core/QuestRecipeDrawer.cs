using System.Collections.Generic;
using Extensions;
using Graphics.Animations.Quests.QuestPanel;
using Quests.Data;
using Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data;
using Quests.Logic;
using ScriptableObjects.Scripts.Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements.Recipe.Core
{
    public class QuestRecipeDrawer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Component Preferences")]
        [SerializeField] private GameObject _equalsSign;
        [SerializeField] private GameObject _plusSign;
        [SerializeField] private GameObject _cardTemplate;

        [Header("Size Preferences")]
        [SerializeField] private Vector2 _summandSize;
        [SerializeField] private Vector2 _resultSize;

        private QuestRecipeCardDataHolder _currentRecipeResultCard;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private QuestsManager _questsManager;
        private DiContainer _container;
        private CardsData _cardsData;
        private QuestShowAnimation _questShowAnimation;

        [Inject]
        private void Constructor(QuestsManager questsManager,
            DiContainer container,
            CardsData cardsData,
            QuestShowAnimation questShowAnimation)
        {
            _questsManager = questsManager;
            _container = container;
            _cardsData = cardsData;
            _questShowAnimation = questShowAnimation;
        }

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        private void OnEnable()
        {
            StartObserving();

            _questShowAnimation.OnPlay += PunchResultedCard;
        }

        private void OnDisable()
        {
            StopObserving();
            ClearRecipe();

            _currentRecipeResultCard = null;
            _questShowAnimation.OnPlay -= PunchResultedCard;
        }

        private void StartObserving()
        {
            StopObserving();

            _questsManager.CurrentQuest.Subscribe(_ => TryDrawCurrentQuestRecipe()).AddTo(_subscriptions);
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => TryDrawCurrentQuestRecipe()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void TryDrawCurrentQuestRecipe()
        {
            QuestData currentQuest = _questsManager.CurrentQuest.Value;
            QuestData nonRewardedQuest = _questsManager.CurrentNonRewardedQuest.Value;

            if (currentQuest == null || nonRewardedQuest != null)
            {
                _currentRecipeResultCard = null;
                return;
            }

            ClearRecipe();

            if (currentQuest == null || currentQuest.Recipe.IsValid() == false) return;

            for (int i = 0; i < currentQuest.Recipe.TargetCards.Count; i++)
            {
                SpawnCard(currentQuest.Recipe.TargetCards[i], _summandSize);

                if (i < currentQuest.Recipe.TargetCards.Count - 1)
                {
                    SpawnPrefab(_plusSign);
                }
            }

            SpawnPrefab(_equalsSign);
            _currentRecipeResultCard = SpawnCard(currentQuest.Recipe.Result, _resultSize);
        }

        private QuestRecipeCardDataHolder SpawnCard(QuestRecipeCardData recipeCardData, Vector2 size)
        {
            GameObject spawnedCard = SpawnPrefab(_cardTemplate);

            QuestRecipeCardDataHolder dataHolder = spawnedCard.GetComponent<QuestRecipeCardDataHolder>();
            dataHolder.RectTransform.sizeDelta = size;

            if (_cardsData.TryGetValue(recipeCardData.Card, out var cardData))
            {
                dataHolder.Icon.Value = cardData.Icon;
                dataHolder.BackgroundColor.Value = cardData.BodyColor;
                dataHolder.Quantity.Value = recipeCardData.Quantity;
            }

            return dataHolder;
        }

        private GameObject SpawnPrefab(GameObject prefab)
        {
            GameObject instance = _container.InstantiatePrefab(prefab);

            instance.transform.SetParent(_transform, false);
            instance.transform.SetAsLastSibling();
            instance.transform.localScale = Vector3.one;

            return instance;
        }

        private void ClearRecipe()
        {
            List<GameObject> children = _transform.gameObject.GetChildren();

            foreach (var child in children)
            {
                Destroy(child);
            }
        }

        private void PunchResultedCard()
        {
            if (_currentRecipeResultCard == null) return;
            
            _currentRecipeResultCard.ScalePunchAnimation.Play();
        }
    }
}
