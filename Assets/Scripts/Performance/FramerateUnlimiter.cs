using UnityEngine;

namespace Performance
{
    public class FramerateUnlimiter : MonoBehaviour
    {
        #region MonoBehaviour

        private void Start()
        {
            Set(Screen.currentResolution.refreshRate);
        }

        #endregion

        private void Set(int framerate)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = framerate;
        }
    }
}
