using System;
using UniRx;

namespace Data.Player.Experience
{
    [Serializable]
    public class ExperienceData
    {
        public IntReactiveProperty Experience = new IntReactiveProperty();
        public IntReactiveProperty TotalExperience = new IntReactiveProperty(110);
        public IntReactiveProperty MaxExperience = new IntReactiveProperty(110);
        public IntReactiveProperty ExperienceLevel = new IntReactiveProperty(1);
        public FloatReactiveProperty FillAmount = new FloatReactiveProperty();
    }
}
