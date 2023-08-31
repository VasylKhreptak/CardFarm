using System.Collections.Generic;
using System.Linq;
using LevelUpPanel.Buttons;
using TreasureChests.Logic.Tags;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace TreasureChests.Logic
{
    public class ChestSelector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<ChestRaycastTarget> _raycastTargets;
        [SerializeField] private WatchAdButton _watchAdButton;
        [SerializeField] private NoThanksButton _noThanksButton;

        [Header("Preferences")]
        [SerializeField] private int _baseOpenedChestsCount = 1;
        [SerializeField] private int _maxChestsToOpen = 2;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private int _openedChestsCount = 0;
        private int _currentMaxChestsToOpen;

        #region MonoBehaviour

        private void OnValidate()
        {
            _raycastTargets ??= GetComponentsInChildren<ChestRaycastTarget>().ToList();
        }

        private void OnEnable()
        {
            _currentMaxChestsToOpen = _baseOpenedChestsCount;
            StartObserving();

            _watchAdButton.OnWatchedAd += OnWatchedAd;
        }

        private void OnDisable()
        {
            StopObserving();
            _openedChestsCount = 0;

            _watchAdButton.OnWatchedAd -= OnWatchedAd;
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
                    })
                    .AddTo(_subscriptions);
            }
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnSelected(ChestRaycastTarget raycastTarget)
        {
            if (raycastTarget.ChestData.IsOpened.Value) return;

            if (_openedChestsCount >= _currentMaxChestsToOpen) return;

            raycastTarget.ChestData.ChestRewardOpener.Open();

            _openedChestsCount++;

            if (_openedChestsCount == 1)
            {
                _watchAdButton.Show(1f);
                _noThanksButton.Show(2f);
            }

            if (_openedChestsCount >= 1 && _openedChestsCount < _currentMaxChestsToOpen)
            {
                _noThanksButton.Show(2f);
            }
        }

        private void OnWatchedAd()
        {
            _currentMaxChestsToOpen = _maxChestsToOpen;
            _watchAdButton.Hide();
            _noThanksButton.Hide();
        }
    }
}
