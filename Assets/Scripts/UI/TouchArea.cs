using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TouchArea : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIBehaviour _behaviour;
        [SerializeField] private TouchCounter _touchCounter;

        #region MonoBehaviour

        private void OnValidate()
        {
            _behaviour ??= GetComponent<UIBehaviour>();
            _touchCounter ??= GetComponent<TouchCounter>();
        }

        #endregion

        public UIBehaviour Behaviour => _behaviour;
        public TouchCounter TouchCounter => _touchCounter;
    }
}
