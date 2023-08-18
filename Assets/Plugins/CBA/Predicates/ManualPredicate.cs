using CBA.Predicates.Core;
using UnityEngine;

namespace CBA.Predicates
{
    public class ManualPredicate : Predicate
    {
        [Header("Preferences")]
        [SerializeField] private bool _predicateResult = true;

        public override bool Result() => _predicateResult;
    }
}
