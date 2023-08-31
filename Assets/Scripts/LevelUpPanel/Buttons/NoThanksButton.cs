using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LevelUpPanel.Buttons
{
    public class NoThanksButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _buttonObject;
        [SerializeField] private Button _button;

        private IDisposable _delaySubscription;

        private LevelUpPanel _levelUpPanel;

        [Inject]
        private void Constructor(LevelUpPanel levelUpPanel)
        {
            _levelUpPanel = levelUpPanel;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
            _delaySubscription?.Dispose();
        }

        #endregion

        public void Show(float delay = 0f)
        {
            _delaySubscription?.Dispose();

            _delaySubscription = Observable.Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(_ => _buttonObject.SetActive(true));
        }

        public void Hide()
        {
            _delaySubscription?.Dispose();
            _buttonObject.SetActive(false);
        }

        private void OnClicked()
        {
            _delaySubscription?.Dispose();
            _levelUpPanel.Hide();
        }
    }
}
