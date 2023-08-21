using UnityEngine;
using Zenject;

namespace Graphics.VisualElements.Gears.Logic
{
    public class GearsDataReseter : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private GearsData _gearsData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _gearsData = GetComponentInParent<GearsData>(true);
        }

        private void OnDisable()
        {
            ResetData();
        }

        #endregion

        private void ResetData()
        {
            _gearsData.ShowAnimation.MoveToStartState();
            _gearsData.GearsShowAnimation.MoveToEndState();
            _gearsData.MarkHideAnimation.MoveToEndState();
        }
    }
}
