using CBA.Events.Core;
using UnityEngine;

namespace CBA.Events.General.Mouse
{
    public class OnMouseButtonUp : MonoEvent
    {
        [Header("Preferences")]
        [SerializeField] private int _mouseButtonID;

        #region MonoBehaviour

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonUp(_mouseButtonID))
            {
                Invoke();
            }
        }

        #endregion
    }
}
