using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;
using Zones.Data;

namespace Zones.Graphics.UI
{
    public class ZoneNameText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ZoneData _zoneData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _nameSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _tmp = GetComponent<TMP_Text>();
            _zoneData = GetComponentInParent<ZoneData>(true);
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
            _nameSubscription = _zoneData.Name.Subscribe(SetText);
        }

        private void StopObserving()
        {
            _nameSubscription?.Dispose();
        }

        private void SetText(string text)
        {
            _tmp.text = text;
        }
    }
}
