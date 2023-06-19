using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Graphics.VisualElements
{
    public class CardBackgroundSpriteUpdater : MonoBehaviour
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
            StartObservingBackgroundSprite();
        }

        private void OnDisable()
        {
            StopObservingBackgroundSprite();
        }

        #endregion

        private void StartObservingBackgroundSprite()
        {
            StopObservingBackgroundSprite();
            _backgroundSpriteSubscription = _cardData.Background.Subscribe(SetSprite);
        }

        private void StopObservingBackgroundSprite()
        {
            _backgroundSpriteSubscription?.Dispose();
        }

        private void SetSprite(Sprite sprite)
        {
            if (sprite == null) return;

            _image.sprite = sprite;
        }
    }
}
