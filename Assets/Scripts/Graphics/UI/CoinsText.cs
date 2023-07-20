using Economy;
using TMPro;
using UnityEngine;
using Zenject;

namespace Graphics.UI
{
    public class CoinsText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        private CoinsBank _coinsBank;

        [Inject]
        private void Constructor(CoinsBank coinsBank)
        {
            _coinsBank = coinsBank;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
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
            SetText(_coinsBank.Value);
            _coinsBank.onChanged += SetText;
        }

        private void StopObserving()
        {
            _coinsBank.onChanged -= SetText;
        }

        private void SetText(int coins)
        {
            _tmp.text = coins.ToString();
        }
    }
}
