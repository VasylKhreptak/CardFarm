using System.Collections.Generic;
using System.Linq;
using Graphics.UI.Particles.Coins.Logic;
using TreasureChests.Logic.Tags;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace TreasureChests.Logic
{
    public class ChestSelector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<ChestRaycastTarget> _raycastTargets;

        [Header("Preferences")]
        [SerializeField] private int _baseOpenedChestsCount = 1;
        [SerializeField] private int _maxOpenedChestsCount = 2;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private int _openedChestsCount = 0;

        private CoinsCollector _coinsCollector;

        [Inject]
        private void Constructor(CoinsCollector coinsCollector)
        {
            _coinsCollector = coinsCollector;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _raycastTargets ??= GetComponentsInChildren<ChestRaycastTarget>().ToList();
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _openedChestsCount = 0;
        }

        #endregion

        private void StartObserving()
        {
            foreach (var raycastTarget in _raycastTargets)
            {
                raycastTarget
                    .UIBehaviour
                    .OnPointerClickAsObservable()
                    .Subscribe(_ =>
                    {
                        OnSelected(raycastTarget);
                    });
            }
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnSelected(ChestRaycastTarget raycastTarget)
        {
            raycastTarget.ChestData.StateAnimation.Open();
        }
    }
}
