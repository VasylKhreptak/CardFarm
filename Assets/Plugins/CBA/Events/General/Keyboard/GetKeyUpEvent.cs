using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.General.Keyboard
{
    public class GetKeyUpEvent : MonoEvent
    {
        [Header("Preferences")]
        [SerializeField] private KeyCode _keyCode;

        #region MonoBehaviour

        private void Update()
        {
            if (UnityEngine.Input.GetKeyUp(_keyCode))
            {
                Invoke();
            }
        }

        #endregion
    }
}
