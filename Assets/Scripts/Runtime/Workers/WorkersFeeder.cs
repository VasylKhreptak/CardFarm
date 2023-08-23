using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.Workers.Data;
using CardsTable.ManualCardSelectors;
using Data.Days;
using Economy;
using Extensions;
using Graphics.UI.Particles.Coins.Logic;
using NaughtyAttributes;
using Providers.Graphics;
using Providers.Graphics.UI;
using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace Runtime.Workers
{
    public class WorkersFeeder : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _feedDelay = 1f;
        [SerializeField] private float _feedInterval = 0.2f;

        private Coroutine _feedWorkersCoroutine;

        private DaysData _daysData;
        private WorkersSelector _workersSelector;
        private CoinsBank _coinsBank;
        private CoinsSpender _coinsSpender;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(DaysData daysData,
            WorkersSelector workersSelector,
            CoinsBank coinsBank,
            CoinsSpender coinsSpender,
            CameraProvider cameraProvider,
            GameRestartCommand gameRestartCommand,
            CanvasProvider canvasProvider)
        {
            _daysData = daysData;
            _workersSelector = workersSelector;
            _coinsBank = coinsBank;
            _coinsSpender = coinsSpender;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            StopFeedingWorkers();
        }

        #endregion

        private void StartObserving()
        {
            _daysData.Callbacks.onNewDayCome += StartFeedingWorkers;
            _gameRestartCommand.OnExecute += StopFeedingWorkers;
        }

        private void StopObserving()
        {
            _daysData.Callbacks.onNewDayCome -= StartFeedingWorkers;
            _gameRestartCommand.OnExecute -= StopFeedingWorkers;
        }

        [Button]
        private void StartFeedingWorkers()
        {
            StopFeedingWorkers();
            _feedWorkersCoroutine = StartCoroutine(FeedWorkersRoutine());
        }

        private void StopFeedingWorkers()
        {
            if (_feedWorkersCoroutine != null)
            {
                StopCoroutine(_feedWorkersCoroutine);
            }
        }

        private IEnumerator FeedWorkersRoutine()
        {
            yield return new WaitForSeconds(_feedDelay);

            List<WorkerData> workerCards = _workersSelector.SelectedCards.Select(x => x as WorkerData).ToList();

            EmptyWorkersSatiety(workerCards);

            foreach (var worker in workerCards)
            {
                if (_coinsBank.Value == 0) yield break;

                FeedWorker(worker);
                yield return new WaitForSeconds(_feedInterval);
            }
        }

        private void EmptyWorkersSatiety(List<WorkerData> workers)
        {
            foreach (var worker in workers)
            {
                worker.Satiety.Value = worker.MinSatiety.Value;
            }
        }

        private void FillWorkerSatiety(WorkerData worker)
        {
            worker.Satiety.Value = worker.MaxSatiety.Value;
        }

        private void FeedWorker(WorkerData worker)
        {
            int neededCoins = worker.NeededSatiety.Value;

            _coinsSpender.Spend(neededCoins,
                () => worker.transform.position,
                onSpentAllCoins: () =>
                {
                    FillWorkerSatiety(worker);
                });
        }
    }
}
