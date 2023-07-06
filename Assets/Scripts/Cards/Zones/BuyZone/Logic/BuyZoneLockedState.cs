using System;
using Cards.Zones.BuyZone.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic
{
    public class BuyZoneLockedState : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;
        [SerializeField] private GameObject _leftCardsTextObject;
        [SerializeField] private TMP_Text _nameText;

        [Header("Preferences")]
        [SerializeField] private string _lockedName = "???";

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
            _nameText.text = isLocked ? _lockedName : _buyZoneData.Name.Value;
            _buyZoneData.IsInteractable.Value = !isLocked;
        }
    }
}
