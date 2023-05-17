using System;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    internal class PlayerDeviceInputManager : MonoSingleton<PlayerDeviceInputManager>
    {
        private bool _filterInputs = false;

        protected override void OnInit()
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();

            if (_filterInputs && input != Vector2.zero)
            {
                return;
            }

            PlayerController.Instance.OnMove(context);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();

            if (_filterInputs && input)
            {
                return;
            }

            PlayerController.Instance.OnSprint(context);
        }
        
        public void OnAbilityTriggerZero(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();

            if (_filterInputs && input)
            {
                return;
            }

            PlayerController.Instance.OnAbilityTriggerZero(context);
        }

        public void OnAbilityTriggerOne(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();

            if (_filterInputs && input)
            {
                return;
            }

            PlayerController.Instance.OnAbilityTriggerOne(context);
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();

            if (_filterInputs && input != Vector2.zero)
            {
                return;
            }

            PlayerController.Instance.OnAim(context);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();

            if (_filterInputs && input)
            {
                return;
            }

            PlayerController.Instance.OnFire(context);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();

            if (_filterInputs && input)
            {
                return;
            }

            PlayerController.Instance.OnJump(context);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            /*
            if (!context.performed)
            {
                return;
            }
            */

            var controller = MenuController.Instance;

            if (controller.NarrativeMenu.IsCurrentlyExecuting)
            {
                controller.NarrativeMenu.OnInteractTrigger(context);
            }
            else
            {
                // TODO: Add interact support for the player if we need it.
            }
        }

        public void OnEsc(InputAction.CallbackContext context)
        {
            if (!context.performed || !context.ReadValueAsButton())
            {
                return;
            }

            var controller = MenuController.Instance;

            if (controller.IsPaused)
            {
                controller.Resume();
            }
            else
            {
                MenuController.Instance.Pause();
            }
        }

        public void DisableFiltering()
        {
            _filterInputs = false;
        }

        public void EnableFiltering()
        {
            _filterInputs = true;
        }
    }
}
