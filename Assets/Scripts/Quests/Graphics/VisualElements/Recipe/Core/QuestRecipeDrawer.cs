using System;
using ObjectPoolers;
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
        [SerializeField] private QuestRecipePartPooler _objectPooler;

        private IDisposable _subscription;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
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

            SpawnCard(questData.Recipe.Result);
            Spawn(QuestRecipePart.EqualsSign);

            for (int i = 0; i < questData.Recipe.TargetCards.Count; i++)
            {
                SpawnCard(questData.Recipe.TargetCards[i]);
                
                if (i < questData.Recipe.TargetCards.Count - 1)
                {
                    Spawn(QuestRecipePart.PlusSign);
                }
            }
        }

        private void SpawnCard(QuestRecipeCardData cardData)
        {
            GameObject spawnedCard = Spawn(QuestRecipePart.Card);

            QuestRecipeCardDataHolder dataHolder = spawnedCard.GetComponent<QuestRecipeCardDataHolder>();

            dataHolder.CopyFrom(cardData);
        }

        private GameObject Spawn(QuestRecipePart part)
        {
            GameObject spawnedPart = _objectPooler.Spawn(part);

            spawnedPart.transform.SetAsLastSibling();
            
            spawnedPart.transform.localScale = Vector3.one;

            return spawnedPart;
        }

        private void ClearRecipe()
        {
            _objectPooler.DisableAllObjects();
        }
    }
}
