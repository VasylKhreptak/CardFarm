using System;
using Cards.Chests.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.Graphics.VisualElements
{
    public class ChestSizeText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestData _chestData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _sizeSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _chestData = GetComponentInParent<ChestData>(true);
            _tmp = GetComponent<TMP_Text>();
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
            _sizeSubscription = _chestData.Size.Subscribe(SetText);
        }

        private void StopObserving()
        {
            _sizeSubscription?.Dispose();
        }

        private void SetText(int size)
        {
            _tmp.text = size.ToString();
        }
    }
}
