using System.Collections.Generic;
using System.Linq;
using TreasureChests.Logic.Tags;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace TreasureChests.Logic
{
    public class ChestOpener : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<ChestRaycastTarget> _raycastTargets;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            _raycastTargets ??= GetComponentsInChildren<ChestRaycastTarget>().ToList();
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
            foreach (var raycastTarget in _raycastTargets)
            {
                raycastTarget
                    .UIBehaviour
                    .OnPointerClickAsObservable()
                    .Subscribe(_ =>
                    {
                        OnOpenedChest();
                    });
            }
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnOpenedChest()
        {
            Debug.Log("Opened Chest");
        }
    }
}
