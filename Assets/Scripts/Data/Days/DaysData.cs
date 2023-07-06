using UniRx;

namespace Data.Days
{
    public class DaysData
    {
        public IntReactiveProperty Days = new IntReactiveProperty(1);

        public DaysDataCallbacks Callbacks = new DaysDataCallbacks();
    }
}
