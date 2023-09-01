using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    public class DoTweenKiller : MonoBehaviour
    {
        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}
