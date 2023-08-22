using System;
using DG.Tweening;
using Extensions;
using Providers.Graphics;
using Providers.Graphics.UI;
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

        private Canvas _canvas;
        private RectTransform _canvasRectTransform;

        [Inject]
        private void Constructor(CanvasProvider canvasProvider)
        {
            _canvas = canvasProvider.Value;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
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
                .AppendCallback(OnRepeated)
                .AppendCallback(_tutorialHand.Show)
                .AppendCallback(() => _tutorialHand.SetPosition(ConvertPosition(from.position)))
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
                    _tutorialHand.SetPosition(ConvertPosition(from.position));
                })
                .OnUpdate(() =>
                {
                    Vector3 position = Vector3.Lerp(ConvertPosition(from.position), ConvertPosition(to.position), progress);
                    _tutorialHand.SetPosition(position);
                });
        }

        protected Vector3 ConvertPosition(Vector3 position)
        {
            return RectTransformUtilityExtensions.ProjectPointOnCameraCanvas(_canvas, _canvasRectTransform, position);
        }

        protected virtual void OnRepeated()
        {

        }
    }
}
