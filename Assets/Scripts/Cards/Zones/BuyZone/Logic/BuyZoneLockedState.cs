using System;
using Cards.Zones.BuyZone.Data;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards.Zones.BuyZone.Logic
{
    public class BuyZoneLockedState : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;
        [SerializeField] private GameObject _leftCardsTextObject;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Image _lockedStateImage;
        [SerializeField] private GameObject _unlockConditionObject;

        private IDisposable _isLockedSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _buyZoneData = GetComponentInParent<BuyZoneData>(true);
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
            _isLockedSubscription = _buyZoneData.IsLocked.Subscribe(OnLockedStateChanged);
        }

        private void StopObserving()
        {
            _isLockedSubscription?.Dispose();
        }

        private void OnLockedStateChanged(bool isLocked)
        {
            _leftCardsTextObject.SetActive(!isLocked);
            _nameText.enabled = !isLocked;
            _buyZoneData.IsInteractable.Value = !isLocked;
            _lockedStateImage.enabled = isLocked;

            if (_unlockConditionObject != null)
            {
                _unlockConditionObject.SetActive(isLocked);
            }
        }
    }
}
