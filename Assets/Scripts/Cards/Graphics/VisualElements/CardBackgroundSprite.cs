using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards.Graphics.VisualElements
{
    public class CardBackgroundSpriteUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;
        [SerializeField] private Image _image;

        private IDisposable _backgroundSpriteSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _image = GetComponent<Image>();
            _cardData = GetComponentInParent<CardDataHolder>(true);
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
