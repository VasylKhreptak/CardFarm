using System.Collections.Generic;
using Extensions;
using Quests.Data;
using Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data;
using Quests.Logic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements.Recipe.Core
{
    public class QuestRecipeDrawer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private GameObject _equalsSign;
        [SerializeField] private GameObject _plusSign;
        [SerializeField] private GameObject _cardTemplate;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private QuestsManager _questsManager;
        private DiContainer _container;

        [Inject]
        private void Constructor(QuestsManager questsManager, DiContainer container)
        {
            _questsManager = questsManager;
            _container = container;
        }

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            ClearRecipe();
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

            if (currentQuest == null || nonRewardedQuest != null) return;

            ClearRecipe();

            if (currentQuest == null || currentQuest.Recipe.IsValid() == false) return;

            for (int i = 0; i < currentQuest.Recipe.TargetCards.Count; i++)
            {
                SpawnCard(currentQuest.Recipe.TargetCards[i]);

                if (i < currentQuest.Recipe.TargetCards.Count - 1)
                {
                    SpawnPrefab(_plusSign);
                }
            }

            SpawnPrefab(_equalsSign);
            SpawnCard(currentQuest.Recipe.Result);
        }

        private void SpawnCard(QuestRecipeCardData cardData)
        {
            GameObject spawnedCard = SpawnPrefab(_cardTemplate);

            QuestRecipeCardDataHolder dataHolder = spawnedCard.GetComponent<QuestRecipeCardDataHolder>();

            dataHolder.CopyFrom(cardData);
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
    }
}
