using UnityEngine;

namespace Shop
{
    public class ShopPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _shopObject;

        public void Show()
        {
            _shopObject.SetActive(true);
        }

        public void Hide()
        {
            _shopObject.SetActive(false);
        }
    }
}
