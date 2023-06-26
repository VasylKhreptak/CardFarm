using Extensions.UniRx.UnityEngineBridge.Triggers;
using Providers.Core;
using UnityEngine;

namespace Providers
{
    public class FloorMouseTriggerProvider : Provider<ObservableMouseTrigger>
    {
        [Header("References")]
        [SerializeField] private ObservableMouseTrigger _floorMouseTrigger;

        #region MonoBehaviour

        private void OnValidate()
        {
            _floorMouseTrigger ??= FindObjectOfType<ObservableMouseTrigger>();
        }

        #endregion

        public override ObservableMouseTrigger Value => _floorMouseTrigger;
    }
}
