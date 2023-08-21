using Cards.Core;
using Cards.Data;
using Graphics.Animations.Shake.Position;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardCameraShaker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CameraShakeAnimation _cameraShakeAnimation;

        [Inject]
        private void Constructor(CameraShakeAnimation cameraShakeAnimation)
        {
            _cameraShakeAnimation = cameraShakeAnimation;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            _cardData.Callbacks.onSpawnedRecipeResult += OnSpawnedRecipeResult;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onSpawnedRecipeResult -= OnSpawnedRecipeResult;
        }

        #endregion

        private void OnSpawnedRecipeResult(Card card) => ShakeCamera();

        private void ShakeCamera() => _cameraShakeAnimation.Play();
    }
}
