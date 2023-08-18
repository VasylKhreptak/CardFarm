using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.General.Mouse
{
    public class OnMouseButtonDown : MonoEvent
    {
        [Header("Preferences")]
        [SerializeField] private int _mouseButtonID;

        #region MonoBehaviour

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(_mouseButtonID))
            {
                Invoke();
            }
        }

        #endregion
    }
}
