using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Management
{
    public class TouchCounter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIBehaviour _behaviour;

        private IntReactiveProperty _touchCount = new IntReactiveProperty();

        public IReadOnlyReactiveProperty<int> TouchCount => _touchCount;

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            _behaviour ??= GetComponent<UIBehaviour>();
        }

        private void OnEnable()
        {
            _compositeDisposable.Clear();
            _behaviour.OnPointerDownAsObservable().Subscribe(_ => _touchCount.Value++).AddTo(_compositeDisposable);
            _behaviour.OnPointerUpAsObservable().Subscribe(_ => _touchCount.Value--).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable.Clear();
            _touchCount.Value = 0;
        }

        #endregion
    }
}
