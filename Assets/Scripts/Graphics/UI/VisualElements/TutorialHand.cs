using DG.Tweening;
using Graphics.Animations.UI;
using Graphics.UI.Panels.Core;
using Graphics.UI.Particles.Core;
using NaughtyAttributes;
using Providers.Graphics;
using UnityEngine;
using Zenject;

namespace Graphics.UI.VisualElements
{
    public class TutorialHand : MovablePanel
    {
        [Header("Preferences")]
        [SerializeField] private float _clickDuration;
        [SerializeField] private float _pressDuration;
        [SerializeField] private AnimationCurve _clickCurve;
        [SerializeField] private Vector3 _pressedScale;
        [SerializeField] private LayerMask _floorLayerMask;

        private Vector3 _initialScale;

        private Sequence _clickSequence;

        private Camera _camera;
        private ParticlePooler _particlePooler;

        [Inject]
        private void Constructor(CameraProvider cameraProvider, ParticlePooler particlePooler)
        {
            _camera = cameraProvider.Value;
            _particlePooler = particlePooler;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _initialScale = _panel.transform.localScale;
        }

        #endregion

        public override void Hide()
        {
            base.Hide();

            KillSequence();
            ResetScale();
        }

        private void KillSequence()
        {
            _clickSequence?.Kill();
        }

        private void ResetScale() => _panel.transform.localScale = _initialScale;

        [Button()]
        public void Click()
        {
            if (_panel.gameObject.activeSelf == false) return;

            KillSequence();

            Tween pressTween = CreatePressTween();

            Tween releaseTween = CreateReleaseTween();

            _clickSequence = DOTween.Sequence()
                .Append(pressTween)
                .AppendCallback(SpawnClickParticle)
                .AppendInterval(_pressDuration)
                .Append(releaseTween)
                .Play();
        }

        [Button()]
        public void Press()
        {
            if (_panel.gameObject.activeSelf == false) return;

            KillSequence();

            Tween pressTween = CreatePressTween();

            _clickSequence = DOTween.Sequence()
                .Append(pressTween)
                .OnComplete(SpawnClickParticle)
                .Play();
        }

        [Button()]
        public void Release()
        {
            if (_panel.gameObject.activeSelf == false) return;

            KillSequence();

            Tween releaseTween = CreateReleaseTween();

            _clickSequence = DOTween.Sequence()
                .Append(releaseTween)
                .Play();
        }

        private Tween CreatePressTween() => _panel.transform.DOScale(_pressedScale, _clickDuration).SetEase(_clickCurve);

        private Tween CreateReleaseTween() => _panel.transform.DOScale(_initialScale, _clickDuration).SetEase(_clickCurve);

        private void SpawnClickParticle()
        {
            Vector3 spawnPosition = RaycastFloor();
            GameObject particleObject = _particlePooler.Spawn(Particle.Click);
            ClickAnimation clickAnimation = particleObject.GetComponent<ClickAnimation>();
            clickAnimation.Play(_panel.gameObject.transform.position,spawnPosition);
        }

        private Vector3 RaycastFloor()
        {
            Vector3 position = Vector3.zero;

            Ray ray = _camera.ScreenPointToRay(_panel.gameObject.transform.position);
            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _floorLayerMask))
            {
                position = hit.point;
            }

            return position;
        }
    }
}
