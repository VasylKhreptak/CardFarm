using Data.Days;
using UnityEngine;
using Zenject;

namespace Runtime.Workers
{
    public class WorkerFeeder : MonoBehaviour
    {
        private DaysData _daysData;

        [Inject]
        private void Constructor(DaysData daysData)
        {
            _daysData = daysData;
        }
    }
}
