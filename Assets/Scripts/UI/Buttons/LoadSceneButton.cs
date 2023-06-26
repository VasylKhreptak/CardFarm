using System;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class LoadSceneButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        [Header("Preferences")]
        [SerializeField, Scene] private string _scene;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
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
            _subscription = _button
                .OnClickAsObservable()
                .Subscribe(_ => LoadScene());
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(_scene);
        }
    }
}
