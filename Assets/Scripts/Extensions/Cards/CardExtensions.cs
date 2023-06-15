using Cards.Data;

namespace Extensions.Cards
{
    public static class CardExtensions
    {
        public static void Link(this CardData card, CardData linkTo)
        {
            if (linkTo == null) return;

            CardData upperCard = card.UpperCard.Value;

            if (upperCard != null)
            {
                upperCard.BottomCard.Value = card;
            }

            card.UpperCard.Value = linkTo;
        }

        public static void Unlink(this CardData card)
        {
            CardData upperCard = card.UpperCard.Value;

            if (upperCard != null)
            {
                upperCard.BottomCard.Value = null;
            }

            card.UpperCard.Value = null;
        }
    }
}
