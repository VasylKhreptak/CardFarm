using System.Collections.Generic;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CardsGroupCenterUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void Update()
        {
            List<CardData> groupCards = _cardData.GroupCards;

            Vector3 center = Vector3.zero;

            foreach (var groupCard in groupCards)
            {
                center += groupCard.transform.position;
            }
            center /= groupCards.Count;

            _cardData.GroupCenter.Value = center;
        }

        #endregion

        private void OnDrawGizmosSelected()
        {
            if (_cardData == null) return;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_cardData.GroupCenter.Value, 0.1f);
        }
    }
}
