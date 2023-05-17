using System;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class PlayerDeviceInputManager : MonoSingleton<PlayerDeviceInputManager>
    {
        private bool _isFiltered = false;

        protected override void OnInit()
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnMove(_isFiltered ? Vector2.zero : context.ReadValue<Vector2>());
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnSprint(!_isFiltered && context.ReadValueAsButton());
        }
        
        public void OnAbilityTriggerZero(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnAbilityTriggerZero(!_isFiltered && context.ReadValueAsButton());
        }

        public void OnAbilityTriggerOne(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnAbilityTriggerOne(!_isFiltered && context.ReadValueAsButton());
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnAim(context.ReadValue<Vector2>());
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnFire(!_isFiltered && context.ReadValueAsButton());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnJump(!_isFiltered && context.ReadValueAsButton());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
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
            _isFiltered = false;
        }

        public void EnableFiltering()
        {
            _isFiltered = true;
        }
    }
}
