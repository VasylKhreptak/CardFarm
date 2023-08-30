using DG.Tweening;
using UnityEngine;

namespace Shop
{
    public class ShopPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _shopObject;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Animation Preferences")]
        [SerializeField] private float _fadeDuration = 0.4f;

        private Tween _animation;

        #region MonoBehaviour

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
        }

        private void OnDisable()
        {
            KillAnimation();
        }

        #endregion

        public void Show()
        {
            _shopObject.SetActive(true);
            KillAnimation();
            _animation = _canvasGroup.DOFade(1f, _fadeDuration).Play();
        }

        public void Hide()
        {
            KillAnimation();
            _animation = _canvasGroup.DOFade(0f, _fadeDuration).OnComplete(() => _shopObject.SetActive(false)).Play();
        }

        private void KillAnimation() => _animation?.Kill();
    }
}
