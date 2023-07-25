using Data.Days;
using DG.Tweening;
using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace Graphics.UI
{
    public class SalaryTimePanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _showTime = 2f;
        [SerializeField] private float _fadeDuration = 0.5f;

        private Sequence _showSequence;

        private DaysData _daysData;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(DaysData daysData, GameRestartCommand gameRestartCommand)
        {
            _daysData = daysData;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnEnable()
        {
            StartObserving();
            Disable();
        }

        private void OnDisable()
        {
            StopObserving();
            Disable();
            KillAnimation();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void StartObserving()
        {
            _daysData.Callbacks.onNewDayCome += Show;

        }

        private void StopObserving()
        {
            _daysData.Callbacks.onNewDayCome -= Show;
        }

        private void Show()
        {
            Enable();
            KillAnimation();

            Tween showAnimation = _canvasGroup.DOFade(1, _fadeDuration);
            Tween hideAnimation = _canvasGroup.DOFade(0, _fadeDuration);

            _showSequence = DOTween.Sequence();
            _showSequence
                .Append(showAnimation)
                .AppendInterval(_showTime)
                .Append(hideAnimation)
                .OnComplete(Disable)
                .Play();
        }

        private void Enable()
        {
            _gameObject.SetActive(true);
        }

        private void Disable()
        {
            _gameObject.SetActive(false);
        }

        private void KillAnimation()
        {
            _showSequence?.Kill();
        }

        private void OnRestart()
        {
            KillAnimation();
            Disable();
        }
    }
}
