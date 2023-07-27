using System;
using CameraManagement.CameraAim.Core;
using CameraManagement.CameraMove.Core;
using CameraManagement.CameraZoom.Core;
using Cards.Data;
using Runtime.Commands;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraManagement.CameraAim
{
    public class CameraLastSelectedCardAimer : MonoBehaviour
    {
        private CompositeDisposable _interruptSubscriptions = new CompositeDisposable();

        private IDisposable _selectedCardSubscription;

        private CardData _previousSelectedCard;

        private CameraAimer _cameraAimer;
        private MapDragObserver _mapDragObserver;
        private ZoomObserver _zoomObserver;
        private CurrentSelectedCardHolder _currentSelectedCardHolder;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(CameraAimer cameraAimer,
            MapDragObserver mapDragObserver,
            ZoomObserver zoomObserver,
            CurrentSelectedCardHolder currentSelectedCardHolder,
            GameRestartCommand gameRestartCommand)
        {
            _cameraAimer = cameraAimer;
            _mapDragObserver = mapDragObserver;
            _zoomObserver = zoomObserver;
            _currentSelectedCardHolder = currentSelectedCardHolder;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnEnable()
        {
            StartObservingInterrupt();
            StartObservingSelectedCard();
        }

        private void OnDisable()
        {
            StopObservingInterrupt();
            StopObservingSelectedCard();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void StartObservingInterrupt()
        {
            StopObservingInterrupt();

            _mapDragObserver.IsDragging.Where(x => x).Subscribe(_ => OnInterrupted()).AddTo(_interruptSubscriptions);
            _zoomObserver.IsZooming.Where(x => x).Subscribe(_ => OnInterrupted()).AddTo(_interruptSubscriptions);
        }

        private void StopObservingInterrupt()
        {
            _interruptSubscriptions.Clear();
        }

        private void OnInterrupted()
        {
            _cameraAimer.StopAiming();
        }

        private void StartObservingSelectedCard()
        {
            StopObservingSelectedCard();

            _selectedCardSubscription = _currentSelectedCardHolder.SelectedCard.Subscribe(OnSelectedCardChanged);
        }

        private void StopObservingSelectedCard()
        {
            _selectedCardSubscription?.Dispose();
        }

        private void OnSelectedCardChanged(CardData selectedCard)
        {
            _cameraAimer.StopAiming();

            if (selectedCard == null && _previousSelectedCard != null)
            {
                _cameraAimer.Aim(_previousSelectedCard.transform);
            }

            _previousSelectedCard = selectedCard;
        }

        private void OnRestart()
        {
            _cameraAimer.StopAiming();
        }
    }
}
