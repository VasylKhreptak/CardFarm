using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GameObjectManagement
{
    public class GameObjectStateInheritor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<ObservableTarget> _observableObjects = new List<ObservableTarget>();
        [SerializeField] private GameObject _inheritTo;

        [Header("Preferences")]
        [SerializeField] private bool _allMatchingState = true;
        [SerializeField] private bool _inverse;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        private void StartObserving()
        {
            StopObserving();

            foreach (var observableTarget in _observableObjects)
            {
                observableTarget.GameObject.OnEnableAsObservable().Subscribe(_ => OnStateUpdated()).AddTo(_subscriptions);
                observableTarget.GameObject.OnDisableAsObservable().Subscribe(_ => OnStateUpdated()).AddTo(_subscriptions);
            }
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        #endregion

        private void OnStateUpdated()
        {
            bool isMatchingState;

            if (_allMatchingState)
            {
                isMatchingState = _observableObjects.All(x => x.GameObject.activeSelf == x.TargetState);
            }
            else
            {
                isMatchingState = _observableObjects.Any(x => x.GameObject.activeSelf == x.TargetState);
            }

            _inheritTo.SetActive(_inverse ? !isMatchingState : isMatchingState);
        }

        [Serializable]
        private class ObservableTarget
        {
            public GameObject GameObject;
            public bool TargetState;
        }
    }
}
