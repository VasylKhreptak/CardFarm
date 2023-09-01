using System;
using System.Collections.Generic;
using System.Linq;
using LevelUpPanel.Buttons;
using TreasureChests.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace TreasureChests.Logic
{
    public class ChestSelector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<UITreasureChestData> _uiChests;
        [SerializeField] private WatchAdButton _watchAdButton;
        [SerializeField] private NoThanksButton _noThanksButton;

        [Header("Preferences")]
        [SerializeField] private int _baseOpenedChestsCount = 1;
        [SerializeField] private int _maxChestsToOpen = 2;
        [SerializeField] private float _closeDelay = 1f;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private int _openedChestsCount = 0;
        private int _currentMaxChestsToOpen;
        private bool _watchedAd = false;
        private bool _openedAllChests = false;

        private LevelUpPanel.LevelUpPanel _levelUpPanel;

        [Inject]
        private void Constructor(LevelUpPanel.LevelUpPanel levelUpPanel)
        {
            _levelUpPanel = levelUpPanel;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _uiChests ??= GetComponentsInChildren<UITreasureChestData>().ToList();
        }

        private void Awake()
        {
            _noThanksButton.onClicked += OnCLickedOnNoThanksButton;
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
            _watchedAd = false;
            _openedAllChests = false;
        }

        private void OnDestroy()
        {
            OnCLickedOnNoThanksButton();
        }

        #endregion

        private void StartObserving()
        {
            foreach (var uiChest in _uiChests)
            {
                uiChest
                    .RaycastTarget
                    .OnPointerClickAsObservable()
                    .Subscribe(_ =>
                    {
                        OnSelected(uiChest);
                    })
                    .AddTo(_subscriptions);

                uiChest
                    .RaycastTarget
                    .OnPointerDownAsObservable()
                    .Subscribe(_ =>
                    {
                        OnPointerDown(uiChest);
                    })
                    .AddTo(_subscriptions);

                uiChest
                    .RaycastTarget
                    .OnPointerUpAsObservable()
                    .Subscribe(_ =>
                    {
                        OnPointerUp(uiChest);
                    })
                    .AddTo(_subscriptions);
            }
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnSelected(UITreasureChestData uiChest)
        {
            if (uiChest.ChestData.IsOpened.Value || _openedAllChests) return;

            if (_openedChestsCount >= _currentMaxChestsToOpen) return;

            uiChest.ChestData.ChestRewardOpener.Open();
            uiChest.TreasureController.ShowTreasure();

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

            if (_watchedAd && _openedChestsCount == _currentMaxChestsToOpen)
            {
                _watchAdButton.Hide();
                _noThanksButton.Hide();

                _openedAllChests = true;

                _levelUpPanel.Hide(2f, () =>
                {
                    foreach (var uiChest in _uiChests)
                    {
                        uiChest.TreasureController.SpawnTreasure();
                    }
                });
            }
        }

        private void OnWatchedAd()
        {
            _currentMaxChestsToOpen = _maxChestsToOpen;
            _watchAdButton.Hide();
            _noThanksButton.Hide();
            _watchedAd = true;
        }

        private void OnPointerDown(UITreasureChestData uiChest)
        {
            // uiChest.ChestData.ScaleReleaseAnimation.Stop();
            // uiChest.ChestData.ScalePressAnimation.PlayForwardImmediate();
        }

        private void OnPointerUp(UITreasureChestData uiChest)
        {
            // uiChest.ChestData.ScalePressAnimation.Stop();
            // uiChest.ChestData.ScaleReleaseAnimation.PlayForwardImmediate();
        }

        private void OnCLickedOnNoThanksButton()
        {
            _watchAdButton.Hide();
            _noThanksButton.Hide();

            _openedAllChests = true;

            _levelUpPanel.Hide(0.5f, () =>
            {
                foreach (var uiChest in _uiChests)
                {
                    uiChest.TreasureController.SpawnTreasure();
                }
            });
        }
    }
}
