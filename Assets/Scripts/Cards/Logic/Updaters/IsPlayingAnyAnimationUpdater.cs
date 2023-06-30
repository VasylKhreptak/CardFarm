using System;
using System.Linq;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsPlayingAnyAnimationUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private CardAnimation[] _animations;

        private IDisposable _subscription;

        #region MonoBehaviour

        public void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);

            if (_cardData == null) return;

            _animations = _cardData.GetComponentsInChildren<CardAnimation>();
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _subscription = _animations.Select(x => x.IsPlaying).Merge().Subscribe(_ =>
            {
                OnAnimationStatesUpdated();
            });
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnAnimationStatesUpdated()
        {
            _cardData.IsPlayingAnyAnimation.Value = _animations.Any(x => x.IsPlaying.Value);
        }
    }
}
