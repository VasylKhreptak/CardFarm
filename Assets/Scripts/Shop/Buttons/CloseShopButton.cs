using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shop.Buttons
{
    public class CloseShopButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        private ShopPanel _shopPanel;

        [Inject]
        private void Constructor(ShopPanel shopPanel)
        {
            _shopPanel = shopPanel;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(_shopPanel.Hide);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(_shopPanel.Hide);
        }

        #endregion
    }
}
