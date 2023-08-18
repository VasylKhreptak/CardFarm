using CBA.Actions.Mono.Core;

namespace CBA.Actions.Mono
{
    public class OnStartAction : ActionContainer
    {
        #region MonoBehaviour

        private void Start()
        {
            _action.Do();
        }

        #endregion
    }
}
