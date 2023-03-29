using Movement;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private AnimationDataPipe _pipe;

        [Header("Configuration"), SerializeField]
        private float _walkingSpeed;

        [SerializeField]
        private float _runningSpeed;

        [SerializeField]
        private float _crawlingSpeed;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private bool _hasAnimations;

        private InputInfo _inputInfo;
        private List<PlayerAbilityBehaviour> _abilities;
        private Dictionary<string, Coroutine> _activeAbilityCoroutines;

        public bool MovementIsOverridden { get; set; }

        private void Awake()
        {
            _inputInfo = new(Vector2.zero, false, false);
            _abilities = new();
            _activeAbilityCoroutines = new();
            MovementIsOverridden = false;
        }

        private void Start()
        {
            _abilities.AddRange(gameObject.GetComponents<PlayerAbilityBehaviour>());
        }

        // Update is called once per frame
        private void Update()
        {
            PlayerContextObject context = new(this, _mover, _inputInfo, false, _activeAbilityCoroutines);

            if (_hasAnimations)
            {
                _pipe.InputBasedUpdates(_inputInfo);
            }

            foreach (var ability in _abilities)
            {
                if (_activeAbilityCoroutines.ContainsKey(ability.Name))
                {
                    continue;
                }

                if (ability.TryExecute(context, out var abilityRoutine))
                {
                    _activeAbilityCoroutines.Add(ability.Name, abilityRoutine);
                }
            }

            if (!MovementIsOverridden)
            {
                float finalMovementSpeed = _walkingSpeed;
                bool modifyInputSpeed = false;
                var finalInputData = _inputInfo.InputAxes;

                if (_inputInfo.InputSprintModifier && _inputInfo.InputAxes.y >= 0)
                {
                    finalMovementSpeed = _runningSpeed;
                }
                else if (_inputInfo.InputAxes.y < -0.1f)
                {
                    finalMovementSpeed = _crawlingSpeed;
                    modifyInputSpeed = true;
                }

                if (modifyInputSpeed)
                {
                    finalInputData.y = 0;
                }

                _mover.ApplyMove(finalInputData, finalMovementSpeed, _jumpForce, context.ForceJump);
            }


            if (Mathf.Approximately(_inputInfo.InputAxes.y, 1))
            {
                var copy = _inputInfo.InputAxes;
                copy.y = 0;
                _inputInfo.InputAxes = copy;
            }

            if (_inputInfo.InputAbilityTriggerZero)
            {
                _inputInfo.InputAbilityTriggerZero = false;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _inputInfo.InputAxes = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _inputInfo.InputSprintModifier = context.ReadValueAsButton();
        }
        
        public void OnAbilityTriggerZero(InputAction.CallbackContext context)
        {
            _inputInfo.InputAbilityTriggerZero = context.ReadValueAsButton();
        }

        public void RegisterAbility<TAbility>() where TAbility : PlayerAbilityBehaviour
        {
            _abilities.Add(gameObject.AddComponent<TAbility>());
        }
    }
}
