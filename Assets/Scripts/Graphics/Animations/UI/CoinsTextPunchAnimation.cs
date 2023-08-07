using Economy;
using UnityEngine;
using Zenject;

namespace Graphics.Animations.UI
{
    public class CoinsTextPunchAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScalePunchAnimation _scalePunchAnimation;

        private CoinsBank _coinsBank;

        [Inject]
        private void Constructor(CoinsBank coinsBank)
        {
            _coinsBank = coinsBank;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _scalePunchAnimation ??= GetComponent<ScalePunchAnimation>();
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
            _coinsBank.onAdded += OnCoinsAdded;
        }

        private void StopObserving()
        {
            _coinsBank.onAdded -= OnCoinsAdded;
        }

        private void OnCoinsAdded(int coins)
        {
            _scalePunchAnimation.Play();
        }
    }
}
