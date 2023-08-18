using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.General.Keyboard
{
    public class OnKeyDownEvent : MonoEvent
    {
        [Header("Preferences")]
        [SerializeField] private KeyCode _keyCode;

        #region MonoBehaviour

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(_keyCode))
            {
                Invoke();
            }
        }

        #endregion
    }
}
