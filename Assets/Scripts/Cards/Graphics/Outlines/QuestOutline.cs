using System;
using Graphics.UI.Panels;
using UniRx;

namespace Cards.Graphics.Outlines
{
    public class QuestOutline : Panel
    {
        private IDisposable _delaySubscription;

        #region MonoBehaviour

        private void Awake()
        {
            Hide();
        }

        private void OnDisable()
        {
            _delaySubscription?.Dispose();
            Hide();
        }

        #endregion

        public void Show(float time)
        {
            Show();
            _delaySubscription?.Dispose();
            _delaySubscription = Observable.Timer(TimeSpan.FromSeconds(time)).Subscribe(_ => Hide());
        }
    }
}
