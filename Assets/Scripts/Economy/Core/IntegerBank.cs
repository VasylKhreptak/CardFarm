namespace Economy.Core
{
    public class IntegerBank : Bank<int>
    {
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
    }
}
