using System;
using Cards.Data;
using EditorTools.Validators.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using IValidatable = EditorTools.Validators.Core.IValidatable;

namespace Cards.Graphics.VisualElements
{
    public class CardIcon : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private Image _image;

        private IDisposable _backgroundSpriteSubscription;

        #region MonoBehaviour

        public void OnValidate()
        {
            _image = GetComponent<Image>();
            _cardData = GetComponentInParent<CardData>(true);
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
