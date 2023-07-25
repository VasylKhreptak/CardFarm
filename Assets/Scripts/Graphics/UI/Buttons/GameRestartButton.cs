using Runtime.Commands;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.UI.Buttons
{
    public class GameRestartButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand)
        {
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(_gameRestartCommand.Execute);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(_gameRestartCommand.Execute);
        }

        #endregion
    }
}
