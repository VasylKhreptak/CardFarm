using System;
using Data.Player.Experience;

namespace Data.Player.Core
{
    [Serializable]
    public class PlayerData
    {
        public ExperienceData ExperienceData = new ExperienceData();
    }
}
