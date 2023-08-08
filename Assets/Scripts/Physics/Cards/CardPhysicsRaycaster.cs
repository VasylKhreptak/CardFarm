using Cards.Data;
using Extensions;
using Providers.Graphics;
using UnityEngine;
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

        private CardDataHolder _hitCard;

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
            int touchCount = Input.touchCount;

            if (touchCount > 1)
            {
                if (Input.GetTouch(touchCount - 1).phase == TouchPhase.Began)
                {
                    OnMouseUp();
                }
            }

            if (Input.touchCount != 1) return;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                OnMouseDown();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnMouseUp();
            }
        }

        #endregion

        private void OnMouseDown()
        {
            if (PointerTools.IsPointerOverUI()) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            int hitsCount = UnityEngine.Physics.RaycastNonAlloc(ray, _hits, float.MaxValue, _cardLayerMask);

            CardDataHolder topCard = null;
            int topCardSiblingIndex = int.MinValue;
            for (int i = 0; i < hitsCount; i++)
            {
                CardDataHolder hitCard = _hits[i].collider.GetComponent<CardDataHolder>();
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
