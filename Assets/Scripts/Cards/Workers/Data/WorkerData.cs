using Cards.Data;
using UniRx;

namespace Cards.Workers.Data
{
    public class WorkerData : DamageableCardData
    {
        public IntReactiveProperty Satiety = new IntReactiveProperty(0);
        public IntReactiveProperty MaxSatiety = new IntReactiveProperty(0);
        public IntReactiveProperty Efficiency = new IntReactiveProperty(0);
        public IntReactiveProperty MinEfficiency = new IntReactiveProperty(0);
        public IntReactiveProperty MaxEfficiency = new IntReactiveProperty(0);
    }
}
