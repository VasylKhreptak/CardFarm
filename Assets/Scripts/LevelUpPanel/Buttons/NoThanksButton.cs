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
        }

        #endregion

        public void Show() => _buttonObject.SetActive(true);

        public void Hide() => _buttonObject.SetActive(false);

        private void OnClicked()
        {
            _levelUpPanel.Hide();
        }
    }
}
