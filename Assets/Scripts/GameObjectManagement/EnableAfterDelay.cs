using System.Collections;
using UnityEngine;

namespace GameObjectManagement
{
    public class EnableAfterDelay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Header("Preferences")]
        [SerializeField] private float _delay;

        private IEnumerator Start()
        {
            _gameObject.SetActive(false);

            yield return new WaitForSeconds(_delay);

            _gameObject.SetActive(true);
        }
    }
}
