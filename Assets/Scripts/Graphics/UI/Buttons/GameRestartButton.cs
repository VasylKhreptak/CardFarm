using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Graphics.UI.Buttons
{
    public class GameRestartButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        #region MonoBehaviour

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(RestartScene);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(RestartScene);
        }

        #endregion

        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
