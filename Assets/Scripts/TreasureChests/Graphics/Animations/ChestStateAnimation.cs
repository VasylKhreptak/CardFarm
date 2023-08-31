using System;
using DG.Tweening;
using NaughtyAttributes;
using TreasureChests.Data;
using UnityEngine;
using Zenject;

namespace TreasureChests.Graphics.Animations
{
    public class ChestStateAnimation : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private TreasureChestData _chestData;

        [Header("Preferences")]
        [SerializeField] private Vector3 _closedRotation;
        [SerializeField] private Vector3 _openedRotation;
        [SerializeField] private float _openDuration;
        [SerializeField] private float _closeDuration;
        [SerializeField] private AnimationCurve _openCurve;
        [SerializeField] private AnimationCurve _closeCurve;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _chestData = GetComponentInParent<TreasureChestData>(true);
        }

        private void OnDisable()
        {
            SetStateImmediately(false);
        }

        #endregion

        public void Close(Action onComplete = null)
        {
            _chestData.IsOpened.Value = false;

            Stop();

            _sequence = DOTween.Sequence();

            _sequence
                .Join(_chestData.ChestHinge.transform.DOLocalRotate(_closedRotation, _closeDuration).SetEase(_closeCurve))
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        public void Open(Action onComplete = null)
        {
            _chestData.IsOpened.Value = true;

            Stop();

            _sequence = DOTween.Sequence();

            _sequence
                .Join(_chestData.ChestHinge.transform.DOLocalRotate(_openedRotation, _openDuration).SetEase(_openCurve))
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        public void Stop()
        {
            _sequence?.Kill();
        }

        public void SetStateImmediately(bool opened)
        {
            Stop();

            transform.localRotation = Quaternion.Euler(opened ? _openedRotation : _closedRotation);

            _chestData.IsOpened.Value = opened;
        }

        [Button()]
        private void AssignClosedRotation()
        {
            _closedRotation = transform.localRotation.eulerAngles;
        }

        [Button()]
        private void AssignOpenedRotation()
        {
            _openedRotation = transform.localRotation.eulerAngles;
        }
    }
}
