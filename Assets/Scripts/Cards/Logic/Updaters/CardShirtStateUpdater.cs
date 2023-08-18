using Cards.Data;
using Providers.Graphics;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CardShirtStateUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private Transform _transform;

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
            _transform = GetComponent<Transform>();
        }

        // private void Update()
        // {
        //     UpdateShirtState();
        // }

        #endregion

        public void UpdateShirtState()
        {
            if (_camera == null) return;

            Vector3 cameraDirection = _camera.transform.forward;
            Vector3 cardUpDirection = _transform.up;

            float dotProduct = Vector3.Dot(cameraDirection, cardUpDirection);

            bool enabled = dotProduct > 0f;

            _cardData.CardShirt.SetState(enabled);
        }
    }
}
