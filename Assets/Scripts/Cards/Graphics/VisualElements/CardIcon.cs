using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards.Graphics.VisualElements
{
    public class CardIcon : MonoBehaviour, IValidatable
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
            _backgroundSpriteSubscription = _cardData.Icon.Subscribe(SetIcon);
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
