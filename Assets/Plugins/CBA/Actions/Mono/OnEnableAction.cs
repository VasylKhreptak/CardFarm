using CBA.Actions.Mono.Core;

namespace CBA.Actions.Mono
{
    public class OnEnableAction : ActionContainer
    {
        #region MonoBehaviour

        private void OnEnable()
        {
            _action.Do();
        }

        #endregion
    }
}
