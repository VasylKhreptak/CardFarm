using UnityEngine;
using UnlockedCardPanel.VisualizableCard.Data;
using Zenject;

namespace UnlockedCardPanel.VisualizableCard.Logic
{
    public class OnShownCardDrawer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private VisualizableCardData _cardData;
        [SerializeField] private GameObject _objectToDraw;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData ??= GetComponentInParent<VisualizableCardData>(true);
        }

        private void OnEnable()
        {
            Disable();
            _cardData.ShowAnimation.OnComplete += Enable;
        }

        private void OnDisable()
        {
            _cardData.ShowAnimation.OnComplete -= Enable;
        }

        #endregion

        private void Enable() => _objectToDraw.SetActive(true);

        private void Disable() => _objectToDraw.SetActive(false);
    }
}
