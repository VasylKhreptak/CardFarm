using Cards.Data;
using UniRx;

namespace Cards.Boosters.Data
{
    public class BoosterCardData : CardData
    {
        public IntReactiveProperty TotalCards = new IntReactiveProperty(5);
        public IntReactiveProperty LeftCards = new IntReactiveProperty();

        public BoosterCardDataCallbacks BoosterCallabcks = new BoosterCardDataCallbacks();
    }
}
