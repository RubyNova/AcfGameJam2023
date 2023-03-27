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
        private bool _shouldCareAboutXAxis;
        
        [SerializeField]
        private bool _shouldCareAboutYAxis;
        
        [SerializeField]
        private bool _shouldCareAboutSprintModifier;

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

        public InputInfo(Vector2 inputAxes, bool inputSprintModifier)
        {
            InputAxes = inputAxes;
            InputSprintModifier = inputSprintModifier;
        }

        public bool TestInput(InputInfo incomingInput)
        {
            bool xInputAxisMatches = true;
            bool yInputAxisMatches = true;
            bool sprintModifierMatches = true;

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

            return xInputAxisMatches && yInputAxisMatches && sprintModifierMatches;
        }
    }
}
