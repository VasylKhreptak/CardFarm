using System;
using DG.Tweening;
using Providers.Graphics;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Core
{
    public class QuestHandMoveTutorial : QuestTextTutorial
    {
        [Header("Animation Preferences")]
        [SerializeField] private float _handMoveDuration = 1f;
        [SerializeField] private float _handMoveDelay = 0.5f;
        [SerializeField] private float _tutorialHandDelay = 1f;

        private Sequence _handSequence;

        private IDisposable _tutorialHandDelaySubscription;

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        public override void StopTutorial()
        {
            base.StopTutorial();

            StopHandTutorial();
        }

        protected void StartHandTutorialDelayed(Transform from, Transform to)
        {
            _tutorialHandDelaySubscription?.Dispose();
            _tutorialHandDelaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(_tutorialHandDelay))
                .Subscribe(_ => StartHandTutorial(from, to));
        }

        protected void StartHandTutorial(Transform from, Transform to)
        {
            StopHandTutorial();

            _handSequence = DOTween.Sequence();

            _handSequence
                .AppendCallback(_tutorialHand.Show)
                .AppendCallback(() => _tutorialHand.SetPosition(GetScreenPosition(from.position)))
                .AppendCallback(_tutorialHand.Press)
                .Join(CreateHandFollowTween(from, _handMoveDelay))
                .Append(CreateHandMoveTween(from, to, _handMoveDuration))
                .AppendCallback(_tutorialHand.Release)
                .Append(CreateHandFollowTween(to, _handMoveDelay))
                .SetLoops(-1, LoopType.Restart)
                .Play();
        }

        protected void StopHandTutorial()
        {
            KillHandSequence();
            _tutorialHand.Hide();
            _tutorialHandDelaySubscription?.Dispose();
        }

        protected void KillHandSequence()
        {
            _handSequence?.Kill();
        }

        protected Tween CreateHandFollowTween(Transform target, float duration)
        {
            return CreateHandMoveTween(target, target, duration);
        }

        protected Tween CreateHandMoveTween(Transform from, Transform to, float duration)
        {
            float progress = 0;
            return DOTween
                .To(() => progress, x => progress = x, 1, duration)
                .SetEase(Ease.Linear)
                .OnPlay(() =>
                {
                    _tutorialHand.SetPosition(GetScreenPosition(from.transform.position));
                })
                .OnUpdate(() =>
                {
                    Vector3 currentScreenPosition = GetScreenPosition(from.transform.position);
                    Vector3 targetScreenPosition = GetScreenPosition(to.transform.position);
                    Vector3 screenPosition = Vector3.Lerp(currentScreenPosition, targetScreenPosition, progress);
                    _tutorialHand.SetPosition(screenPosition);
                });
        }

        protected Vector3 GetScreenPosition(Vector3 worldPosition)
        {
            return _camera.WorldToScreenPoint(worldPosition);
        }
    }
}
