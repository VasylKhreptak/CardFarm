using Cards.Data;
using Extensions;
using Graphics;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Cards.Graphics
{
    public class CardOverlayDrawer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private SingleLayer _defaultLayer;
        [SerializeField] private SingleLayer _overlayLayer;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnDisable()
        {
            SetOverlayState(false);
        }

        #endregion

        public void SetOverlayState(bool enabled)
        {
            if (enabled)
            {
                _cardData.gameObject.SetLayerRecursive(_overlayLayer.LayerIndex);
                _cardData.IsOverlayed.Value = true;
                _cardData.IsInteractable.Value = false;
            }
            else
            {
                _cardData.gameObject.SetLayerRecursive(_defaultLayer.LayerIndex);
                _cardData.IsOverlayed.Value = false;
                _cardData.IsInteractable.Value = true;
            }
        }


        [Button()]
        public void EnableOverlay() => SetOverlayState(true);

        [Button()]
        public void DisableOverlay() => SetOverlayState(false);
    }
}
