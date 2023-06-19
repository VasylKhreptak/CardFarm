using System;

namespace Cards.Data
{
    public class CardDataCallbacks
    {
        public Action onAnyBottomCardUpdated;
        public Action onBottomCardsListUpdated;

        public Action onAnyUpperCardUpdated;
        public Action onUpperCardsListUpdated;

        public Action onGroupCardsListUpdated;
        public Action onBecameHeadOfGroup;

        public Action onClicked;
    }
}
