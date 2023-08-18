using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.General.Mouse
{
    public class OnMouseButtonPressed : MonoEvent
    {
        [Header("Preferences")]
        [SerializeField] private int _mouseButtonID;

        #region MonoBehaviour

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButton(_mouseButtonID))
            {
                Invoke();
            }
        }

        #endregion
    }
}
