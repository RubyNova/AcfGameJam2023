using System;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class PlayerDeviceInputManager : MonoSingleton<PlayerDeviceInputManager>
    {
        [Header("Dependencies"), SerializeField]
        private PlayerInput _inputComponent;

        protected override void OnInit()
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnMove(context);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnSprint(context);
        }
        
        public void OnAbilityTriggerZero(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnAbilityTriggerZero(context);
        }

        public void OnAbilityTriggerOne(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnAbilityTriggerOne(context);
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnAim(context);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            PlayerController.Instance.OnFire(context);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
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
            _inputComponent.ActivateInput();
        }

        public void EnableFiltering()
        {
            _inputComponent.DeactivateInput();
        }
    }
}
