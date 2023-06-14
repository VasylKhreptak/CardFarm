using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Cards.Data
{
    public class CardData : MonoBehaviour
    {
        public int ID = -1;
        public ObservableMouseTrigger MouseTrigger;
        public ReactiveProperty<CardType> CardType = new ReactiveProperty<CardType>();
        public StringReactiveProperty Name = new StringReactiveProperty("Name");
        public ReactiveProperty<Sprite> Background = new ReactiveProperty<Sprite>();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();
        public ReactiveProperty<CardData> TopCard = new ReactiveProperty<CardData>();
        public ReactiveProperty<CardData> BottomCard = new ReactiveProperty<CardData>();
        public Transform BottomCardFollowPoint;
    }
}
