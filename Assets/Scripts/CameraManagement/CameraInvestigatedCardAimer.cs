using CameraManagement.CameraAim.Core;
using Cards.Data;
using CardsTable.ManualCardSelectors;
using UnityEngine;
using Zenject;

namespace CameraManagement
{
    public class CameraInvestigatedCardAimer : MonoBehaviour
    {
        private InvestigatedCardsObserver _investigatedCardsObserver;
        private CameraAimer _cameraAimer;

        [Inject]
        private void Constructor(InvestigatedCardsObserver investigatedCardsObserver,
            CameraAimer cameraAimer)
        {
            _investigatedCardsObserver = investigatedCardsObserver;
            _cameraAimer = cameraAimer;
        }

        #region MonoBehaviour

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
            _investigatedCardsObserver.OnInvestigatedCard += AimCamera;
        }

        private void StopObserving()
        {
            _investigatedCardsObserver.OnInvestigatedCard -= AimCamera;
        }

        private void AimCamera(CardDataHolder card)
        {
            _cameraAimer.Aim(card.transform);
        }
    }
}
