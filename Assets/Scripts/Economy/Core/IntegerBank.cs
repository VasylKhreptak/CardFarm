using Runtime.Commands;
using Zenject;

namespace Economy.Core
{
    public class IntegerBank : Bank<int>
    {
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand)
        {
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        public override void Add(int value)
        {
            if (value <= 0) return;

            this.value += value;
            onAdded?.Invoke(value);
            OnValueChanged();
        }

        public override bool TrySpend(int value)
        {
            if (CanAfford(value))
            {
                this.value -= value;
                onSpent?.Invoke(value);
                OnValueChanged();
                return true;
            }

            onFailedSpent?.Invoke(value);
            return false;
        }

        protected virtual void OnValueChanged()
        {
            onChanged?.Invoke(this.value);
        }

        public bool CanAfford(int value)
        {
            return this.value >= value;
        }

        private void OnRestart()
        {
            value = 0;
            OnValueChanged();
        }
    }
}
