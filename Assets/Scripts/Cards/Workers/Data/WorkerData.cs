using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Workers.Data
{
    public class WorkerData : DamageableCardData
    {
        [Header("Satiety")]
        public IntReactiveProperty Satiety = new IntReactiveProperty(0);
        public IntReactiveProperty NeededSatiety = new IntReactiveProperty(0);
        public IntReactiveProperty MinSatiety = new IntReactiveProperty(0);
        public IntReactiveProperty MaxSatiety = new IntReactiveProperty(0);

        [Header("Efficiency")]
        public FloatReactiveProperty Efficiency = new FloatReactiveProperty(1);
        public FloatReactiveProperty MinEfficiency = new FloatReactiveProperty(0);
        public FloatReactiveProperty MaxEfficiency = new FloatReactiveProperty(0);
        public AnimationCurve SatietyToEfficiencyCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Energy")]
        public IntReactiveProperty Energy = new IntReactiveProperty(0);
        public IntReactiveProperty MinEnergy = new IntReactiveProperty(0);
        public IntReactiveProperty MaxEnergy = new IntReactiveProperty(0);
        public IntReactiveProperty NeededEnergy = new IntReactiveProperty(0);
        public FloatReactiveProperty EnergyRestoreDuration = new FloatReactiveProperty(1);
        public BoolReactiveProperty IsEnergyFull = new BoolReactiveProperty(false);

        public Transform GearsPoint;
    }
}
