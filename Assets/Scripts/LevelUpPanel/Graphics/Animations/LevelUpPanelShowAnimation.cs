using System;
using DG.Tweening;
using TreasureChests.Data;
using UnityEngine;

namespace LevelUpPanel.Graphics.Animations
{
    public class LevelUpPanelShowAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;
        [SerializeField] private RectTransform _levelUpTextTransform;
        [SerializeField] private GameObject _addButtonObject;
        [SerializeField] private GameObject _noThanksButtonObject;
        [SerializeField] private CanvasGroup _mainCanvasGroup;

        [Header("Level Up Text Animation Preferences")]
        [SerializeField] private float _levelUpTextAnimationDelay;
        [SerializeField] private Vector2 _startLevelUpTextPosition;
        [SerializeField] private Vector2 _targetLevelUpTextPosition;
        [SerializeField] private float _levelUpTextMoveDuration;
        [SerializeField] private AnimationCurve _levelUpTextMoveCurve;

        [Header("Background Fade Preferences")]
        [SerializeField] private float _backgroundFadeDuration;
        [SerializeField] private float _startBackgroundAlpha;
        [SerializeField] private float _endBackgroundAlpha;
        [SerializeField] private AnimationCurve _backgroundFadeCurve;

        [Header("Chests Popup Animation Preferences")]
        [SerializeField] private Chest[] _chests;
        [SerializeField] private float _chestPopupDelay;
        [SerializeField] private float _chestPopupDuration;
        [SerializeField] private float _startChestAlpha;
        [SerializeField] private float _targetChestAlpha = 1;
        [SerializeField] private AnimationCurve _chestFadeCurve;
        [SerializeField] private float _chestInterval;
        [SerializeField] private Vector3 _startChestScale;
        [SerializeField] private Vector3 _targetChestScale;
        [SerializeField] private AnimationCurve _chestScaleCurve;
        [SerializeField] private int _chestSpinCount = 1;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(Action onComplete = null)
        {
            Stop();

            SetStartValues();

            _sequence = DOTween.Sequence();

            _sequence
                .Join(_backgroundCanvasGroup
                    .DOFade(_endBackgroundAlpha, _backgroundFadeDuration)
                    .SetEase(_backgroundFadeCurve))
                .Join(_mainCanvasGroup
                    .DOFade(1f, _backgroundFadeDuration)
                    .SetEase(_backgroundFadeCurve))
                .Join(_levelUpTextTransform
                    .DOAnchorPos(_targetLevelUpTextPosition, _levelUpTextMoveDuration)
                    .SetEase(_levelUpTextMoveCurve)
                    .SetDelay(_levelUpTextAnimationDelay))
                .Join(CreateChestsSequence())
                .OnPlay(() =>
                {
                    _mainCanvasGroup.interactable = false;
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    _mainCanvasGroup.interactable = true;
                })
                .OnKill(() =>
                {
                    _mainCanvasGroup.interactable = true;
                })
                .Play();

        }

        public void Stop()
        {
            _sequence?.Kill();
        }

        public void SetStartValues()
        {
            _backgroundCanvasGroup.alpha = _startBackgroundAlpha;
            _levelUpTextTransform.anchoredPosition = _startLevelUpTextPosition;
            _addButtonObject.SetActive(false);
            _noThanksButtonObject.SetActive(false);

            SetActiveChests(false);

            foreach (var chest in _chests)
            {
                chest.VisualObject.GetComponent<CanvasGroup>().alpha = _startChestAlpha;
                chest.VisualObject.transform.localScale = _startChestScale;
            }
        }

        public void SetActiveChests(bool isActive)
        {
            foreach (var chestObject in _chests)
            {
                chestObject.VisualObject.SetActive(isActive);
            }
        }

        private Sequence CreateChestsSequence()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.SetDelay(_chestPopupDelay);

            for (int i = 0; i < _chests.Length; i++)
            {
                TreasureChestData chestData = _chests[i].ChestData;
                GameObject chestObject = _chests[i].VisualObject;
                CanvasGroup chestCanvasGroup = chestObject.GetComponent<CanvasGroup>();
                sequence
                    .Join(chestObject.transform
                        .DOScale(_targetChestScale, _chestPopupDuration)
                        .SetEase(_chestScaleCurve)
                        .SetDelay(_chestInterval))
                    .Join(chestCanvasGroup
                        .DOFade(_targetChestAlpha, _chestPopupDuration)
                        .SetEase(_chestFadeCurve)
                        .SetDelay(_chestInterval)
                        .OnPlay(() =>
                        {
                            chestObject.SetActive(true);
                            chestData.StateAnimation.SetStateImmediately(false);
                            chestData.SpinAnimation.Play(_chestSpinCount);
                        }));
            }

            return sequence;
        }

        [Serializable]
        public class Chest
        {
            public GameObject VisualObject;
            public TreasureChestData ChestData;
        }
    }
}
