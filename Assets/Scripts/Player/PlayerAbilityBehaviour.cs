using System.Collections;
using UnityEngine;

namespace Player
{
    public abstract class PlayerAbilityBehaviour : MonoBehaviour
    {
        [Header("Configuration"), SerializeField]
        private InputInfo _inputTrigger;

        [SerializeField]
        private bool _overridesNormalMovement;

        public string Name { get; protected set; }
        public InputInfo InputTrigger
        { 
            get => _inputTrigger;
            protected set => _inputTrigger = value;
        }

        private IEnumerator ExecuteAbilityWithLifetime(PlayerContextObject context)
        {
            if (_overridesNormalMovement)
            {
                context.Controller.MovementIsOverridden = true;
            }
            yield return StartCoroutine(ExecuteAbility(context));
            context.ActiveAbilityCoroutines.Remove(Name);

            if (_overridesNormalMovement)
            {
                context.Controller.MovementIsOverridden = false;
            }
        }

        protected virtual bool ValidateExecution(PlayerContextObject context)
        {
            return true;
        }

        protected abstract IEnumerator ExecuteAbility(PlayerContextObject context);

        public bool TryExecute(PlayerContextObject context, out Coroutine outCoroutine)
        {
            outCoroutine = null;

            if (context.ActiveAbilityCoroutines.ContainsKey(Name) || !_inputTrigger.TestInput(context.PlayerInput) || !ValidateExecution(context))
            {
                return false;
            }

            outCoroutine = StartCoroutine(ExecuteAbilityWithLifetime(context));
            return true;
        }
    }
}
