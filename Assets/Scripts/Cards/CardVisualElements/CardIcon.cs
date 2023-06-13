using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.CardVisualElements
{
    public class CardIcon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private Image _image;

        private IDisposable _backgroundSpriteSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _image ??= GetComponent<Image>();
        }

        private void OnEnable()
        {
            StartObservingIcon();
        }

        private void OnDisable()
        {
            StopObservingIcon();
        }

        #endregion

        private void StartObservingIcon()
        {
            StopObservingIcon();
            _backgroundSpriteSubscription = _cardData.Background.Subscribe(SetIcon);
        }

        private void StopObservingIcon()
        {
            _backgroundSpriteSubscription?.Dispose();
        }

        private void SetIcon(Sprite sprite)
        {
            if (sprite == null) return;

            _image.sprite = sprite;
        }
    }
}
