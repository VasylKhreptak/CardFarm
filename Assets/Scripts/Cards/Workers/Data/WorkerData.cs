using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Workers.Data
{
    public class WorkerData : DamageableCardData
    {
        public IntReactiveProperty Satiety = new IntReactiveProperty(0);
        public IntReactiveProperty MinSatiety = new IntReactiveProperty(0);
        public IntReactiveProperty MaxSatiety = new IntReactiveProperty(0);
        public FloatReactiveProperty Efficiency = new FloatReactiveProperty(1);
        public IntReactiveProperty MinEfficiency = new IntReactiveProperty(0);
        public IntReactiveProperty MaxEfficiency = new IntReactiveProperty(0);
        public AnimationCurve SatietyToEfficiencyCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}
