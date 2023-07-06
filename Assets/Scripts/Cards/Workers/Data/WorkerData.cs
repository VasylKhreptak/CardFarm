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
        public FloatReactiveProperty MinEfficiency = new FloatReactiveProperty(0);
        public FloatReactiveProperty MaxEfficiency = new FloatReactiveProperty(0);
        public AnimationCurve SatietyToEfficiencyCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}
