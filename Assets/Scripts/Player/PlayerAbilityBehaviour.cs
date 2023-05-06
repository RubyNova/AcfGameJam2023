using System.Collections;
using UnityEngine;

namespace Player
{
    public abstract class PlayerAbilityBehaviour : MonoBehaviour
    {
        [Header("Configuration"), SerializeField]
        protected InputInfo _inputTrigger;

        [SerializeField]
        protected bool _overridesNormalMovement;

        public string Name { get; protected set; }
        public InputInfo InputTrigger
        { 
            get => _inputTrigger;
            protected set => _inputTrigger = value;
        }

        private void Reset()
        {
            EnforceInputDefaults();
        }

        protected abstract void EnforceInputDefaults();

        private IEnumerator ExecuteAbilityWithLifetime(PlayerContextObject context)
        {
            if (_overridesNormalMovement)
            {
                context.Controller.MovementIsOverridden = true;
            }
            yield return ExecuteAbility(context);
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
