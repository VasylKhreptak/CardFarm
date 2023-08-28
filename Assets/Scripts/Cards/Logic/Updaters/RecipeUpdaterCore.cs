using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public abstract class RecipeUpdaterCore<T> : MonoBehaviour, IValidatable where T : CardData
    {
        [Header("References")]
        [SerializeField] protected T _cardData;

        private IDisposable _isTopCardSubscription;
        private IDisposable _isCardSingleSubscription;
        private CompositeDisposable _environmentSubscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<T>(true);
        }

        private void OnEnable()
        {
            OnBecameHeadOfGroup();
            _cardData.Callbacks.onBecameHeadOfGroup += OnBecameHeadOfGroup;
            _isTopCardSubscription = _cardData.IsTopCard.Where(x => x == false).Subscribe(_ => ResetCurrentRecipe());
            _isCardSingleSubscription = _cardData.IsSingleCard.Where(x => x).Subscribe(_ => ResetCurrentRecipe());
        }

        protected virtual void OnDisable()
        {
            _cardData.Callbacks.onBecameHeadOfGroup -= OnBecameHeadOfGroup;
            _isTopCardSubscription?.Dispose();
            _isCardSingleSubscription?.Dispose();
            _environmentSubscriptions.Clear();
            ResetCurrentRecipe();
        }

        #endregion

        protected virtual void OnBecameHeadOfGroup()
        {
            _environmentSubscriptions?.Clear();

            _cardData.IsTopCard.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_environmentSubscriptions);
            _cardData.IsSingleCard.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_environmentSubscriptions);
        }

        private void OnEnvironmentUpdated()
        {
            if (_cardData.IsTopCard.Value == false || _cardData.IsSingleCard.Value)
            {
                ResetCurrentRecipe();
                return;
            }

            UpdateRecipe();
        }

        protected abstract void ResetCurrentRecipe();

        protected abstract void UpdateRecipe();
    }
}
