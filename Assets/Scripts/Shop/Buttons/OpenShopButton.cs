using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shop.Buttons
{
    public class OpenShopButton : MonoBehaviour
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
            _button.onClick.AddListener(_shopPanel.Show);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(_shopPanel.Show);
        }

        #endregion
    }
}
