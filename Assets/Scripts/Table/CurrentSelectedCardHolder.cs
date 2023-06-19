using Cards.Data;
using UniRx;
using UnityEngine;

namespace Table
{
    public class CurrentSelectedCardHolder : MonoBehaviour
    {
        public ReactiveProperty<CardData> SelectedCard = new ReactiveProperty<CardData>();
    }
}
