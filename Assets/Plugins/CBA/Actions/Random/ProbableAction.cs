using UnityEngine;
using Action = CBA.Actions.Core.Action;

namespace CBA.Actions.Random
{
    public class ProbableAction : Action
    {
        [Header("References")]
        [SerializeField] private Action _action;

        [Header("Preferences")]
        [SerializeField, Range(0f, 1f)] private float _probability;

        #region MonoBehaviour

        private void OnValidate()
        {
            _action ??= GetComponent<Action>();
        }

        #endregion

        public override void Do()
        {
            Extensions.Mathf.Probability(_probability, _action.Do);
        }
    }
}
