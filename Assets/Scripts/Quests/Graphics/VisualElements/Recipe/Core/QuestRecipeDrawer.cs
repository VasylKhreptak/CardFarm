using System;
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

        private IDisposable _subscription;

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
            _subscription = _questsManager.CurrentQuest.Subscribe(TryDrawQuestRecipe);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void TryDrawQuestRecipe(QuestData questData)
        {
            ClearRecipe();

            if (questData == null || questData.Recipe.IsValid() == false) return;

            for (int i = 0; i < questData.Recipe.TargetCards.Count; i++)
            {
                SpawnCard(questData.Recipe.TargetCards[i]);

                if (i < questData.Recipe.TargetCards.Count - 1)
                {
                    SpawnPrefab(_plusSign);
                }
            }

            SpawnPrefab(_equalsSign);
            SpawnCard(questData.Recipe.Result);
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
