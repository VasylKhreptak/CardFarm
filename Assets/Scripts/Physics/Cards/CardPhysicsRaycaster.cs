using Cards.Data;
using Providers.Graphics;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Physics.Cards
{
    public class CardPhysicsRaycaster : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _cardLayerMask;

        [Header("Preferences")]
        [SerializeField] private int _bufferSize = 10;

        private RaycastHit[] _hits;

        private CardData _hitCard;

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _hits = new RaycastHit[_bufferSize];
        }

        private void OnDestroy()
        {
            OnMouseUp();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp();
            }
        }

        #endregion

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            int hitsCount = UnityEngine.Physics.RaycastNonAlloc(ray, _hits, float.MaxValue, _cardLayerMask);

            CardData topCard = null;
            int topCardSiblingIndex = int.MinValue;
            for (int i = 0; i < hitsCount; i++)
            {
                CardData hitCard = _hits[i].collider.GetComponent<CardData>();
                if (hitCard == null) continue;

                int hitCardSiblingIndex = hitCard.transform.GetSiblingIndex();
                if (hitCardSiblingIndex > topCardSiblingIndex)
                {
                    topCard = hitCard;
                    topCardSiblingIndex = hitCardSiblingIndex;
                }
            }

            _hitCard = topCard;

            if (topCard == null) return;

            topCard.Callbacks.onPointerDown?.Invoke();
        }

        private void OnMouseUp()
        {
            if (_hitCard == null) return;

            _hitCard.Callbacks.onPointerUp?.Invoke();
        }
    }
}
