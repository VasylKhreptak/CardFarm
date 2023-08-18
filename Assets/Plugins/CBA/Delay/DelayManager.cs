using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CBA.Delay
{
    public static class DelayManager
    {
        private class DelayMonoBehaviour : MonoBehaviour
        {
        }

        private static DelayMonoBehaviour _monoBehaviour;

        private static void TryInit()
        {
            if (_monoBehaviour != null) return;

            GameObject gameObject = new GameObject("Delay Handler");
            Object.DontDestroyOnLoad(gameObject);
            _monoBehaviour = gameObject.AddComponent<DelayMonoBehaviour>();
        }

        public static Coroutine Wait(float delay, Action action)
        {
            TryInit();

            return _monoBehaviour.StartCoroutine(WaitRoutine(delay, action));
        }

        public static Coroutine WaitFrame(Action action)
        {
            TryInit();

            return _monoBehaviour.StartCoroutine(WaitUntilNextFrameRoutine(action));
        }

        public static void StopWaiting(ref Coroutine coroutine)
        {
            if (coroutine == null) return;

            if (SceneManager.GetActiveScene().isLoaded == false) return;

            _monoBehaviour.StopCoroutine(coroutine);
            coroutine = null;
        }

        private static IEnumerator WaitRoutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);

            action?.Invoke();
        }

        private static IEnumerator WaitUntilNextFrameRoutine(Action action)
        {
            yield return null;

            action?.Invoke();
        }
    }
}
