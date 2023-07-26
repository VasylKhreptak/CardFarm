using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace GameRestartActions
{
    public class TransformReseter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private bool _resetLocalPosition = true;
        [SerializeField] private bool _resetLocalRotation = true;
        [SerializeField] private bool _resetScale = true;

        private Vector3 _initialLocalPosition;
        private Quaternion _initialLocalRotation;
        private Vector3 _initialScale;

        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand)
        {
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        private void Awake()
        {
            _initialLocalPosition = _transform.localPosition;
            _initialLocalRotation = _transform.localRotation;
            _initialScale = _transform.localScale;
        }

        private void Start()
        {
            _gameRestartCommand.OnExecute += ResetTransform;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= ResetTransform;
        }

        #endregion

        private void ResetTransform()
        {
            if (_resetLocalPosition)
            {
                _transform.localPosition = _initialLocalPosition;
            }

            if (_resetLocalRotation)
            {
                _transform.localRotation = _initialLocalRotation;
            }

            if (_resetScale)
            {
                _transform.localScale = _initialScale;
            }
        }
    }
}
