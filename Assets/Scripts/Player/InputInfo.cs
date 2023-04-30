using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class InputInfo
    {
        [SerializeField]
        private Vector2 _inputAxes;

        [SerializeField]
        private bool _inputSprintModifier;

        [SerializeField]
        private bool _inputAbilityTriggerZero;

        [SerializeField]
        private bool _jumpInput;

        [SerializeField]
        private bool _shouldCareAboutXAxis;
        
        [SerializeField]
        private bool _shouldCareAboutYAxis;
        
        [SerializeField]
        private bool _shouldCareAboutSprintModifier;

        [SerializeField]
        private bool _shouldCareAboutAbilityTriggerZero;

        [SerializeField]
        private bool _shouldCareAboutJumping;

        public Vector2 InputAxes
        { 
            get => _inputAxes;
            set => _inputAxes = value;
        }

        public bool InputSprintModifier
        {
            get => _inputSprintModifier;
            set => _inputSprintModifier = value;
        }

        public bool InputAbilityTriggerZero
        {
            get => _inputAbilityTriggerZero;
            set => _inputAbilityTriggerZero = value;
        }

        public bool JumpInput
        {
            get => _jumpInput;
            set => _jumpInput = value;
        }

        public InputInfo(Vector2 inputAxes, bool inputSprintModifier, bool inputAbilityTriggerZero)
        {
            InputAxes = inputAxes;
            InputSprintModifier = inputSprintModifier;
            InputAbilityTriggerZero = inputAbilityTriggerZero;
        }

        public bool TestInput(InputInfo incomingInput)
        {
            bool xInputAxisMatches = true;
            bool yInputAxisMatches = true;
            bool sprintModifierMatches = true;
            bool abilityTriggerZeroMatches = true;
            bool jumpInputMatches = true;

            if (_shouldCareAboutXAxis)
            {
                xInputAxisMatches = Mathf.Approximately(InputAxes.x, incomingInput.InputAxes.x);
            }
            
            if (_shouldCareAboutYAxis)
            {
                yInputAxisMatches = Mathf.Approximately(InputAxes.y, incomingInput.InputAxes.y);
            }

            if (_shouldCareAboutSprintModifier)
            {
                sprintModifierMatches = InputSprintModifier == incomingInput.InputSprintModifier;
            }

            if (_shouldCareAboutAbilityTriggerZero)
            {
                abilityTriggerZeroMatches = InputAbilityTriggerZero == incomingInput.InputAbilityTriggerZero;
            }

            if (_shouldCareAboutJumping)
            {
                jumpInputMatches = JumpInput == incomingInput.JumpInput;
            }

            return xInputAxisMatches && yInputAxisMatches && sprintModifierMatches && abilityTriggerZeroMatches && jumpInputMatches;
        }
    }
}
