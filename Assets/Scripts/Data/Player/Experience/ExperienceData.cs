using System;
using UniRx;

namespace Data.Player.Experience
{
    [Serializable]
    public class ExperienceData
    {
        public IntReactiveProperty Experience = new IntReactiveProperty(0);
        public IntReactiveProperty MinLevelExperience = new IntReactiveProperty(0);
        public IntReactiveProperty MaxLevelExperience = new IntReactiveProperty(0);
        public IntReactiveProperty ExperienceLevel = new IntReactiveProperty(0);
    }
}
