using Cards.Entities.Animals.Cattle.Data;
using Extensions;
using UnityEngine;

namespace Cards.Entities.Animals.Cattle.Logic
{
    public class CattleMoveLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CattleCardData _cardData;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CattleCardData>();
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();
            _cardData.CattleCallbacks.OnItemSpawnedNoArgs += OnCattleSpawnedCard;
        }

        private void StopObserving()
        {
            _cardData.CattleCallbacks.OnItemSpawnedNoArgs -= OnCattleSpawnedCard;
        }

        private void OnCattleSpawnedCard()
        {
            JumpInRandomDirection();
        }

        private void JumpInRandomDirection()
        {
            Vector3 randomPosition = GetRandomPosition();

            _cardData.Animations.JumpAnimation.Play(randomPosition);

            if (_cardData.CanSortingLayerChange)
            {
                _cardData.RenderOnTop();
            }
        }

        private Vector3 GetRandomPosition()
        {
            float range = GetRange();

            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * range;

            Vector3 randomSphere = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
            return _cardData.transform.position + randomSphere;
        }

        private float GetRange()
        {
            return Random.Range(_minRange, _maxRange);
        }
    }
}
