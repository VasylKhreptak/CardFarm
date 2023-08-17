using System;
using CameraManagement.CameraAim.Core;
using Cards.Data;
using CardsTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraManagement.CameraAim
{
    public class CameraLastSelectedCardAimer : MonoBehaviour
    {
        private IDisposable _selectedCardSubscription;

        private CardData _previousSelectedCard;

        private CameraAimer _cameraAimer;
        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        [Inject]
        private void Constructor(CameraAimer cameraAimer,
            CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _cameraAimer = cameraAimer;
            _currentSelectedCardHolder = currentSelectedCardHolder;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingSelectedCard();
        }

        private void OnDisable()
        {
            StopObservingSelectedCard();
        }

        #endregion

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
                _cameraAimer.Aim(_previousSelectedCard.transform, true);
            }

            _previousSelectedCard = selectedCard;
        }
    }
}
