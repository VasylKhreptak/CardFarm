using Graphics.UI.Panels;

namespace Cards.Graphics.Outlines
{
    public class QuestOutline : Panel
    {
        #region MonoBehaviour

        private void Awake()
        {
            Hide();
        }

        private void OnDisable()
        {
            Hide();
        }

        #endregion
    }
}
