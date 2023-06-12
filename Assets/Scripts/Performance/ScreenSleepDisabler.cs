using UnityEngine;

namespace Performance
{
    public class ScreenSleepDisabler : MonoBehaviour
    {
        #region MonoBehaviour

        private void Start()
        {
            DisableScreenSleep();
        }

        #endregion

        private void DisableScreenSleep()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
