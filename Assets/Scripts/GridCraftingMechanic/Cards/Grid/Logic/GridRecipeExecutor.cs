using System;
using System.Linq;
using Cards.Data;
using Cards.Logic.Spawn;
using UniRx;
using UnityEngine;
using Zenject;

namespace GridCraftingMechanic.Cards.Grid.Logic
{
    public class GridRecipeExecutor : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CraftingGridCardData _cardData;

        private CompositeDisposable _cellsSubscriptions = new CompositeDisposable();

        private IDisposable _frameDelaySubscription;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CraftingGridCardData>(true);
        }

        private void OnEnable()
        {
            ResetCells();
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _frameDelaySubscription?.Dispose();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            foreach (var gridCell in _cardData.GridCells)
            {
                gridCell.ContainsTargetCard.Subscribe(_ => OnEnvironmentChanged()).AddTo(_cellsSubscriptions);
            }
        }

        private void StopObserving()
        {
            _cellsSubscriptions?.Clear();
        }

        private void OnEnvironmentChanged()
        {
            bool isRecipeMatched = _cardData.GridCells.All(x => x.ContainsTargetCard.Value);

            if (isRecipeMatched)
            {
                StopObserving();
                
                _frameDelaySubscription?.Dispose();
                _frameDelaySubscription = Observable.NextFrame().Subscribe(_ =>
                {
                    ResetCells();
                    
                    SpawnTargetCard();
                    
                    StartObserving();
                });
            }
        }

        private void SpawnTargetCard()
        {
            CardData spawnedCard = _cardSpawner.Spawn(_cardData.GridRecipe.Value.RecipeResult, _cardData.transform.position);

            spawnedCard.Animations.JumpAnimation.PlayRandomly();
        }

        private void ResetCells()
        {
            foreach (var cell in _cardData.GridCells)
            {
                cell.ContainsTargetCard.Value = false;
            }
        }
    }
}
